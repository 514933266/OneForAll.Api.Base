using Sys.Application.Dtos;
using Sys.Domain.Models;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Sys.Application.Interfaces
{
    /// <summary>
    /// 用户
    /// </summary>
    public interface ISysTenantUserService
    {
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <returns>分页</returns>
        Task<PageList<SysTenantUserDto>> GetPageAsync(int pageIndex, int pageSize, string key);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="ids">实体id</param>
        /// <returns>结果</returns>
        Task<IEnumerable<SysTenantUserDto>> GetListAsync(IEnumerable<Guid> ids);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="form">用户</param>
        /// <returns>结果</returns>
        Task<BaseErrType> AddAsync(SysUserForm form);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="form">用户</param>
        /// <returns>结果</returns>
        Task<BaseErrType> UpdateAsync(SysUserUpdateForm form);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">用户id</param>
        /// <returns>结果</returns>
        Task<BaseErrType> DeleteAsync(IEnumerable<Guid> ids);

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="ids">用户id</param>
        /// <returns>结果</returns>
        Task<BaseErrType> ResetPasswordAsync(IEnumerable<Guid> ids);
    }
}
