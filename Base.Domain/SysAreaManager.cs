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
    /// 领域服务：地区
    /// </summary>
    public class SysAreaManager : BaseManager, ISysAreaManager
    {
        private readonly ISysAreaRepository _areaRepository;
        public SysAreaManager(
            ISysAreaRepository areaRepository)
        {
            _areaRepository = areaRepository;
        }

        /// <summary>
        /// 获取省份
        /// </summary>
        /// <returns>列表</returns>
        public async Task<IEnumerable<SysArea>> GetListProvinceAsync()
        {
            return await _areaRepository.GetChildrenAsync(0);
        }

        /// <summary>
        /// 获取子级
        /// </summary>
        /// <param name="parentId">父级id</param>
        /// <returns>列表</returns>

        public async Task<IEnumerable<SysArea>> GetChildrenAsync(int parentId)
        {
            return await _areaRepository.GetChildrenAsync(parentId);
        }
    }
}
