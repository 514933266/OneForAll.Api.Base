using Base.Application.Dtos;
using Base.Domain.Models;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Base.Application.Interfaces
{
    /// <summary>
    /// 子系统机构（租户）
    /// </summary>
    public interface ISysTenantService
    {
        //#region 子机构

        ///// <summary>
        ///// 获取机构
        ///// </summary>
        ///// <param name="id">机构id</param>
        ///// <returns>机构</returns>
        //Task<SysTenantDto> GetAsync(Guid id);

        ///// <summary>
        ///// 获取分页
        ///// </summary>
        ///// <param name="pageIndex">页码</param>
        ///// <param name="pageSize">页数</param>
        ///// <param name="key">关键字</param>
        ///// <param name="isEnabled">合作状态（false未合作，true合作中）</param>
        ///// <param name="startDate">注册开始时间</param>
        ///// <param name="endDate">注册结束时间</param>
        ///// <returns>机构列表</returns>
        //Task<PageList<SysTenantDto>> GetPageAsync(
        //    int pageIndex,
        //    int pageSize,
        //    string key,
        //    bool isEnabled,
        //    DateTime? startDate,
        //    DateTime? endDate);

        ///// <summary>
        ///// 添加
        ///// </summary>
        ///// <param name="tenantId">登录机构id</param>
        ///// <param name="form">实体</param>
        ///// <returns>结果</returns>
        //Task<BaseErrType> AddAsync(Guid tenantId, SysTenantForm form);

        ///// <summary>
        ///// 修改
        ///// </summary>
        ///// <param name="form">实体</param>
        ///// <returns>结果</returns>
        //Task<BaseErrType> UpdateAsync(SysTenantForm form);

        ///// <summary>
        ///// 删除
        ///// </summary>
        ///// <param name="ids">菜单id</param>
        ///// <returns>结果</returns>
        //Task<BaseErrType> DeleteAsync(IEnumerable<Guid> ids);
        //#endregion

        /// <summary>
        /// 查询机构菜单
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SysMenuTreeDto>> GetListMenuTreeAsync();
    }
}
