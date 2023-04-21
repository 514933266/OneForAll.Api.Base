using AutoMapper;
using Sys.Application.Dtos;
using Sys.Application.Interfaces;
using Sys.Domain.AggregateRoots;
using Sys.Domain.Interfaces;
using Sys.Domain.Models;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Application
{
    /// <summary>
    /// 应用服务：地区组
    /// </summary>
    public class SysAreaGroupService : ISysAreaGroupService
    {
        private readonly IMapper _mapper;
        private readonly ISysAreaGroupManager _groupManager;
        private readonly ISysAreaGroupPermissionManager _permManager;
        private readonly ISysAreaGroupMemberManager _memberManager;
        public SysAreaGroupService(
            IMapper mapper,
            ISysAreaGroupManager groupManager,
            ISysAreaGroupPermissionManager permManager,
            ISysAreaGroupMemberManager memberManager)
        {
            _mapper = mapper;
            _groupManager = groupManager;
            _permManager = permManager;
            _memberManager = memberManager;
        }

        #region 地区组

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <returns>分页列表</returns>
        public async Task<PageList<SysAreaGroupDto>> GetPageAsync(int pageIndex, int pageSize, string key)
        {
            var data = await _groupManager.GetPageAsync(pageIndex, pageSize, key);
            var items = _mapper.Map<IEnumerable<SysAreaGroup>, IEnumerable<SysAreaGroupDto>>(data.Items);
            return new PageList<SysAreaGroupDto>(data.Total, data.PageSize, data.PageIndex, items);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="tenantId">租户id</param>
        /// <param name="entity">地区组</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> AddAsync(Guid tenantId, SysAreaGroupForm entity)
        {
            return await _groupManager.AddAsync(tenantId,entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">地区组</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> UpdateAsync(SysAreaGroupForm entity)
        {
            return await _groupManager.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">地区组id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> DeleteAsync(IEnumerable<Guid> ids)
        {
            return await _groupManager.DeleteAsync(ids);
        }

        #endregion

        #region 权限

        /// <summary>
        /// 获取地区
        /// </summary>
        /// <param name="id">地区组id</param>
        /// <returns>地区列表</returns>
        public async Task<IEnumerable<SysAreaDto>> GetListAreaAsync(Guid id)
        {
            var data = await _permManager.GetListAsync(id);
            return _mapper.Map<IEnumerable<SysArea>, IEnumerable<SysAreaDto>>(data);
        }

        /// <summary>
        /// 绑定地区
        /// </summary>
        /// <param name="id">地区组id</param>
        /// <param name="areaCodes">地区代码</param>
        /// <returns>地区列表</returns>
        public async Task<BaseErrType> AddAreaAsync(Guid id, IEnumerable<string> areaCodes)
        {
            return await _permManager.AddAsync(id, areaCodes);
        }

        #endregion

        #region 成员

        /// <summary>
        /// 获取成员
        /// </summary>
        /// <param name="id">地区组id</param>
        /// <returns>成员列表</returns>
        public async Task<IEnumerable<SysAreaGroupMemberDto>> GetListMemberAsync(Guid id)
        {
            var data = await _memberManager.GetListAsync(id);
            return _mapper.Map<IEnumerable<SysUser>, IEnumerable<SysAreaGroupMemberDto>>(data);
        }

        /// <summary>
        /// 获取没有加入组的人员
        /// </summary>
        /// <param name="id">地区组id</param>
        /// <param name="key">用户查询关键字</param>
        /// <returns>人员列表</returns>
        public async Task<IEnumerable<SysAreaGroupSelectionMemberDto>> GetListUnJoinedUserAsync(Guid id, string key)
        {
            var data = await _memberManager.GetListUnJoinedAsync(id, key);
            return _mapper.Map<IEnumerable<SysUser>, IEnumerable<SysAreaGroupSelectionMemberDto>>(data);
        }

        /// <summary>
        /// 添加成员
        /// </summary>
        /// <param name="id">地区组id</param>
        /// <param name="userIds">用户id集合</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> AddMemberAsync(Guid id, IEnumerable<Guid> userIds)
        {
            return await _memberManager.AddAsync(id, userIds);
        }

        /// <summary>
        /// 移除成员
        /// </summary>
        /// <param name="id">地区组id</param>
        /// <param name="userIds">用户id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> RemoveMemberAsync(Guid id, IEnumerable<Guid> userIds)
        {
            return await _memberManager.RemoveAsync(id, userIds);
        }
        #endregion
    }
}
