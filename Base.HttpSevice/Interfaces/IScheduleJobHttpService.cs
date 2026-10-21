using Base.HttpService.Models;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.HttpService.Interfaces
{
    /// <summary>
    /// 定时任务调度中心
    /// </summary>
    public interface IScheduleJobHttpService
    {
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="request">实体</param>
        /// <returns></returns>
        Task<BaseErrType> RegisterAsync(JobRegisterRequest request);

        /// <summary>
        /// 定时服务下线
        /// </summary>
        /// <param name="appId">应用程序id</param>
        /// <param name="taskName">定时任务名称</param>
        /// <returns结果</returns>
        Task DownLineAsync(string appId, string taskName);

        /// <summary>
        /// 定时服务运行日志
        /// </summary>
        /// <param name="appId">应用程序id</param>
        /// <param name="taskName">定时任务名称</param>
        /// <param name="log">日志内容</param>
        /// <param name="isException">是否异常日志</param>
        /// <returns结果</returns>
        Task<BaseErrType> LogAsync(string appId, string taskName, string log, bool isException = false);

        /// <summary>
        /// 尝试获取任务执行资格（调度中心统一并发控制，返回成功后执行）
        /// </summary>
        /// <param name="appId">应用程序id</param>
        /// <param name="taskName">定时任务名称</param>
        /// <param name="clientCode">客户端编号</param>
        /// <returns>检查结果（调度中心不可达或接口报错时抛出异常）</returns>
        Task<JobLockCheckResponse> TryAcquireLockAsync(string appId, string taskName, string clientCode);

        /// <summary>
        /// 释放任务执行资格
        /// </summary>
        /// <param name="appId">应用程序id</param>
        /// <param name="taskName">定时任务名称</param>
        /// <param name="version">锁版本号</param>
        /// <returns>结果</returns>
        Task<BaseErrType> ReleaseLockAsync(string appId, string taskName, int version);
    }
}