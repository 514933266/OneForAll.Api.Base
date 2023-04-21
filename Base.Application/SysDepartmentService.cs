using AutoMapper;
using Sys.Application.Dtos;
using Sys.Application.Interfaces;
using Sys.Domain.AggregateRoots;
using Sys.Domain.Interfaces;
using Sys.Domain.Models;
using OneForAll.Core;
using OneForAll.Core.Extension;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Application
{
    /// <summary>
    /// 应用服务：部门组织
    /// </summary>
    public class SysDepartmentService : ISysDepartmentService
    {
        private readonly IMapper _mapper;
        private readonly ISysDepartmentManager _departManager;
        private readonly ISysJobManager _jobManager;
        public SysDepartmentService(
            IMapper mapper,
            ISysDepartmentManager departManager,
            ISysJobManager jobManager)
        {
            _mapper = mapper;
            _departManager = departManager;
            _jobManager = jobManager;
        }

        #region 部门

        /// <summary>
        /// 获取机构组织列表
        /// </summary>
        /// <returns>部门列表</returns>
        public async Task<IEnumerable<SysDepartmentDto>> GetListAsync()
        {
            var data = await _departManager.GetListAsync();
            var dtos = _mapper.Map<IEnumerable<SysDepartment>, IEnumerable<SysDepartmentDto>>(data);
            return dtos.ToTree<SysDepartmentDto, Guid>();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="tenantId">租户id</param>
        /// <param name="entity">部门表单</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> AddAsync(Guid tenantId, SysDepartmentForm entity)
        {
            return await _departManager.AddAsync(tenantId, entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">部门表单</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> UpdateAsync(SysDepartmentForm entity)
        {
            return await _departManager.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">部门id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> DeleteAsync(Guid id)
        {
            return await _departManager.DeleteAsync(id);
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="id">部门id</param>
        /// <param name="entity">排序表单</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> SortAsync(Guid id, SysDepartmentForm entity)
        {
            return await _departManager.SortAsync(id, entity);
        }
        #endregion

        #region 岗位

        /// <summary>
        /// 获取部门岗位列表
        /// </summary>
        /// <param name="id">部门Id</param>
        /// <returns>岗位列表</returns>
        public async Task<IEnumerable<SysJobDto>> GetListJobAsync(Guid id)
        {
            var data = await _jobManager.GetListAsync(id);
            return _mapper.Map<IEnumerable<SysJob>, IEnumerable<SysJobDto>>(data);
        }

        /// <summary>
        /// 添加岗位
        /// </summary>
        /// <param name="id">部门组织Id</param>
        /// <param name="entity">岗位表单</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> AddJobAsync(Guid id, SysJobForm entity)
        {
            return await _jobManager.AddAsync(id, entity);
        }

        /// <summary>
        /// 修改岗位
        /// </summary>
        /// <param name="id">部门组织Id</param>
        /// <param name="entity">岗位表单</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> UpdateJobAsync(Guid id, SysJobForm entity)
        {
            return await _jobManager.UpdateAsync(id, entity);
        }

        /// <summary>
        /// 删除岗位
        /// </summary>
        /// <param name="id">部门组织Id</param>
        /// <param name="jobIds">岗位id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> DeleteJobAsync(Guid id, IEnumerable<Guid> jobIds)
        {
            return await _jobManager.DeleteAsync(id, jobIds);
        }
        #endregion

        #region 角色

        /// <summary>
        /// 获取部门角色列表
        /// </summary>
        /// <param name="id">部门id</param>
        /// <returns>用户列表</returns>
        public async Task<IEnumerable<SysDepartmentRoleDto>> GetListRoleAsync(Guid id)
        {
            var data = await _departManager.GetListRoleAsync(id);
            return _mapper.Map<IEnumerable<SysJobRoleContact>, IEnumerable<SysDepartmentRoleDto>>(data);
        }

        /// <summary>
        /// 获取没有加入岗位的角色列表
        /// </summary>
        /// <param name="id">部门id</param>
        /// <param name="jobId">岗位id</param>
        /// <param name="key">关键字</param>
        /// <returns>角色列表</returns>
        public async Task<IEnumerable<SysJobRoleSelectionDto>> GetListUnJoinedRoleAsync(Guid id, Guid jobId, string key)
        {
            var data = await _jobManager.GetListUnJoinedRoleAsync(jobId, key);
            return _mapper.Map<IEnumerable<SysRole>, IEnumerable<SysJobRoleSelectionDto>>(data);
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="id">部门id</param>
        /// <param name="jobId">岗位id</param>
        /// <param name="ids">角色id集合</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> AddJobRoleAsync(Guid id, Guid jobId, IEnumerable<Guid> ids)
        {
            return await _jobManager.AddRoleAsync(jobId, ids);
        }

        /// <summary>
        /// 移除角色
        /// </summary>
        /// <param name="id">部门id</param>
        /// <param name="jobId">岗位id</param>
        /// <param name="roleId">角色Id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> RemoveJobRoleAsync(Guid id, Guid jobId, Guid roleId)
        {
            return await _jobManager.RemoveRoleAsync(jobId, roleId);
        }
        #endregion

        #region 成员

        /// <summary>
        /// 获取部门成员列表
        /// </summary>
        /// <param name="id">部门Id</param>
        /// <returns>用户列表</returns>
        public async Task<IEnumerable<SysDepartmentUserDto>> GetListUserAsync(Guid id)
        {
            var data = await _departManager.GetListUserAsync(id);
            return _mapper.Map<IEnumerable<SysJobUserContact>, IEnumerable<SysDepartmentUserDto>>(data);
        }

        /// <summary>
        /// 获取没有加入岗位的用户列表
        /// </summary>
        /// <param name="id">部门id</param>
        /// <param name="jobId">岗位id</param>
        /// <param name="key">关键字</param>
        /// <returns>用户列表</returns>
        public async Task<IEnumerable<SysJobUserSelectionDto>> GetListUnJoinedUserAsync(Guid id, Guid jobId, string key)
        {
            var data = await _jobManager.GetListUnJoinedUserAsync(jobId, key);
            return _mapper.Map<IEnumerable<SysUser>, IEnumerable<SysJobUserSelectionDto>>(data);
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="id">部门id</param>
        /// <param name="jobId">岗位id</param>
        /// <param name="ids">用户id集合</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> AddJobUserAsync(Guid id, Guid jobId, IEnumerable<Guid> ids)
        {
            return await _jobManager.AddUserAsync(jobId, ids);
        }

        /// <summary>
        /// 移除用户
        /// </summary>
        /// <param name="id">部门id</param>
        /// <param name="jobId">岗位id</param>
        /// <param name="userId">用户Id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> RemoveJobUserAsync(Guid id, Guid jobId, Guid userId)
        {
            return await _jobManager.RemoveUserAsync(jobId, userId);
        }
        #endregion
    }
}
