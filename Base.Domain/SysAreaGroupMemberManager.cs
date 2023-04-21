using AutoMapper;
using Base.Domain.AggregateRoots;
using Base.Domain.Interfaces;
using Base.Domain.Repositorys;
using OneForAll.Core;
using OneForAll.Core.DDD;
using OneForAll.Core.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain
{
    /// <summary>
    /// 领域服务：地区组成员
    /// </summary>
    public class SysAreaGroupMemberManager : BaseManager, ISysAreaGroupMemberManager
    {
        private readonly ISysAreaGroupRepository _groupRepository;
        private readonly ISysUserRepository _userRepository;
        public SysAreaGroupMemberManager(
            ISysAreaGroupRepository groupRepository,
            ISysUserRepository userRepository)
        {
            _groupRepository = groupRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="groupId">地区组id</param>
        /// <returns>成员列表</returns>
        public async Task<IEnumerable<SysUser>> GetListAsync(Guid groupId)
        {
            var data = await _groupRepository.GetWithMembersAsync(groupId);
            if (data != null)
            {
                return data.SysAreaGroupUserContacts.Select(s=>s.SysUser);
            }
            return new List<SysUser>();
        }

        /// <summary>
        /// 获取未加入实体的成员列表
        /// </summary>
        /// <param name="groupId">实体id</param>
        /// <param name="key">关键字</param>
        /// <returns>用户列表</returns>
        public async Task<IEnumerable<SysUser>> GetListUnJoinedAsync(Guid groupId, string key)
        {
            var data = await _groupRepository.GetWithMembersAsync(groupId);
            if (data != null)
            {
                var users = await _userRepository.GetListAsync(key);
                var existsIds = data.SysAreaGroupUserContacts.Select(s => s.SysUserId);
                return users.Where(w => w.Id.NotIn(existsIds));
            }
            return new List<SysUser>();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="groupId">地区组id</param>
        /// <param name="userIds">用户id</param>
        /// <returns>权限列表</returns>
        public async Task<BaseErrType> AddAsync(Guid groupId, IEnumerable<Guid> userIds)
        {
            var data = await _groupRepository.GetWithMembersAsync(groupId);
            if (data == null) return BaseErrType.DataNotFound;

            var users = await _userRepository.GetListAsync(userIds);
            data.AddMember(users);
            return await ResultAsync(() => _groupRepository.UpdateAsync(data));
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="groupId">地区组id</param>
        /// <param name="userIds">用户id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> RemoveAsync(Guid groupId, IEnumerable<Guid> userIds)
        {
            var data = await _groupRepository.GetWithMembersAsync(groupId);
            if (data == null) return BaseErrType.DataNotFound;

            data.RemoveMember(userIds);
            return await ResultAsync(() => _groupRepository.UpdateAsync(data));
        }
    }
}

