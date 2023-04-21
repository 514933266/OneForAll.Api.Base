using AutoMapper;
using Base.Domain.AggregateRoots;
using Base.Domain.Interfaces;
using Base.Domain.Repositorys;
using OneForAll.Core;
using OneForAll.Core.DDD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain
{
    /// <summary>
    /// 领域服务：地区组权限
    /// </summary>
    public class SysAreaGroupPermissionManager : BaseManager, ISysAreaGroupPermissionManager
    {
        private readonly ISysAreaRepository _areaRepository;
        private readonly ISysAreaGroupRepository _areaGroupRepository;
        public SysAreaGroupPermissionManager(
            ISysAreaRepository areaRepository,
            ISysAreaGroupRepository areaGroupRepository)
        {
            _areaRepository = areaRepository;
            _areaGroupRepository = areaGroupRepository;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="groupId">地区组id</param>
        /// <returns>权限列表</returns>
        public async Task<IEnumerable<SysArea>> GetListAsync(Guid groupId)
        {
            var group = await _areaGroupRepository.GetWithAreaContactsAsync(groupId);
            if (group != null)
            {
                var codes = group.SysAreaGroupContacts.Select(s => s.AreaCode);
                var areas = await _areaRepository.GetListAsync(codes);
                return areas;
            }
            return new List<SysArea>();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="groupId">地区组id</param>
        /// <param name="areaCodes">地区代码</param>
        /// <returns>权限列表</returns>
        public async Task<BaseErrType> AddAsync(Guid groupId, IEnumerable<string> areaCodes)
        {
            var data = await _areaGroupRepository.GetWithAreaContactsAsync(groupId);
            if (data != null)
            {
                var areas = await _areaRepository.GetListAsync(areaCodes);
                data.SysAreaGroupContacts.Clear();
                data.AddArea(areas);
                return await ResultAsync(() => _areaGroupRepository.UpdateAsync(data));
            }
            return BaseErrType.DataNotFound;
        }
    }
}
