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

namespace Base.Domain
{
    /// <summary>
    /// 领域服务：地区组
    /// </summary>
    public class SysAreaGroupManager : BaseManager, ISysAreaGroupManager
    {
        private readonly IMapper _mapper;
        private readonly ISysAreaGroupRepository _groupRepository;

        public SysAreaGroupManager(
            IMapper mapper,
            ISysAreaGroupRepository groupRepository)
        {
            _mapper = mapper;
            _groupRepository = groupRepository;
        }
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <returns>分页列表</returns>
        public async Task<PageList<SysAreaGroup>> GetPageAsync(int pageIndex, int pageSize, string key)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            return await _groupRepository.GetPageAsync(pageIndex, pageSize, key);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="tenantId">租户id</param>
        /// <param name="entity">地区组</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> AddAsync(Guid tenantId, SysAreaGroupForm entity)
        {
            var data = await _groupRepository.GetByNameAsync(entity.Name);
            if (data != null) return BaseErrType.DataExist;

            data = _mapper.Map<SysAreaGroupForm, SysAreaGroup>(entity);
            data.Id = Guid.NewGuid();
            data.SysTenantId = tenantId;
            return await ResultAsync(() => _groupRepository.AddAsync(data));
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">地区组</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> UpdateAsync(SysAreaGroupForm entity)
        {
            var data = await _groupRepository.GetByNameAsync(entity.Name);
            if (data != null && data.Id != entity.Id) return BaseErrType.DataExist;

            data = await _groupRepository.GetWithAreaContactsAsync(entity.Id);
            data.MapFrom(entity);
            data.SysAreaGroupContacts.Clear();
            return await ResultAsync(() => _groupRepository.UpdateAsync(data));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">地区组id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> DeleteAsync(IEnumerable<Guid> ids)
        {
            var data = await _groupRepository.GetListAsync(ids);
            if (data == null) return BaseErrType.DataNotFound;

            return await ResultAsync(() => _groupRepository.DeleteRangeAsync(data));
        }
    }
}
