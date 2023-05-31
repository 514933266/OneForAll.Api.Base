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

namespace Base.Domain
{
    public class SysArticleTypeManager :BaseManager, ISysArticleTypeManager
    {
        private readonly IMapper _mapper;
        private readonly ISysArticleTypeRepository _typeRepository;
        public SysArticleTypeManager(
            IMapper mapper,
            ISysArticleTypeRepository typeRepository)
        {
            _mapper = mapper;
            _typeRepository = typeRepository;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns>列表</returns>
        public async Task<IEnumerable<SysArticleType>> GetTreeAsync()
        {
            return await _typeRepository.GetListAsync();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="tenantId">租户id</param>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> AddAsync(Guid tenantId, SysArticleTypeForm form)
        {
            var exists = await CheckParentExists(form);
            if (!exists) return BaseErrType.DataNotFound;

            var data = await _typeRepository.GetAsync(w => w.Name.Equals(form.Name));
            if (data != null) return BaseErrType.DataExist;

            data = _mapper.Map<SysArticleTypeForm, SysArticleType>(form);
            data.SysTenantId = tenantId;
            
            return await ResultAsync(() => _typeRepository.AddAsync(data));
        }

        private async Task<bool> CheckParentExists(SysArticleTypeForm form)
        {
            if (form.ParentId != Guid.Empty)
            {
                var menus = await _typeRepository.GetListAsync();
                var parent = menus.FirstOrDefault(w => w.Id.Equals(form.ParentId));
                var children = FindAllChildren(menus, form.Id);

                // 1. 禁止选择不存在的上级
                // 2. 禁止选择下级作为自己的上级
                if (parent == null) return false;
                if (children.Any(w => w.Id.Equals(form.ParentId))) return false;
            }
            return true;
        }

        private IEnumerable<SysArticleType> FindAllChildren(IEnumerable<SysArticleType> list, Guid parentId)
        {
            var result = new List<SysArticleType>();
            var data = list.ToList();
            var children = data.FindAll(w => w.ParentId == parentId);
            if (children.Count > 0)
            {
                children.ForEach(e =>
                {
                    var deepChildren = FindAllChildren(list, e.Id);
                    if (deepChildren.Any()) result.AddRange(deepChildren);
                });
            }
            return result;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> UpdateAsync(SysArticleTypeForm form)
        {
            var exists = await CheckParentExists(form);
            if (!exists) return BaseErrType.DataNotFound;

            var data = await _typeRepository.GetAsync(w => w.Name.Equals(form.Name));
            if (data != null && data.Id != form.Id) return BaseErrType.DataExist;

            data = await _typeRepository.FindAsync(form.Id);
            data.MapFrom(form);

            return await ResultAsync(() => _typeRepository.UpdateAsync(data));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">实体id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> DeleteAsync(Guid id)
        {
            var data = await _typeRepository.FindAsync(id);
            if (data == null) return BaseErrType.DataNotFound;

            return await ResultAsync(() => _typeRepository.DeleteAsync(data));
        }
    }
}
