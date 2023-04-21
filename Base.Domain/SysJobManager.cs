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

namespace Base.Domain
{
    /// <summary>
    /// 领域服务：岗位
    /// </summary>
    public class SysJobManager :BaseManager, ISysJobManager
    {
        private readonly IMapper _mapper;
        private readonly ISysJobRepository _jobRepository;
        private readonly ISysDepartmentRepository _departRepository;
        private readonly ISysUserRepository _userRepository;
        private readonly ISysRoleRepository _roleRepository;
        private readonly ISysJobRoleContactRepository _jobRoleRepository;
        private readonly ISysJobUserContactRepository _jobUserRepository;
        public SysJobManager(
            IMapper mapper,
            ISysJobRepository jobRepository,
            ISysDepartmentRepository departRepository,
            ISysUserRepository userRepository,
            ISysRoleRepository roleRepository,
            ISysJobRoleContactRepository jobRoleRepository,
            ISysJobUserContactRepository jobUserRepository)
        {
            _mapper = mapper;
            _jobRepository = jobRepository;
            _departRepository = departRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _jobRoleRepository = jobRoleRepository;
            _jobUserRepository = jobUserRepository;
        }

        /// <summary>
        /// 获取部门岗位列表
        /// </summary>
        /// <param name="departmentId">部门id</param>
        /// <returns>角色列表</returns>
        public async Task<IEnumerable<SysJob>> GetListAsync(Guid departmentId)
        {
            var data = await _departRepository.GetListAsync();
            if (data.Any())
            {
                var children = data.FindChildren(departmentId);
                var ids = children.Select(s => s.Id);
                ids = ids.Append(departmentId);
                var jobs = await _jobRepository.GetListWithContactsAsync(ids);
                if (jobs.Any())
                {
                    var uids = new List<Guid>();
                    var rids = new List<Guid>();
                    var users = new List<SysUser>();
                    var roles = new List<SysRole>();
                    jobs.ForEach(e =>
                    {
                        uids.AddRange(e.SysJobUserContacts.Select(s => s.SysUserId));
                        rids.AddRange(e.SysJobRoleContacts.Select(s => s.SysRoleId));
                    });
                    uids = uids.Distinct().ToList();
                    rids = rids.Distinct().ToList();
                    if (uids.Count > 0)
                    {
                        var uData = await _userRepository.GetListAsync(w => uids.Contains(w.Id));
                        users = uData.ToList();
                    }
                    if (rids.Count > 0)
                    {
                        var rData = await _roleRepository.GetListAsync(w => rids.Contains(w.Id));
                        roles = rData.ToList();
                    }
                    // 绑定
                    jobs.ForEach(e =>
                    {
                        e.SysJobUserContacts.ForEach(user =>
                        {
                            user.SysUser = users.FirstOrDefault(w => w.Id.Equals(user.SysUserId));
                        });
                        e.SysJobRoleContacts.ForEach(role =>
                        {
                            role.SysRole = roles.FirstOrDefault(w => w.Id.Equals(role.SysRoleId));
                        });
                    });
                }
                return jobs;
            }
            return new List<SysJob>();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="departmentId">部门组织Id</param>
        /// <param name="entity">岗位表单</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> AddAsync(Guid departmentId, SysJobForm entity)
        {
            var department = await _departRepository.GetWithJobsAsync(departmentId);
            if (department == null) return BaseErrType.DataNotFound;

            var data = _mapper.Map<SysJobForm, SysJob>(entity);
            department.AddJob(data);
            return await ResultAsync(() => _departRepository.UpdateAsync(department));
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="departmentId">部门组织Id</param>
        /// <param name="entity">岗位表单</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> UpdateAsync(Guid departmentId, SysJobForm entity)
        {
            var department = await _departRepository.GetWithJobsAsync(departmentId);
            if (department == null) return BaseErrType.DataNotFound;
            if (department.SysJobs.Any(w => w.Name.Equals(entity.Name) && w.Id != entity.Id)) return BaseErrType.DataExist;

            var data = department.SysJobs.FirstOrDefault(w => w.Id == entity.Id);
            if (data == null) return BaseErrType.DataNotFound;

            data.MapFrom(entity);
            return await ResultAsync(() => _jobRepository.UpdateAsync(data));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="departmentId">部门组织Id</param>
        /// <param name="ids">岗位id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> DeleteAsync(Guid departmentId, IEnumerable<Guid> ids)
        {
            var department = await _departRepository.GetWithJobsAsync(departmentId);
            if (department == null) return BaseErrType.DataNotFound;

            department.RemoveJob(ids);
            return await ResultAsync(() => _departRepository.UpdateAsync(department));
        }

        #region 角色

        /// <summary>
        /// 获取未加入岗位的角色列表
        /// </summary>
        /// <param name="id">岗位id</param>
        /// <param name="key">角色关键字</param>
        /// <returns>角色列表</returns>
        public async Task<IEnumerable<SysRole>> GetListUnJoinedRoleAsync(Guid id, string key)
        {
            var result = new List<SysRole>();
            var data = await _jobRepository.GetWithRoleContactsAsync(id);
            if (data == null) return result;

            var roles = await _roleRepository.GetListAsync(w => w.Name.Contains(key)) as ICollection<SysRole>;
            data.SysJobRoleContacts.ForEach(e =>
            {
                var role = roles.FirstOrDefault(w => w.Id.Equals(e.SysRoleId));
                if (role != null)
                {
                    roles.Remove(role);
                }
            });
            return roles;
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="id">岗位id</param>
        /// <param name="ids">角色id集合</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> AddRoleAsync(Guid id, IEnumerable<Guid> ids)
        {
            var data = await _jobRepository.GetWithRoleContactsAsync(id);
            if (data == null) return BaseErrType.DataNotFound;

            var roles = await _roleRepository.GetListAsync(w => ids.Contains(w.Id)) as ICollection<SysRole>;
            if (roles.Count < 1) return BaseErrType.DataEmpty;

            var result = new List<SysJobRoleContact>();
            roles.ForEach(e =>
            {
                var role = data.SysJobRoleContacts.FirstOrDefault(w => w.SysRoleId.Equals(e.Id));
                if (role == null)
                {
                    result.Add(new SysJobRoleContact()
                    {
                        Id = Guid.NewGuid(),
                        SysRoleId = e.Id,
                        SysJobId = id
                    });
                }
            });
            if (result.Count > 0)
            {
                return await ResultAsync(() => _jobRoleRepository.AddRangeAsync(result));
            }
            return BaseErrType.DataEmpty;
        }

        /// <summary>
        /// 移除角色
        /// </summary>
        /// <param name="id">岗位id</param>
        /// <param name="roleId">角色Id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> RemoveRoleAsync(Guid id, Guid roleId)
        {
            var data = await _jobRepository.GetWithRoleContactsAsync(id);
            if (data == null) return BaseErrType.DataNotFound;

            var role = data.SysJobRoleContacts.FirstOrDefault(w => w.Id.Equals(roleId));
            if (role != null)
            {
                return await ResultAsync(() => _jobRoleRepository.DeleteAsync(role));
            }
            return BaseErrType.DataEmpty;
        }

        #endregion

        #region 成员

        /// <summary>
        /// 获取未加入岗位的用户列表
        /// </summary>
        /// <param name="id">岗位id</param>
        /// <param name="key">用户关键字</param>
        /// <returns>角色列表</returns>
        public async Task<IEnumerable<SysUser>> GetListUnJoinedUserAsync(Guid id, string key)
        {
            var result = new List<SysUser>();
            var data = await _jobRepository.GetWithUserContactsAsync(id);
            if (data == null) return result;

            var users = await _userRepository.GetListAsync(w =>
                w.Name.Contains(key) ||
                w.UserName.Contains(key)) as ICollection<SysUser>;

            data.SysJobUserContacts.ForEach(e =>
            {
                var role = users.FirstOrDefault(w => w.Id.Equals(e.SysUserId));
                if (role != null)
                {
                    users.Remove(role);
                }
            });
            return users;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="id">岗位id</param>
        /// <param name="ids">用户id集合</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> AddUserAsync(Guid id, IEnumerable<Guid> ids)
        {
            var data = await _jobRepository.GetWithUserContactsAsync(id);
            if (data == null) return BaseErrType.DataNotFound;

            var users = await _userRepository.GetListAsync(w => ids.Contains(w.Id)) as ICollection<SysUser>;
            if (users.Count < 1) return BaseErrType.DataEmpty;

            var result = new List<SysJobUserContact>();
            users.ForEach(e =>
            {
                var user = data.SysJobUserContacts.FirstOrDefault(w => w.SysUserId.Equals(e.Id));
                if (user == null)
                {
                    result.Add(new SysJobUserContact()
                    {
                        Id = Guid.NewGuid(),
                        SysUserId = e.Id,
                        SysJobId = id
                    });
                }
            });
            if (result.Count > 0)
            {
                return await ResultAsync(() => _jobUserRepository.AddRangeAsync(result));
            }
            return BaseErrType.DataEmpty;
        }

        /// <summary>
        /// 移除用户
        /// </summary>
        /// <param name="id">岗位id</param>
        /// <param name="userId">用户Id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> RemoveUserAsync(Guid id, Guid userId)
        {
            var data = await _jobRepository.GetWithUserContactsAsync(id);
            if (data == null) return BaseErrType.DataNotFound;

            var user = data.SysJobUserContacts.FirstOrDefault(w => w.Id.Equals(userId));
            if (user != null)
            {
                return await ResultAsync(() => _jobUserRepository.DeleteAsync(user));
            }
            return BaseErrType.DataEmpty;
        }

        #endregion
    }
}
