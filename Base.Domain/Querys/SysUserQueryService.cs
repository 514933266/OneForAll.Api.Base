using AutoMapper;
using Base.Domain.Aggregates;
using Base.Domain.Entities;
using Base.Domain.Interfaces;
using Base.Domain.Repositorys;
using Base.Domain.ValueObjects;
using Microsoft.Extensions.Caching.Distributed;
using OneForAll.Core;
using OneForAll.Core.Extension;
using OneForAll.Core.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Base.Domain.Querys
{
    /// <summary>
    /// 读路径服务实现：从 Entity 组装 Aggr，返回不含 DTO；分页批量装配消除 N+1
    /// </summary>
    public class SysUserQueryService : ISysUserQueryService
    {
        // 登录态读缓存键（与旧 PersonalManager/PermissionCheckManager 保持一致）
        private readonly string LOGIN_CACHE_KEY = "LoginInfo:{0}";

        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;
        private readonly IDistributedCache _cacheRepository;
        private readonly ISysUserRepository _userRepository;
        private readonly ISysTenantRepository _tenantRepository;
        private readonly ISysMenuRepository _menuRepository;
        private readonly ISysPermissionRepository _permRepository;
        private readonly ISysMidRoleUserRepository _roleUserRepository;
        private readonly ISysMidUserPermissionRepository _userPermRepository;
        private readonly ISysMidRolePermissionRepository _rolePermRepository;
        private readonly ISysMidTenantPermissionRepository _tenantPermRepository;

        public SysUserQueryService(
            IMapper mapper,
            ICurrentUser currentUser,
            IDistributedCache cacheRepository,
            ISysUserRepository userRepository,
            ISysTenantRepository tenantRepository,
            ISysMenuRepository menuRepository,
            ISysPermissionRepository permRepository,
            ISysMidRoleUserRepository roleUserRepository,
            ISysMidUserPermissionRepository userPermRepository,
            ISysMidRolePermissionRepository rolePermRepository,
            ISysMidTenantPermissionRepository tenantPermRepository)
        {
            _mapper = mapper;
            _currentUser = currentUser;
            _cacheRepository = cacheRepository;
            _userRepository = userRepository;
            _tenantRepository = tenantRepository;
            _menuRepository = menuRepository;
            _permRepository = permRepository;
            _roleUserRepository = roleUserRepository;
            _userPermRepository = userPermRepository;
            _rolePermRepository = rolePermRepository;
            _tenantPermRepository = tenantPermRepository;
        }

        // 当前登录用户（通过 ICurrentUser 端口获取，不直接依赖 HTTP）
        private LoginUser LoginUser
        {
            get { return _currentUser?.LoginUser ?? new LoginUser(); }
        }

        /// <summary>
        /// 组装单个登录用户聚合（角色权限 ∪ 直授权限；tenantFilterId 非空时与租户权限求交）
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="tenantFilterId">租户权限过滤</param>
        /// <returns>登录用户聚合，用户不存在返回 null</returns>
        public async Task<SysLoginUserAggr> GetLoginUserAsync(Guid userId, Guid? tenantFilterId = null)
        {
            var user = await _userRepository.GetByIdIQFAsync(userId);
            if (user == null)
                return null;

            // 角色权限 ∪ 直授权限
            var pids = await _roleUserRepository.GetListPermIdByUserAsync(userId);
            var pids2 = await _userPermRepository.GetListPermIdByUserAsync(userId);
            var permIds = pids.Concat(pids2);
            // 个人中心场景：与租户可用权限求交
            if (tenantFilterId.HasValue)
            {
                var tpids = await _tenantPermRepository.GetListPermissionIdAsync(tenantFilterId.Value);
                permIds = permIds.Intersect(tpids);
            }

            var distinctIds = permIds.Distinct().ToList();
            var permissions = distinctIds.Any()
                ? await _permRepository.GetListAsync(distinctIds)
                : new List<SysPermissionAggr>();

            // 单用户：加载租户导航（还原原投影聚合语义）；不做菜单排序（与个人中心/权限校验原逻辑一致）
            var tenant = await _tenantRepository.GetIQFAsync(user.SysTenantId);
            var loginUser = _mapper.Map<SysUser, SysLoginUserAggr>(user);
            loginUser.SysTenant = tenant;
            loginUser.SetPermissons(permissions);
            return loginUser;
        }

        /// <summary>
        /// 分页组装登录用户聚合（一次批量装配消除 N+1）
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <returns>分页的登录用户聚合</returns>
        public async Task<PageList<SysLoginUserAggr>> GetPagedLoginUsersAsync(int pageIndex, int pageSize, string key)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var data = await _userRepository.GetPageAsync(pageIndex, pageSize, key);
            var users = data.Items == null ? new List<SysUser>() : data.Items.ToList();
            var result = new List<SysLoginUserAggr>();
            if (!users.Any())
                return new PageList<SysLoginUserAggr>(data.Total, data.PageIndex, data.PageSize, result);

            var userIds = users.Select(u => u.Id).ToList();

            // 一次批量查租户（导航）
            var tenantIds = users.Select(u => u.SysTenantId).Distinct().ToList();
            var tenants = await _tenantRepository.GetListAsync(tenantIds);
            var tenantMap = new Dictionary<Guid, SysTenant>();
            foreach (var t in tenants)
                if (t != null && !tenantMap.ContainsKey(t.Id))
                    tenantMap[t.Id] = t;

            // 一次批量查 用户→角色
            var roleUsers = await _roleUserRepository.GetListByUserAsync(userIds);
            var userRoleMap = roleUsers
                .GroupBy(ru => ru.SysUserId)
                .ToDictionary(g => g.Key, g => g.Select(x => x.SysRoleId).Distinct().ToList());
            var allRoleIds = roleUsers.Select(ru => ru.SysRoleId).Distinct().ToList();

            // 一次批量查 角色→权限
            var rolePerms = await _rolePermRepository.GetListByRolesAsync(allRoleIds);
            var rolePermMap = rolePerms
                .GroupBy(rp => rp.SysRoleId)
                .ToDictionary(g => g.Key, g => g.Select(x => x.SysPermissionId).ToList());

            // 一次批量查 用户→直授权限
            var userPerms = await _userPermRepository.GetListByUsersAsync(userIds);
            var userPermMap = userPerms
                .GroupBy(up => up.SysUserId)
                .ToDictionary(g => g.Key, g => g.Select(x => x.SysPermissionId).ToList());

            // 一次批量查 权限详情
            var allPermIds = rolePerms.Select(rp => rp.SysPermissionId)
                .Concat(userPerms.Select(up => up.SysPermissionId))
                .Distinct().ToList();
            var permissions = allPermIds.Any()
                ? await _permRepository.GetListAsync(allPermIds)
                : new List<SysPermissionAggr>();
            var permDict = new Dictionary<Guid, SysPermissionAggr>();
            foreach (var p in permissions)
                if (p != null && !permDict.ContainsKey(p.Id))
                    permDict[p.Id] = p;

            // 内存分组组装：逐用户合并权限并装配（保持原逐条装配的排序语义）
            foreach (var user in users)
            {
                var myPermIds = new List<Guid>();
                if (userRoleMap.TryGetValue(user.Id, out var myRoleIds))
                {
                    foreach (var rid in myRoleIds)
                        if (rolePermMap.TryGetValue(rid, out var rpid))
                            myPermIds.AddRange(rpid);
                }
                if (userPermMap.TryGetValue(user.Id, out var directIds))
                    myPermIds.AddRange(directIds);

                var myPerms = myPermIds.Distinct()
                    .Where(pid => permDict.ContainsKey(pid))
                    .Select(pid => permDict[pid])
                    .ToList();

                var loginUser = _mapper.Map<SysUser, SysLoginUserAggr>(user);
                if (tenantMap.TryGetValue(user.SysTenantId, out var tenant))
                    loginUser.SysTenant = tenant;
                loginUser.SetPermissons(myPerms);
                loginUser.SysLoginUserMenus = loginUser.SysLoginUserMenus.OrderBy(w => w.Code).ToList();
                loginUser.SysLoginUserMenus.ForEach(e =>
                {
                    e.SysLoginUserPermissions = e.SysLoginUserPermissions.OrderBy(o => o.SortCode).ToList();
                });
                result.Add(loginUser);
            }

            return new PageList<SysLoginUserAggr>(data.Total, data.PageIndex, data.PageSize, result);
        }

        /// <summary>
        /// 获取当前用户可见菜单（含上级菜单，按排序号升序去重）
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="tenantFilterId">租户权限过滤</param>
        /// <returns>菜单列表</returns>
        public async Task<IEnumerable<SysMenu>> GetMenusAsync(Guid userId, Guid tenantFilterId)
        {
            var menus = new List<SysMenu>();
            var user = await GetLoginUserAsync(userId, tenantFilterId);
            if (user != null)
            {
                var ids = user.SysLoginUserMenus.Select(s => s.Id).ToList();
                var data = await _menuRepository.GetListAsync();
                // 拥有下级菜单权限，代表拥有上级菜单
                ids.ForEach(id =>
                {
                    var userMenus = GetUserMenus(id, data);
                    menus.AddRange(userMenus);
                });
            }
            return menus.OrderBy(o => o.SortNumber).Distinct();
        }

        /// <summary>
        /// 当前登录用户个人信息：组装最新聚合（含租户权限求交）并回填 LoginInfo 缓存
        /// </summary>
        public async Task<SysLoginUserAggr> GetPersonalAsync()
        {
            // 不读缓存，保证每次拿到最新权限；写缓存不设过期（与旧 PersonalManager.GetAsync 一致）
            var loginUser = await GetLoginUserAsync(LoginUser.Id.TryGuid(), LoginUser.TenantId.TryGuid());
            var cacheKey = LOGIN_CACHE_KEY.Fmt(LoginUser.Id);
            await _cacheRepository.SetStringAsync(cacheKey, loginUser.ToJson());
            return loginUser;
        }

        /// <summary>
        /// 当前登录用户可见菜单（含上级展开）
        /// </summary>
        public async Task<IEnumerable<SysMenu>> GetPersonalMenusAsync()
        {
            return await GetMenusAsync(LoginUser.Id.TryGuid(), LoginUser.TenantId.TryGuid());
        }

        /// <summary>
        /// 校验当前用户功能权限（LoginInfo 读透缓存 + 权限求并，不加载租户过滤）
        /// </summary>
        public async Task<BaseErrType> ValidateAsync(SysPermissionCheck form)
        {
            var action = form.Action.Replace("Async", "");
            var cacheKey = LOGIN_CACHE_KEY.Fmt(LoginUser.Id);
            var loginUser = new SysLoginUserAggr();
            try
            {
                var cache = await _cacheRepository.GetStringAsync(cacheKey);
                if (cache.IsNullOrEmpty())
                {
                    // 没有缓存，直接读库
                    loginUser = await GetLoginUserAsync(LoginUser.Id.TryGuid());
                    await _cacheRepository.SetStringAsync(cacheKey, loginUser.ToJson(), new DistributedCacheEntryOptions() { SlidingExpiration = TimeSpan.FromMinutes(30) });
                }
                else
                {
                    loginUser = cache.FromJson<SysLoginUserAggr>();
                    if (!loginUser.SysLoginUserMenus.Any())
                    {
                        loginUser = await GetLoginUserAsync(LoginUser.Id.TryGuid());
                        await _cacheRepository.SetStringAsync(cacheKey, loginUser.ToJson(), new DistributedCacheEntryOptions() { SlidingExpiration = TimeSpan.FromMinutes(30) });
                    }
                }
            }
            catch
            {
                // redis 未启动，直接读库
                loginUser = await GetLoginUserAsync(LoginUser.Id.TryGuid());
            }
            return loginUser.ValidatePermission(form.Controller, action);
        }

        /// <summary>
        /// 退出登录：清除当前用户 LoginInfo 缓存
        /// </summary>
        public async Task<BaseErrType> LoginoutAsync()
        {
            try
            {
                var cacheKey = LOGIN_CACHE_KEY.Fmt(LoginUser.Id);
                var data = await _cacheRepository.GetAsync(cacheKey);
                if (data != null)
                {
                    await _cacheRepository.RemoveAsync(cacheKey);
                }
            }
            catch
            {

            }
            return BaseErrType.Success;
        }

        // 获取菜单以及上级
        private IEnumerable<SysMenu> GetUserMenus(Guid id, IEnumerable<SysMenu> sources)
        {
            var result = new List<SysMenu>();
            var data = sources.FirstOrDefault(w => w.Id.Equals(id));
            if (data != null)
            {
                result.Add(data);
                if (data.ParentId != Guid.Empty)
                {
                    var parents = GetUserMenus(data.ParentId, sources);
                    result.AddRange(parents);
                }
            }
            return result;
        }
    }
}
