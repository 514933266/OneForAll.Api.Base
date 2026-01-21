using AutoMapper;
using Base.Domain.Aggregates;
using Base.Domain.Interfaces;
using Base.Domain.Repositorys;
using Microsoft.AspNetCore.Http;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain
{
    /// <summary>
    /// 用户角色
    /// </summary>
    public class SysUserRoleManager : BaseManager, ISysUserRoleManager
    {
        private readonly IMapper _mapper;
        private readonly ISysRoleRepository _roleRepository;
        private readonly ISysMidRoleUserRepository _roleUserRepository;
        public SysUserRoleManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ISysRoleRepository roleRepository,
            ISysMidRoleUserRepository roleUserRepository) : base(httpContextAccessor)
        {
            _mapper = mapper;
            _roleRepository = roleRepository;
            _roleUserRepository = roleUserRepository;
        }

        /// <summary>
        /// 获取用户角色列表
        /// </summary>
        /// <param name="userIds">用户id</param>
        /// <returns>角色列表</returns>
        public async Task<IEnumerable<SysUserRoleAggr>> GetListAsync(IEnumerable<Guid> userIds)
        {
            return await _roleUserRepository.GetListUserRoleAsync(userIds);
        }
    }
}
