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

namespace Base.Domain
{
    /// <summary>
    /// 领域服务：角色
    /// </summary>
    public class SysTenantRoleManager : BaseManager, ISysTenantRoleManager
    {
        private readonly IMapper _mapper;
        private readonly ISysRoleRepository _roleRepository;
        private readonly ISysRoleUserContactRepository _roleUserRepository;
        public SysTenantRoleManager(
            IMapper mapper,
            ISysRoleRepository roleRepository,
            ISysRoleUserContactRepository roleUserRepository)
        {
            _mapper = mapper;
            _roleRepository = roleRepository;
            _roleUserRepository = roleUserRepository;
        }

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <returns>角色分页</returns>
        public async Task<PageList<SysRoleAggr>> GetPageAsync(int pageIndex, int pageSize, string key)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;
            var data = await _roleRepository.GetPageAsync(pageIndex, pageSize, key);
            var result = _mapper.Map<IEnumerable<SysRole>, IEnumerable<SysRoleAggr>>(data.Items);
            var rids = data.Items.Select(s => s.Id).ToList();
            var counts = await _roleUserRepository.GetListRoleUserCountAsync(rids);
            result.ForEach(e =>
            {
                var item = counts.FirstOrDefault(w => w.SysRoleId == e.Id);
                if (item != null)
                {
                    e.MemberCount = item.MemberCount;
                }
            });
            return new PageList<SysRoleAggr>(data.Total, data.PageIndex, data.PageSize, result);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>角色列表</returns>
        public async Task<IEnumerable<SysRoleAggr>> GetListAsync(string key)
        {
            var data = await _roleRepository.GetListAsync(key);
            var result = _mapper.Map<IEnumerable<SysRole>, IEnumerable<SysRoleAggr>>(data);
            var rids = data.Select(s => s.Id).ToList();
            var counts = await _roleUserRepository.GetListRoleUserCountAsync(rids);
            result.ForEach(e =>
            {
                var item = counts.FirstOrDefault(w => w.SysRoleId == e.Id);
                if (item != null)
                {
                    e.MemberCount = item.MemberCount;
                }
            });
            return result;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="tenantId">租户id</param>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> AddAsync(Guid tenantId, SysRoleForm form)
        {
            var data = await _roleRepository.GetByNameAsync(form.Name);
            if (data != null) return BaseErrType.DataExist;

            data = _mapper.Map<SysRoleForm, SysRole>(form);
            data.SysTenantId = tenantId;
            return await ResultAsync(()=> _roleRepository.AddAsync(data));
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> UpdateAsync(SysRoleForm form)
        {
            var data = await _roleRepository.GetByNameAsync(form.Name);
            if (data != null && data.Id != form.Id) return BaseErrType.DataExist;

            data = await _roleRepository.FindAsync(form.Id);
            if (data == null) return BaseErrType.DataNotFound;

            data.MapFrom(form);
            return await ResultAsync(() => _roleRepository.UpdateAsync(data));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">用户id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> DeleteAsync(IEnumerable<Guid> ids)
        {
            var data = await _roleRepository.GetListAsync(ids);
            return await ResultAsync(() => _roleRepository.DeleteRangeAsync(data));
        }
    }
}
