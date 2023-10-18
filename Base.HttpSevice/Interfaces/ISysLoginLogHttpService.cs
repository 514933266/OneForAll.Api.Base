using Base.HttpService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.HttpService.Interfaces
{
    /// <summary>
    /// 登录日志
    /// </summary>
    public interface ISysLoginLogHttpService
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns></returns>
        Task AddAsync(SysLoginLogForm form);
    }
}
