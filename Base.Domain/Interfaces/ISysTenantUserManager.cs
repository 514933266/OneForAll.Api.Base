using Base.Domain.AggregateRoots;
using Base.Domain.Models;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Base.Domain.Aggregates;

namespace Base.Domain.Interfaces
{
    /// <summary>
    /// 领域服务：子机构用户
    /// </summary>
    public interface ISysTenantUserManager
    {
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <returns>分页</returns>
        Task<PageList<SysLoginUserAggr>> GetPageAsync(int pageIndex, int pageSize, string key);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>用户列表</returns>
        Task<IEnumerable<SysUser>> GetListAsync(string key);

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns>用户列表</returns>
        Task<SysUser> GetAsync(Guid id);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        Task<BaseMessage> AddAsync(SysUserForm form);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        Task<BaseMessage> UpdateAsync(SysUserUpdateForm form);

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
