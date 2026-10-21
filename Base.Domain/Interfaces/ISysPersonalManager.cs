using Base.Domain.Entities;
using Base.Domain.Aggregates;
using Base.Domain.Models;
using Base.Domain.ValueObjects;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using OneForAll.Core.UploadFile;

namespace Base.Domain.Interfaces
{
    /// <summary>
    /// 个人中心领域
    /// </summary>
    public interface ISysPersonalManager
    {
        /// <summary>
        /// 修改个人信息
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        Task<BaseErrType> UpdateAsync(SysPersonalForm form);

        /// <summary>
        /// 上传头像
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="file">文件流</param>
        /// <returns>上传结果</returns>
        Task<IUploadResult> UploadHeaderAsync(string filename, Stream file);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="password">密码</param>
        /// <returns>结果</returns>
        Task<BaseErrType> ChangePasswordAsync(ResetPasswordVo password);

        /// <summary>
        /// 更换所属租户
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        Task<BaseErrType> UpdateTenantAsync(Guid tenantId);

        /// <summary>
        /// 发起修改手机号：校验可否发送验证码并生成验证码（短信发送归 Application 编排）
        /// </summary>
        /// <param name="mobile">目标手机号</param>
        Task<(BaseErrType Err, string Code)> BeginSendMobileUpdateCodeAsync(string mobile);

        /// <summary>
        /// 确认验证码短信已发出并记录验证码（仅发送成功后调用）
        /// </summary>
        /// <param name="mobile">目标手机号</param>
        /// <param name="code">验证码</param>
        Task ConfirmMobileUpdateCodeSentAsync(string mobile, string code);

        /// <summary>
        /// 提交修改手机号：校验验证码/手机号一致性/占用
        /// </summary>
        /// <param name="form">表单</param>
        Task<BaseErrType> CommitMobileUpdateCodeAsync(SysPersonalUpdateMobileForm form);

        /// <summary>
        /// 变更手机号（领域写操作）
        /// </summary>
        /// <param name="mobile">新手机号</param>
        Task<BaseErrType> ChangeMobileAsync(string mobile);
    }
}
