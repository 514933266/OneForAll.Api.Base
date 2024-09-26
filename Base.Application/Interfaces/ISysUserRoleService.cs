using Base.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Application.Interfaces
{
    /// <summary>
    /// 用户角色
    /// </summary>
    public interface ISysUserRoleService
    {
        /// <summary>
        /// 获取用户角色列表
        /// </summary>
        /// <param name="userIds">用户id</param>
        /// <returns>角色列表</returns>
        Task<IEnumerable<SysUserRoleDto>> GetListAsync(IEnumerable<Guid> userIds);
    }
}
