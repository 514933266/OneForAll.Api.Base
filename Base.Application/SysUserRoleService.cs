using AutoMapper;
using Base.Application.Dtos;
using Base.Application.Interfaces;
using Base.Domain.AggregateRoots;
using Base.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Application
{
    /// <summary>
    /// 用户角色
    /// </summary>
    public class SysUserRoleService : ISysUserRoleService
    {
        private readonly IMapper _mapper;
        private readonly ISysUserRoleManager _manager;
        public SysUserRoleService(
            IMapper mapper,
            ISysUserRoleManager manager)
        {
            _mapper = mapper;
            _manager = manager;
        }

        /// <summary>
        /// 获取用户角色列表
        /// </summary>
        /// <param name="userIds">用户id</param>
        /// <returns>角色列表</returns>
        public async Task<IEnumerable<SysUserRoleDto>> GetListAsync(IEnumerable<Guid> userIds)
        {
            var data = await _manager.GetListAsync(userIds);
            return _mapper.Map<IEnumerable<SysRole>, IEnumerable<SysUserRoleDto>>(data);
        }
    }
}
