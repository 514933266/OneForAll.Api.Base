using Base.Domain.ValueObjects;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Interfaces
{
    /// <summary>
    /// 领域服务：权限校验
    /// </summary>
    public interface ISysPermissionCheckManager
    {
        /// <summary>
        /// 校验用户权限
        /// </summary>
        /// <param name="form">验证实体</param>
        /// <returns>结果</returns>
        Task<BaseErrType> ValidateAsync(SysPermissionCheck form);
    }
}
