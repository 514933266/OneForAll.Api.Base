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
    /// 领域服务：部门组织
    /// </summary>
    public class SysDepartmentManager : BaseManager, ISysDepartmentManager
    {
        private readonly IMapper _mapper;
        private readonly ISysJobRepository _jobRepository;
        private readonly ISysUserRepository _userRepository;
        private readonly ISysRoleRepository _roleRepository;
        private readonly ISysDepartmentRepository _departRepository;
        public SysDepartmentManager(
            IMapper mapper,
            ISysJobRepository jobRepository,
            ISysUserRepository userRepository,
            ISysRoleRepository roleRepository,
            ISysDepartmentRepository departRepository)
        {
            _mapper = mapper;
            _jobRepository = jobRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _departRepository = departRepository;
        }

        /// <summary>
        /// 获取机构组织列表
        /// </summary>
        /// <returns>部门列表</returns>
        public async Task<IEnumerable<SysDepartment>> GetListAsync()
        {
            return await _departRepository.GetListAsync();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="tenantId">租户id</param>
        /// <param name="entity">部门表单</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> AddAsync(Guid tenantId, SysDepartmentForm entity)
        {
            var exists = await CheckParentExists(entity);
            if (!exists) return BaseErrType.DataNotFound;

            var data = _mapper.Map<SysDepartmentForm, SysDepartment>(entity);
            data.Id = Guid.NewGuid();
            data.SysTenantId = tenantId;
            data.CreateTime = DateTime.Now;
            return await ResultAsync(() => _departRepository.AddAsync(data));
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">部门表单</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> UpdateAsync(SysDepartmentForm entity)
        {
            var exists = await CheckParentExists(entity);
            if (!exists) return BaseErrType.DataNotFound;
            var data = await _departRepository.FindAsync(entity.Id);
            if (data == null) return BaseErrType.DataNotFound;

            data.MapFrom(entity);
            return await ResultAsync(() => _departRepository.UpdateAsync(data));
        }

        private async Task<bool> CheckParentExists(SysDepartmentForm entity)
        {
            if (entity.ParentId != Guid.Empty)
            {
                var menus = await _departRepository.GetListAsync();
                var parent = menus.FirstOrDefault(w => w.Id == entity.ParentId);
                var children = menus.FindChildren(entity.Id);

                // 1. 禁止选择不存在的上级
                // 2. 禁止选择下级作为自己的上级
                if (parent == null) return false;
                if (entity.Id != Guid.Empty &&
                    children.Any(w => w.Id.Equals(entity.ParentId))) return false;
            }
            return true;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">部门id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> DeleteAsync(Guid id)
        {
            var data = await _departRepository.FindAsync(id);
            if (data == null) return BaseErrType.DataNotFound;

            var count = await _departRepository.CountChildrenAsync(data.Id);
            if (count > 0) return BaseErrType.DataExist;

            return await ResultAsync(() => _departRepository.DeleteAsync(data));
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="id">部门id</param>
        /// <param name="entity">排序表单</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> SortAsync(Guid id, SysDepartmentForm entity)
        {
            var data = await _departRepository.FindAsync(id);
            if (data == null) return BaseErrType.DataNotFound;

            data.SortNumber = entity.SortNumber;
            return await ResultAsync(() => _departRepository.UpdateAsync(data));
        }

        #region 角色

        /// <summary>
        /// 获取部门角色列表
        /// </summary>
        /// <param name="id">部门Id</param>
        /// <returns>用户列表</returns>
        public async Task<IEnumerable<SysJobRoleContact>> GetListRoleAsync(Guid id)
        {
            var result = new List<SysJobRoleContact>();
            var data = await _departRepository.GetListAsync();
            if (data.Any())
            {
                var children = data.FindChildren(id);
                var ids = children.Select(s => s.Id);
                ids = ids.Append(id);
                var jobs = await _jobRepository.GetListWithRolesAsync(ids);
                if (jobs.Any())
                {
                    var rids = new List<Guid>();
                    var roles = new List<SysRole>();
                    jobs.ForEach(e =>
                    {
                        rids.AddRange(e.SysJobRoleContacts.Select(s => s.SysRoleId));
                    });
                    rids = rids.Distinct().ToList();
                    if (rids.Count > 0)
                    {
                        var rData = await _roleRepository.GetListAsync(w => rids.Contains(w.Id));
                        roles = rData.ToList();
                    }
                    jobs.ForEach(e =>
                    {
                        e.SysJobRoleContacts.ForEach(e2 =>
                        {
                            if (!result.Any(w => w.SysRoleId.Equals(e2.SysRoleId)))
                            {
                                e2.SysRole = roles.FirstOrDefault(w => w.Id.Equals(e2.SysRoleId));
                                result.Add(e2);
                            }
                        });
                    });
                }
            }
            return result;
        }

        #endregion

        #region 用户

        /// <summary>
        /// 获取部门成员列表
        /// </summary>
        /// <param name="id">部门Id</param>
        /// <returns>用户列表</returns>
        public async Task<IEnumerable<SysJobUserContact>> GetListUserAsync(Guid id)
        {
            var result = new List<SysJobUserContact>();
            var data = await _departRepository.GetListAsync();
            if (data.Any())
            {
                var children = data.FindChildren(id);
                var ids = children.Select(s => s.Id);
                ids = ids.Append(id);
                var jobs = await _jobRepository.GetListWithUsersAsync(ids);
                if (jobs.Any())
                {
                    var uids = new List<Guid>();
                    var users = new List<SysUser>();
                    jobs.ForEach(e =>
                    {
                        uids.AddRange(e.SysJobUserContacts.Select(s => s.SysUserId));
                    });
                    uids = uids.Distinct().ToList();
                    if (uids.Count > 0)
                    {
                        var uData = await _userRepository.GetListAsync(w => uids.Contains(w.Id));
                        users = uData.ToList();
                    }
                    jobs.ForEach(e =>
                    {
                        e.SysJobUserContacts.ForEach(e2 =>
                        {
                            if (!result.Any(w => w.SysUserId.Equals(e2.SysUserId)))
                            {
                                e2.SysUser = users.FirstOrDefault(w => w.Id.Equals(e2.SysUserId));
                                result.Add(e2);
                            }
                        });
                    });
                }
            }
            return result;
        }

        #endregion
    }
}
