using AutoMapper;
using Sys.Domain.AggregateRoots;
using Sys.Domain.Interfaces;
using Sys.Domain.Repositorys;
using OneForAll.Core;
using OneForAll.Core.DDD;
using OneForAll.Core.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Domain
{
    /// <summary>
    /// 领域服务：角色成员
    /// </summary>
    public class SysRoleMemberManager : BaseManager, ISysRoleMemberManager
    {
        private readonly ISysRoleRepository _roleRepository;
        private readonly ISysUserRepository _userRepository;
        private readonly ISysRoleUserContactRepository _roleUserRepository;
        public SysRoleMemberManager(
            ISysRoleRepository roleRepository,
            ISysUserRepository userRepository,
            ISysRoleUserContactRepository roleUserRepository)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _roleUserRepository = roleUserRepository;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <returns>权限列表</returns>
        public async Task<IEnumerable<SysUser>> GetListAsync(Guid roleId)
        {
            return await _roleUserRepository.GetListUserByRoleAsync(roleId);
        }

        /// <summary>
        /// 获取未加入实体的成员列表
        /// </summary>
        /// <param name="roleId">实体id</param>
        /// <param name="key">关键字</param>
        /// <returns>用户列表</returns>
        public async Task<IEnumerable<SysUser>> GetListUnJoinedAsync(Guid roleId, string key)
        {
            var users = await _userRepository.GetListAsync(key);
            var uids = await _roleUserRepository.GetListUserIdByRoleAsync(roleId);
            if (uids.Any())
            {
                return users.Where(w => !uids.Contains(w.Id)).ToList();
            }
            return users;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <param name="userIds">用户id</param>
        /// <returns>权限列表</returns>
        public async Task<BaseErrType> AddAsync(Guid roleId, IEnumerable<Guid> userIds)
        {
            var role = await _roleRepository.FindAsync(roleId);
            if (role == null)
                return BaseErrType.DataError;

            var uids = await _roleUserRepository.GetListUserIdByRoleAsync(roleId);
            var users = await _userRepository.GetListAsync(userIds);
            if (users.Any())
            {
                var data = new List<SysRoleUserContact>();
                users.ForEach(e =>
                {
                    if (!uids.Contains(e.Id))
                    {
                        data.Add(new SysRoleUserContact()
                        {
                            SysRoleId = roleId,
                            SysUserId = e.Id
                        });
                    }
                });

                if (data.Any())
                    return await ResultAsync(() => _roleUserRepository.AddRangeAsync(data));
            }
            return BaseErrType.DataEmpty;
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <param name="userIds">用户id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> RemoveAsync(Guid roleId, IEnumerable<Guid> userIds)
        {
            var role = await _roleRepository.FindAsync(roleId);
            if (role == null)
                return BaseErrType.DataError;

            var data = await _roleUserRepository.GetListByUserAsync(userIds);
            return await ResultAsync(() => _roleUserRepository.DeleteRangeAsync(data));
        }
    }
}
