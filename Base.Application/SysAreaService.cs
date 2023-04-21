using AutoMapper;
using Sys.Application.Dtos;
using Sys.Application.Interfaces;
using Sys.Domain.AggregateRoots;
using Sys.Domain.Interfaces;
using Sys.Domain.Models;
using OneForAll.Core;
using OneForAll.Core.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Application
{
    /// <summary>
    /// 地区
    /// </summary>
    public class SysAreaService : ISysAreaService
    {
        private readonly IMapper _mapper;
        private readonly ISysAreaManager _areaManageer;

        public SysAreaService(
            IMapper mapper,
            ISysAreaManager areaManageer)
        {
            _mapper = mapper;
            _areaManageer = areaManageer;
        }

        /// <summary>
        /// 获取省份
        /// </summary>
        /// <returns>列表</returns>
        public async Task<IEnumerable<SysAreaSelectionDto>> GetListProvinceAsync()
        {
            var data = await _areaManageer.GetChildrenAsync(0);
            return _mapper.Map<IEnumerable<SysArea>, IEnumerable<SysAreaSelectionDto>>(data);
        }

        /// <summary>
        /// 获取子级
        /// </summary>
        /// <param name="parentId">父级id</param>
        /// <returns>列表</returns>
        public async Task<IEnumerable<SysAreaSelectionDto>> GetChildrenAsync(int parentId)
        {
            var data = await _areaManageer.GetChildrenAsync(parentId);
            return _mapper.Map<IEnumerable<SysArea>, IEnumerable<SysAreaSelectionDto>>(data);
        }
    }
}
