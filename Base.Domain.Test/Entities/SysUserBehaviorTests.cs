using Base.Domain.Entities;
using Base.Domain.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneForAll.Core;
using System;

namespace Base.Domain.Test.Entities
{
    /// <summary>
    /// SysUser 聚合行为单元测试（Phase 2 验证项）：
    /// 锁定 ChangePassword 三分支、ChangeMobile、AssignTenant 的领域规则不回归。
    /// </summary>
    [TestClass]
    public class SysUserBehaviorTests
    {
        // 构造一个可控 SysUser（仅填行为方法涉及的字段）
        private static SysUser NewUser(string password = "oldpwd")
        {
            return new SysUser
            {
                Id = Guid.NewGuid(),
                UserName = "zhangsan",
                Password = password,
                Mobile = "13800000000",
                SysTenantId = Guid.Empty
            };
        }

        // ChangePassword 分支1：两次新密码不一致 → DataNotMatch，且不应改动密码
        [TestMethod]
        public void ChangePassword_RepeatNotEqualNew_ReturnsDataNotMatch()
        {
            var user = NewUser("oldpwd");
            var vo = new ResetPasswordVo { Old = "oldpwd", New = "new1", Repeat = "new2" };

            var result = user.ChangePassword(vo);

            Assert.AreEqual(BaseErrType.DataNotMatch, result);
            Assert.AreEqual("oldpwd", user.Password);
        }

        // ChangePassword 分支2：旧密码匹配且两次新密码一致 → Success，且密码更新为新值
        [TestMethod]
        public void ChangePassword_OldMatches_ReturnsSuccessAndUpdatePassword()
        {
            var user = NewUser("oldpwd");
            var vo = new ResetPasswordVo { Old = "oldpwd", New = "newpwd", Repeat = "newpwd" };

            var result = user.ChangePassword(vo);

            Assert.AreEqual(BaseErrType.Success, result);
            Assert.AreEqual("newpwd", user.Password);
        }

        // ChangePassword 分支3：旧密码不匹配 → PasswordInvalid，且不应改动密码
        [TestMethod]
        public void ChangePassword_OldNotMatch_ReturnsPasswordInvalid()
        {
            var user = NewUser("oldpwd");
            var vo = new ResetPasswordVo { Old = "wrong", New = "newpwd", Repeat = "newpwd" };

            var result = user.ChangePassword(vo);

            Assert.AreEqual(BaseErrType.PasswordInvalid, result);
            Assert.AreEqual("oldpwd", user.Password);
        }

        // ChangeMobile：更新手机号并刷新 UpdateTime
        [TestMethod]
        public void ChangeMobile_UpdatesMobileAndTouchTime()
        {
            var user = NewUser();
            user.UpdateTime = null;

            user.ChangeMobile("13900001111");

            Assert.AreEqual("13900001111", user.Mobile);
            Assert.IsNotNull(user.UpdateTime);
        }

        // AssignTenant：绑定目标租户
        [TestMethod]
        public void AssignTenant_SetsSysTenantId()
        {
            var user = NewUser();
            var tenantId = Guid.NewGuid();

            user.AssignTenant(tenantId);

            Assert.AreEqual(tenantId, user.SysTenantId);
        }
    }
}
