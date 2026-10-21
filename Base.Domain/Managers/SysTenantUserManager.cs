using AutoMapper;
using Base.Domain.Entities;
using Base.Domain.Events;
using Base.Domain.Interfaces;
using Base.Domain.Models;
using Base.Domain.Repositorys;
using OneForAll.Core;
using OneForAll.Core.Events;
using OneForAll.Core.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Domain.Aggregates;
using OneForAll.Core.OAuth;

namespace Base.Domain.Managers
{
    /// <summary>
    /// 领域服务：子机构用户
    /// </summary>
    public class SysTenantUserManager : BaseDomainManager, ISysTenantUserManager
    {
        private readonly IMapper _mapper;
        private readonly ISysUserRepository _userRepository;
        private readonly IEventDispatcher _eventDispatcher;
        public SysTenantUserManager(
            IMapper mapper,
            ICurrentUser currentUser,
            ISysUserRepository userRepository,
            IEventDispatcher eventDispatcher) : base(currentUser)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _eventDispatcher = eventDispatcher;
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
        /// 获取列表（按id集合；ids 为空时返回全部）
        /// </summary>
        /// <param name="ids">用户id集合</param>
        /// <returns>用户列表</returns>
        public async Task<IEnumerable<SysUser>> GetListAsync(IEnumerable<Guid> ids)
        {
            if (ids != null && ids.Any())
                return await _userRepository.GetListAsync(ids);
            return await _userRepository.GetListAsync();
        }


        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns>用户列表</returns>
        public async Task<SysUser> GetAsync(Guid id)
        {
            return await _userRepository.GetByIdIQFAsync(id);
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
            var data = await _userRepository.GetByNameIQFAsync(form.UserName);
            if (data != null)
            {
                msg.Data = data.Id;// 返回id以便部分业务场景使用
                return msg.Fail(BaseErrType.DataExist, "账号已被使用");
            }
            #endregion

            data = _mapper.Map<SysUserForm, SysUser>(form);

            // 默认密码/默认手机/绑定租户下沉至聚合根行为
            data.ApplyForRegister(LoginUser.TenantId.TryGuid());

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
            var result = await ResultAsync(() => _userRepository.DeleteRangeAsync(data));
            if (result == BaseErrType.Success)
                await _eventDispatcher.PublishAsync(new UserDeletedEvent(ids));
            return result;
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
            var result = await ResultAsync(() => _userRepository.UpdateRangeAsync(data));
            if (result == BaseErrType.Success)
                await _eventDispatcher.PublishAsync(new UserPasswordResetEvent(data.Select(u => u.Id)));
            return result;
        }
    }
}
