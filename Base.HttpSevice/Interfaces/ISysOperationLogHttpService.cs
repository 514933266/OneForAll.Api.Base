using Base.HttpService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.HttpService.Interfaces
{
    /// <summary>
    /// 操作日志
    /// </summary>
    public interface ISysOperationLogHttpService
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        Task AddAsync(SysOperationLogRequest entity);
    }
}
