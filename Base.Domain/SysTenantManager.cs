using AutoMapper;
using Base.Domain.AggregateRoots;
using Base.Domain.Interfaces;
using Base.Domain.Models;
using Base.Domain.Repositorys;
using OneForAll.Core;
using OneForAll.Core.DDD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneForAll.EFCore;
using OneForAll.Core.Utility;
using OneForAll.Core.Extension;
using OneForAll.Core.Security;
using Microsoft.AspNetCore.Http;

namespace Base.Domain
{
    /// <summary>
    /// 领域服务：机构（租户）
    /// </summary>
    public class SysTenantManager: SysBaseManager, ISysTenantManager
    {
        private readonly ISysTenantRepository _repository;
        public SysTenantManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ISysTenantRepository repository) : base(mapper, httpContextAccessor)
        {
            _repository = repository;
        }

        /// <summary>
        /// 获取机构
        /// </summary>
        /// <param name="id">机构id</param>
        /// <returns>机构</returns>
        public async Task<SysTenant> GetAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return await _repository.FindAsync(LoginUser.SysTenantId);
            }
            return await _repository.FindAsync(id);
        }

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <param name="isEnabled">合作状态（false未合作，true合作中）</param>
        /// <param name="startDate">注册开始时间</param>
        /// <param name="endDate">注册结束时间</param>
        /// <returns>机构列表</returns>
        public async Task<PageList<SysTenant>> GetPageAsync(
            int pageIndex,
            int pageSize,
            string key,
            bool isEnabled,
            DateTime? startDate,
            DateTime? endDate)
        {
            if (pageIndex < 1)  pageIndex = 1;
            if (pageSize < 1)   pageSize = 10;
            if (pageSize > 100) pageSize = 100;
            return await _repository.GetPageAsync(pageIndex, pageSize, key, isEnabled, startDate, endDate);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="tenantId">登录机构id</param>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> AddAsync(Guid tenantId, SysTenantForm form)
        {
            var data = await _repository.GetByNameAsync(form.Name);
            if (data != null)
                return BaseErrType.DataExist;

            data = _mapper.Map<SysTenantForm, SysTenant>(form);
            data.ParentId = tenantId;
            data.CreateTime = DateTime.Now;
            if (data.Code.IsNullOrEmpty())
            {
                data.Code = StringHelper.GetRandomString(18);
            }
            return await ResultAsync(() => _repository.AddAsync(data));
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> UpdateAsync(SysTenantForm form)
        {
            var data = await _repository.GetByNameAsync(form.Name);
            if (data != null && data.Id != form.Id)
                return BaseErrType.DataExist;
            data = await _repository.GetByCodeAsync(form.Code);
            if (data != null && data.Id != form.Id)
                return BaseErrType.DataExist;
            data = await _repository.FindAsync(form.Id);
            if (data == null)
                return BaseErrType.DataNotFound;

            _mapper.Map(form, data);
            if (data.Code.IsNullOrEmpty())
            {
                data.Code = StringHelper.GetRandomString(18);
            }
            return await ResultAsync(() => _repository.UpdateAsync(data));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">机构id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> DeleteAsync(IEnumerable<Guid> ids)
        {
            var data = await _repository.GetListAsync(ids);
            if (!data.Any()) return BaseErrType.DataEmpty;

            return await ResultAsync(() => _repository.DeleteRangeAsync(data));
        }
    }
}
