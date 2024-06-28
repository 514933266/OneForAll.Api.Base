using AutoMapper;
using Base.Domain.AggregateRoots;
using Base.Domain.Interfaces;
using Base.Domain.Models;
using Base.Domain.Repositorys;
using OneForAll.Core;
using OneForAll.Core.DDD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneForAll.Core.Extension;
using OneForAll.EFCore;
using Microsoft.AspNetCore.Http;
using NPOI.SS.Formula.Functions;
using OneForAll.Core.Upload;

namespace Base.Domain
{
    /// <summary>
    /// 领域服务：角色权限
    /// </summary>
    public class SysRolePermissionManager : SysBaseManager, ISysRolePermissionManager
    {
        private readonly ISysMenuRepository _menuRepository;
        private readonly ISysRoleRepository _roleRepository;
        private readonly ISysPermissionRepository _permRepository;
        private readonly ISysRolePermContactRepository _rolePermRepository;
        public SysRolePermissionManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ISysMenuRepository menuRepository,
            ISysRoleRepository roleRepository,
            ISysPermissionRepository permRepository,
            ISysRolePermContactRepository rolePermRepository) : base(mapper, httpContextAccessor)
        {
            _menuRepository = menuRepository;
            _roleRepository = roleRepository;
            _permRepository = permRepository;
            _rolePermRepository = rolePermRepository;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <returns>权限列表</returns>
        public async Task<IEnumerable<SysPermission>> GetListAsync(Guid roleId)
        {
            return await _rolePermRepository.GetListPermissionAsync(roleId);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <param name="pids">权限id</param>
        /// <returns>权限列表</returns>
        public async Task<BaseErrType> AddAsync(Guid roleId, IEnumerable<Guid> pids)
        {
            var data = await _roleRepository.FindAsync(roleId);
            if (data == null)
                return BaseErrType.DataError;

            var rolePerms = await _rolePermRepository.GetListAsync(roleId);
            var addList = pids.Select(s => new SysRolePermContact() { SysRoleId = roleId, SysPermissionId = s }).ToList();

            using (var tran = new UnitOfWork().BeginTransaction())
            {
                if (rolePerms.Any())
                    await _rolePermRepository.DeleteRangeAsync(rolePerms, tran);
                if (addList.Any())
                    await _rolePermRepository.AddRangeAsync(addList, tran);

                return await ResultAsync(tran.CommitAsync);
            }
        }

        // 从下至上查找所有父级菜单
        private IEnumerable<SysMenu> FindAllMenus(IEnumerable<Guid> targetIds, IEnumerable<SysMenu> sources)
        {
            var result = new List<SysMenu>();
            var data = sources.Where(w => targetIds.Contains(w.Id)).ToList();
            if (data.Any())
            {
                result.AddRange(data);
                var pids = data.Select(s => s.ParentId).ToList();
                if (pids.Any())
                {
                    var parents = FindAllMenus(pids, sources);
                    if (parents.Any())
                        result.AddRange(parents);
                }
            }
            return result;
        }
    }
}
