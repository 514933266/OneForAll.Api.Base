using OneForAll.Core;
using OneForAll.Core.DDD;
using OneForAll.Core.Extension;
using OneForAll.Core.Security;
using Sys.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Sys.Domain.AggregateRoots
{
    /// <summary>
    /// 基础表：租户用户
    /// </summary>
    public partial class SysUser : AggregateRoot<Guid>
    {
        /// <summary>
        /// 租户id
        /// </summary>
        [Required]
        public Guid SysTenantId { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [MaxLength(32)]
        public string Password { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [Required]
        [MaxLength(300)]
        public string IconUrl { get; set; } = "";

        /// <summary>
        /// 个性签名
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Signature { get; set; } = "";

        /// <summary>
        /// 用户状态（关联BaseErrType，1正常 0异常 -20006禁止登录)
        /// </summary>
        [Required]
        public int Status { get; set; }

        /// <summary>
        /// 是否默认（默认用户禁止删除）
        /// </summary>
        [Required]
        public bool IsDefault { get; set; }

        /// <summary>
        /// 最后登陆时间
        /// </summary>
        [Column(TypeName ="datetime")]
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 最后登陆Ip
        /// </summary>
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string LastLoginIp { get; set; } = "";

        /// <summary>
        /// 状态最后更新时间（Status）
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 密码输入错误次数
        /// </summary>
        [Required]
        public byte PwdErrCount { get; set; }

        /// <summary>
        /// 上传头像
        /// </summary>
        /// <param name="url">头像地址</param>
        public void UploadHeader(string url)
        {
            IconUrl = url;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="password">密码</param>
        /// <returns>结果</returns>
        public BaseErrType ChangePassword(Password password)
        {
            if (password.Repeat != password.New)
            {
                return BaseErrType.DataNotMatch;
            }
            else if (password.Old == Password)
            {
                Password = password.New;
                return BaseErrType.Success;
            }
            return BaseErrType.PasswordInvalid;
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        public void ResetPassword()
        {
            Password = UserName.ToMd5();
        }
    }
}
