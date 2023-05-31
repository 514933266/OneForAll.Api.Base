using AutoMapper;
using Base.Domain.AggregateRoots;
using Base.Domain.Interfaces;
using Base.Domain.Models;
using Base.Domain.Repositorys;
using OneForAll.Core;
using OneForAll.Core.DDD;
using OneForAll.Core.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Domain.Aggregates;
using OneForAll.Core.OAuth;
using Microsoft.AspNetCore.Http;
using NPOI.SS.Formula;

namespace Base.Domain
{
    /// <summary>
    /// 领域服务：子机构用户
    /// </summary>
    public class SysTenantUserManager : SysBaseManager, ISysTenantUserManager
    {
        private readonly ISysUserRepository _userRepository;
        private readonly ISysPermissionRepository _permRepository;
        private readonly ISysRoleUserContactRepository _roleUserRepository;
        private readonly ISysUserPermContactRepository _userPermRepository;
        public SysTenantUserManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ISysUserRepository userRepository,
            ISysPermissionRepository permRepository,
            ISysRoleUserContactRepository roleUserRepository,
            ISysUserPermContactRepository userPermRepository) : base(mapper, httpContextAccessor)
        {
            _userRepository = userRepository;
            _permRepository = permRepository;
            _roleUserRepository = roleUserRepository;
            _userPermRepository = userPermRepository;
        }

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <returns>用户分页</returns>
        public async Task<PageList<SysLoginUserAggr>> GetPageAsync(int pageIndex, int pageSize, string key)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;
            var result = new List<SysLoginUserAggr>();
            var data = await _userRepository.GetPageAsync(pageIndex, pageSize, key);
            var userIds = data.Items.Select(s => s.Id).ToList();
            userIds.ForEach(id =>
            {
                // 后续优化
                var user = GetLoginAsync(id).Result;
                result.Add(user);
            });
            return new PageList<SysLoginUserAggr>(data.Total, data.PageIndex, data.PageSize, result);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>用户列表</returns>
        public async Task<IEnumerable<SysUser>> GetListAsync(string key)
        {
            return await _userRepository.GetListAsync(key);
        }


        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns>用户列表</returns>
        public async Task<SysUser> GetAsync(Guid id)
        {
            return await _userRepository.GetAsync(id);
        }

        // 获取登录个人信息(含菜单、权限)
        private async Task<SysLoginUserAggr> GetLoginAsync(Guid id)
        {
            var user = await _userRepository.GetAsync(id);
            var pids = await _roleUserRepository.GetListPermissionIdByUserAsync(id);
            var pids2 = await _userPermRepository.GetListPermissionIdByUserAsync(id);
            pids = pids.Concat(pids2);

            var permissions = await _permRepository.GetListAsync(pids);
            var loginUser = _mapper.Map<SysTenantUserAggr, SysLoginUserAggr>(user);
            loginUser.SetPermissons(permissions);
            loginUser.SysLoginUserMenus = loginUser.SysLoginUserMenus.OrderBy(w => w.Code).ToList();
            loginUser.SysLoginUserMenus.ForEach(e => { e.SysLoginUserPermissions = e.SysLoginUserPermissions.OrderBy(o => o.SortCode).ToList(); });
            return loginUser;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> AddAsync(SysUserForm form)
        {
            var data = await _userRepository.GetAsync(form.UserName);
            if (data != null)
                return BaseErrType.DataExist;
            if (form.Password != form.RePassword)
                return BaseErrType.DataNotMatch;

            data = _mapper.Map<SysUserForm, SysUser>(form);
            data.SysTenantId = LoginUser.TenantId;
            return await ResultAsync(() => _userRepository.AddAsync(data));
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> UpdateAsync(SysUserUpdateForm form)
        {
            var data = await _userRepository.FindAsync(form.Id);
            if (data == null)
                return BaseErrType.DataNotFound;

            _mapper.Map(form, data);
            return await ResultAsync(() => _userRepository.UpdateAsync(data));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">用户id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> DeleteAsync(IEnumerable<Guid> ids)
        {
            var data = await _userRepository.GetListAsync(ids);
            return await ResultAsync(() => _userRepository.DeleteRangeAsync(data));
        }


        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="ids">用户id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> ResetPasswordAsync(IEnumerable<Guid> ids)
        {
            var data = await _userRepository.GetListAsync(ids);
            data.ForEach(e => { e.ResetPassword(); });
            return await ResultAsync(() => _userRepository.UpdateRangeAsync(data));
        }
    }
}
