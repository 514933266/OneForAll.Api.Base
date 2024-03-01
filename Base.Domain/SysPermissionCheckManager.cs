using AutoMapper;
using Base.Domain.AggregateRoots;
using Base.Domain.Aggregates;
using Base.Domain.Interfaces;
using Base.Domain.Models;
using Base.Domain.Repositorys;
using Base.Domain.ValueObjects;
using Microsoft.Extensions.Caching.Distributed;
using OneForAll.Core;
using OneForAll.Core.Extension;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Base.Domain
{
    /// <summary>
    /// 领域服务：权限校验
    /// </summary>
    public class SysPermissionCheckManager : SysBaseManager, ISysPermissionCheckManager
    {
        private readonly string CACHE_KEY = "LoginInfo:{0}";

        private readonly IDistributedCache _cacheRepository;
        private readonly ISysUserRepository _userRepository;
        private readonly ISysPermissionRepository _permRepository;
        private readonly ISysRoleUserContactRepository _roleUserRepository;
        private readonly ISysUserPermContactRepository _userPermRepository;

        public SysPermissionCheckManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IDistributedCache cacheRepository,
            ISysUserRepository userRepository,
            ISysPermissionRepository permRepository,
            ISysRoleUserContactRepository roleUserRepository,
            ISysUserPermContactRepository userPermRepository) : base(mapper, httpContextAccessor)
        {
            _userRepository = userRepository;
            _cacheRepository = cacheRepository;
            _permRepository = permRepository;
            _roleUserRepository = roleUserRepository;
            _userPermRepository = userPermRepository;
        }

        /// <summary>
        /// 校验用户权限
        /// </summary>
        /// <param name="form">验证实体</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> ValidateAsync(SysPermissionCheck form)
        {
            var action = form.Action.Replace("Async", "");
            var cacheKey = CACHE_KEY.Fmt(LoginUser.Id);
            var loginUser = new SysLoginUserAggr();
            try
            {
                var cache = await _cacheRepository.GetStringAsync(cacheKey);
                if (cache.IsNullOrEmpty())
                {
                    // 没有缓存，直接读库
                    loginUser = await GetLoginUserAsync();
                    await _cacheRepository.SetStringAsync(cacheKey, loginUser.ToJson(), new DistributedCacheEntryOptions() { SlidingExpiration = TimeSpan.FromMinutes(30) });
                }
                else
                {
                    loginUser = cache.FromJson<SysLoginUserAggr>();
                    if (!loginUser.SysLoginUserMenus.Any())
                    {
                        loginUser = await GetLoginUserAsync();
                        await _cacheRepository.SetStringAsync(cacheKey, loginUser.ToJson(), new DistributedCacheEntryOptions() { SlidingExpiration = TimeSpan.FromMinutes(30) });
                    }
                }
            }
            catch
            {
                // redis 未启动，直接读库
                loginUser = await GetLoginUserAsync();
            }
            return loginUser.ValidatePermission(form.Controller, action);
        }

        private async Task<SysLoginUserAggr> GetLoginUserAsync()
        {
            var user = await _userRepository.GetAsync(LoginUser.Id);
            var pids = await _roleUserRepository.GetListPermIdByUserAsync(LoginUser.Id);
            var pids2 = await _userPermRepository.GetListPermIdByUserAsync(LoginUser.Id);
            pids = pids.Concat(pids2);

            var permissions = await _permRepository.GetListAsync(pids);
            var loginUser = _mapper.Map<SysTenantUserAggr, SysLoginUserAggr>(user);
            loginUser.SetPermissons(permissions);
            return loginUser;
        }
    }
}
