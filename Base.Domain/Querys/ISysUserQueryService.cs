using Base.Domain.Aggregates;
using Base.Domain.Entities;
using Base.Domain.ValueObjects;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Base.Domain.Querys
{
    /// <summary>
    /// 读路径服务：用户（登录态组装、分页批量组装、菜单树）
    /// 说明：读逻辑与领域解读（权限合并/菜单上级展开）归 Domain/Querys，从 Entity 组装 Aggr 返回，不含 DTO
    /// </summary>
    public interface ISysUserQueryService
    {
        /// <summary>
        /// 组装单个登录用户聚合（角色权限 ∪ 直授权限；tenantFilterId 非空时与租户权限求交）
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="tenantFilterId">租户权限过滤（个人中心场景传入，其余为 null）</param>
        /// <returns>登录用户聚合，用户不存在返回 null</returns>
        Task<SysLoginUserAggr> GetLoginUserAsync(Guid userId, Guid? tenantFilterId = null);

        /// <summary>
        /// 分页组装登录用户聚合（批量装配，消除逐用户查询的 N+1；对每条应用菜单/权限排序）
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <returns>分页的登录用户聚合</returns>
        Task<PageList<SysLoginUserAggr>> GetPagedLoginUsersAsync(int pageIndex, int pageSize, string key);

        /// <summary>
        /// 获取当前用户可见菜单（含上级菜单，按排序号升序去重）
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="tenantFilterId">租户权限过滤</param>
        /// <returns>菜单列表</returns>
        Task<IEnumerable<SysMenu>> GetMenusAsync(Guid userId, Guid tenantFilterId);

        /// <summary>
        /// 当前登录用户个人信息：组装最新聚合（含租户权限求交）并回填 LoginInfo 缓存
        /// （F5 总拿到最新权限；不读缓存，保持与旧 PersonalManager.GetAsync 一致）
        /// </summary>
        /// <returns>登录用户聚合</returns>
        Task<SysLoginUserAggr> GetPersonalAsync();

        /// <summary>
        /// 当前登录用户可见菜单（含上级展开）
        /// </summary>
        /// <returns>菜单列表</returns>
        Task<IEnumerable<SysMenu>> GetPersonalMenusAsync();

        /// <summary>
        /// 校验当前用户功能权限（LoginInfo 读透缓存 + 权限求并，不加载租户过滤）
        /// </summary>
        /// <param name="form">验证实体（Controller/Action）</param>
        /// <returns>结果</returns>
        Task<BaseErrType> ValidateAsync(SysPermissionCheck form);

        /// <summary>
        /// 退出登录：清除当前用户 LoginInfo 缓存
        /// </summary>
        /// <returns>结果</returns>
        Task<BaseErrType> LoginoutAsync();
    }
}
