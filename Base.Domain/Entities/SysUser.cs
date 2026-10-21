using OneForAll.Core;
using OneForAll.Core.Security;
using OneForAll.Core.Extension;
using Base.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Base.Domain.Entities
{
    /// <summary>
    /// 基础表：租户用户
    /// </summary>
    public partial class SysUser
    {
        /// <summary>
        /// id
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// 租户id
        /// </summary>
        [Required]
        public Guid SysTenantId { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        [Required]
        [StringLength(20)]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [StringLength(32)]
        public string Password { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Mobile { get; set; } = "";

        /// <summary>
        /// 头像
        /// </summary>
        [Required]
        [StringLength(300)]
        public string IconUrl { get; set; } = "";

        /// <summary>
        /// 个性签名
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Signature { get; set; } = "";

        /// <summary>
        /// 用户状态
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
        /// 创建时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

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
        public BaseErrType ChangePassword(ResetPasswordVo password)
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

        /// <summary>
        /// 注册初始化（聚合行为）：默认密码（账号 MD5）、默认手机号（账号为手机号时回填）、绑定所属租户
        /// </summary>
        /// <param name="tenantId">所属租户id</param>
        public void ApplyForRegister(Guid tenantId)
        {
            // 默认密码
            if (Password.IsNullOrEmpty())
                Password = UserName.ToMd5();

            // 默认手机
            if (UserName.IsMobile() && Mobile.IsNullOrEmpty())
                Mobile = UserName;

            // 绑定租户
            SysTenantId = tenantId;
        }

        /// <summary>
        /// 更换手机号（聚合行为）
        /// </summary>
        /// <param name="mobile">新手机号</param>
        public void ChangeMobile(string mobile)
        {
            Mobile = mobile;
            UpdateTime = DateTime.UtcNow;
        }

        /// <summary>
        /// 变更所属租户（聚合行为）
        /// </summary>
        /// <param name="tenantId">目标租户id</param>
        public void AssignTenant(Guid tenantId)
        {
            SysTenantId = tenantId;
        }
    }
}
