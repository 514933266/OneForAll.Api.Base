using AutoMapper;
using Base.Domain.AggregateRoots;
using Base.Domain.Interfaces;
using Base.Domain.Models;
using Base.Domain.Repositorys;
using OneForAll.Core;
using OneForAll.Core.DDD;
using OneForAll.Core.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Domain.Aggregates;
using OneForAll.Core.OAuth;
using Microsoft.AspNetCore.Http;
using OneForAll.Core.Security;

namespace Base.Domain
{
    /// <summary>
    /// 领域服务：子机构用户
    /// </summary>
    public class SysTenantUserManager : SysBaseManager, ISysTenantUserManager
    {
        private readonly ISysUserRepository _userRepository;
        private readonly ISysPermissionRepository _permRepository;
        private readonly ISysRoleUserContactRepository _roleUserRepository;
        private readonly ISysUserPermContactRepository _userPermRepository;
        public SysTenantUserManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ISysUserRepository userRepository,
            ISysPermissionRepository permRepository,
            ISysRoleUserContactRepository roleUserRepository,
            ISysUserPermContactRepository userPermRepository) : base(mapper, httpContextAccessor)
        {
            _userRepository = userRepository;
            _permRepository = permRepository;
            _roleUserRepository = roleUserRepository;
            _userPermRepository = userPermRepository;
        }

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <returns>用户分页</returns>
        public async Task<PageList<SysLoginUserAggr>> GetPageAsync(int pageIndex, int pageSize, string key)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;
            var result = new List<SysLoginUserAggr>();
            var data = await _userRepository.GetPageAsync(pageIndex, pageSize, key);
            var userIds = data.Items.Select(s => s.Id).ToList();
            userIds.ForEach(id =>
            {
                // 后续优化
                var user = GetLoginAsync(id).Result;
                result.Add(user);
            });
            return new PageList<SysLoginUserAggr>(data.Total, data.PageIndex, data.PageSize, result);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>用户列表</returns>
        public async Task<IEnumerable<SysUser>> GetListAsync(string key)
        {
            return await _userRepository.GetListAsync(key);
        }


        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns>用户列表</returns>
        public async Task<SysUser> GetAsync(Guid id)
        {
            return await _userRepository.GetAsync(id);
        }

        // 获取登录个人信息(含菜单、权限)
        private async Task<SysLoginUserAggr> GetLoginAsync(Guid id)
        {
            var user = await _userRepository.GetAsync(id);
            var pids = await _roleUserRepository.GetListPermIdByUserAsync(id);
            var pids2 = await _userPermRepository.GetListPermIdByUserAsync(id);
            pids = pids.Concat(pids2);

            var permissions = await _permRepository.GetListAsync(pids);
            var loginUser = _mapper.Map<SysTenantUserAggr, SysLoginUserAggr>(user);
            loginUser.SetPermissons(permissions);
            loginUser.SysLoginUserMenus = loginUser.SysLoginUserMenus.OrderBy(w => w.Code).ToList();
            loginUser.SysLoginUserMenus.ForEach(e => { e.SysLoginUserPermissions = e.SysLoginUserPermissions.OrderBy(o => o.SortCode).ToList(); });
            return loginUser;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        public async Task<BaseMessage> AddAsync(SysUserForm form)
        {
            var msg = new BaseMessage();
            #region 校验

            if (form.Password != form.RePassword)
                return msg.Fail(BaseErrType.DataNotMatch, "两次密码输入不一致");
            var data = await _userRepository.GetIQFAsync(form.UserName);
            if (data != null)
            {
                msg.Data = data.Id;// 返回id以便部分业务场景使用
                return msg.Fail(BaseErrType.DataExist, "账号已被使用");
            }
            #endregion

            data = _mapper.Map<SysUserForm, SysUser>(form);

            // 默认密码
            if (data.Password.IsNullOrEmpty())
                data.Password = data.UserName.ToMd5();

            // 默认手机
            if (data.UserName.IsMobile() && data.Mobile.IsNullOrEmpty())
                data.Mobile = data.UserName;

            data.SysTenantId = LoginUser.SysTenantId;

            // 检测手机号是否被使用
            if (!form.Mobile.IsNullOrEmpty())
            {
                var exists = await _userRepository.GetByMobileIQFAsync(data.Mobile);
                if (exists != null)
                {
                    msg.Data = exists.Id;
                    return msg.Fail(BaseErrType.DataExist, "手机号码已被使用");
                }
            }

            var effected = await _userRepository.AddAsync(data);
            msg.Data = data.Id;

            return effected > 0 ? msg.Success("添加账号成功") : msg.Fail("添加账号失败");
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        public async Task<BaseMessage> UpdateAsync(SysUserUpdateForm form)
        {
            var msg = new BaseMessage();
            var data = await _userRepository.FindAsync(form.Id);
            if (data == null)
                return msg.Fail(BaseErrType.DataError, "账号不存在");

            // 检测手机号是否被使用
            if (!form.Mobile.IsNullOrEmpty())
            {
                var exists = await _userRepository.GetAsync(w => w.Mobile == form.Mobile);
                if (exists != null && exists.Id != data.Id)
                    return msg.Fail(BaseErrType.DataExist, "手机号码已被使用");
            }

            _mapper.Map(form, data);
            var effected = await _userRepository.SaveChangesAsync();
            return effected > 0 ? msg.Success("修改账号成功") : msg.Fail("修改账号失败");
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">用户id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> DeleteAsync(IEnumerable<Guid> ids)
        {
            var data = await _userRepository.GetListAsync(ids);
            return await ResultAsync(() => _userRepository.DeleteRangeAsync(data));
        }


        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="ids">用户id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> ResetPasswordAsync(IEnumerable<Guid> ids)
        {
            var data = await _userRepository.GetListAsync(ids);
            data.ForEach(e => { e.ResetPassword(); });
            return await ResultAsync(() => _userRepository.UpdateRangeAsync(data));
        }
    }
}
