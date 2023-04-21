using System;
using System.Collections.Generic;
using System.Text;

namespace Sys.Public.Models
{
    /// <summary>
    /// 授权用户身份信息
    /// </summary>
    public static class UserClaimType
    {
        /// <summary>
        /// 租户id
        /// </summary>
        public const string TENANT_ID = "TenantId";

        /// <summary>
        /// 用户名
        /// </summary>
        public const string USERNAME = "UserName";

        /// <summary>
        /// 名称
        /// </summary>
        public const string USER_NICKNAME = "UserNickName";

        /// <summary>
        /// id
        /// </summary>
        public const string USER_ID = "UserId";

        /// <summary>
        /// 角色
        /// </summary>
        public const string ROLE = "Role";

        /// <summary>
        /// 人员档案id
        /// </summary>
        public const string PERSON_ID = "PersonId";
    }
}
