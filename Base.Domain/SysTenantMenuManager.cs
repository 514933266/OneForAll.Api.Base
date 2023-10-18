using AutoMapper;
using Base.Domain.AggregateRoots;
using Base.Domain.Interfaces;
using Base.Domain.Repositorys;
using OneForAll.Core.DDD;
using OneForAll.Core.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Domain.Aggregates;
using Microsoft.AspNetCore.Http;
using NPOI.SS.Formula.Functions;

namespace Base.Domain
{
    /// <summary>
    /// 领域服务：机构菜单
    /// </summary>
    public class SysTenantMenuManager: SysBaseManager, ISysTenantMenuManager
    {
        private readonly ISysMenuRepository _menuRepository;
        private readonly ISysPermissionRepository _permRepository;
        private readonly ISysTenantPermContactRepository _tenantPermRepository;
        public SysTenantMenuManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ISysMenuRepository menuRepository,
            ISysPermissionRepository permRepository,
            ISysTenantPermContactRepository tenantPermRepository) : base(mapper, httpContextAccessor)
        {
            _menuRepository = menuRepository;
            _permRepository = permRepository;
            _tenantPermRepository = tenantPermRepository;
        }

        /// <summary>
        /// 获取树列表
        /// </summary>
        /// <returns>列表</returns>
        public async Task<IEnumerable<SysMenuPermissionAggr>> GetListAsync()
        {
            var pids = await _tenantPermRepository.GetListPermissionIdAsync(LoginUser.SysTenantId);
            var data = await _menuRepository.GetListByTenantAsync(LoginUser.SysTenantId);
            var result = _mapper.Map<IEnumerable<SysMenu>, IEnumerable<SysMenuPermissionAggr>>(data);
            // 查询带权限时，只显示启用的菜单
            result = result.DistinctBy(d => d.Id).Where(w => w.IsEnabled).ToList();
            var ids = data.Select(s => s.Id).ToList();
            var perms = await _permRepository.GetListByMenuAsync(ids);
            result.ForEach(e =>
            {
                e.SysPermissions = perms.Where(w => w.SysMenuId == e.Id && pids.Contains(w.Id)).OrderByDescending(o => o.SortCode).ToList();
            });
            return result;
        }
    }
}
