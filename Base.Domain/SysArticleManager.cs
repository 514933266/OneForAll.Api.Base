using AutoMapper;
using Sys.Domain.AggregateRoots;
using Sys.Domain.Aggregates;
using Sys.Domain.Interfaces;
using Sys.Domain.Models;
using Sys.Domain.Repositorys;
using OneForAll.Core;
using OneForAll.Core.DDD;
using OneForAll.Core.Extension;
using OneForAll.Core.Upload;
using OneForAll.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Domain
{
    public class SysArticleManager : BaseManager, ISysArticleManager
    {
        // 文件存储路径
        private readonly string UPLOAD_PATH = "upload/tenants/{0}/articles/{1}";
        // 虚拟路径：根据Startup配置设置,返回给前端访问资源
        private readonly string VIRTUAL_PATH = "resources/tenants/{0}/articles/{1}";

        private readonly IMapper _mapper;
        private readonly IUploader _uploader;
        private readonly ISysArticleTypeRepository _typeRepository;
        private readonly ISysArticleRepository _articleRepository;
        private readonly ISysArticleRecordRepository _recordRepository;
        public SysArticleManager(
            IMapper mapper,
            IUploader uploader,
            ISysArticleRepository articleRepository,
            ISysArticleTypeRepository typeRepository,
            ISysArticleRecordRepository recordRepository)
        {
            _mapper = mapper;
            _uploader = uploader;
            _typeRepository = typeRepository;
            _articleRepository = articleRepository;
            _recordRepository = recordRepository;
        }

        /// <summary>
        /// 获取已发布分页列表（含用户阅读记录）
        /// </summary>
        /// <param name="user">当前登录用户</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <returns>分页列表</returns>
        public async Task<PageList<SysArticle>> GetPagePublishedAsync(SysLoginUserAggr user, int pageIndex, int pageSize)
        {
            if (pageIndex < 1)  pageIndex = 1;
            if (pageSize < 1)   pageSize = 10;
            if (pageSize > 100) pageSize = 100;
            var data = await _articleRepository.GetPagePublishedAsync(pageIndex, pageSize);
            var records = await _recordRepository.GetListAsync(user.Id, data.Items.Select(s => s.Id));
            data.Items.ForEach(e =>
            {
                e.SysArticleRecords = records.Where(w => w.SysArticleId.Equals(e.Id)).ToList();
            });
            return data;
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="typeId">分类Id</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <returns>分页列表</returns>
        public async Task<PageList<SysArticle>> GetPageAsync(Guid typeId, int pageIndex, int pageSize, string key)
        {
            if (pageIndex < 1)  pageIndex = 1;
            if (pageSize < 1)   pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var ids = new List<Guid>();
            if (!typeId.Equals(Guid.Empty))
            {
                ids.Add(typeId);
                var types = await _typeRepository.GetListAsync();
                var children = FindAllChildren(types, typeId);
                ids.AddRange(children.Select(s => s.Id));
            }
            return await _articleRepository.GetPageAsync(ids, pageIndex, pageSize, key);
        }

        private IEnumerable<SysArticleType> FindAllChildren(IEnumerable<SysArticleType> types, Guid parentId)
        {
            var result = new List<SysArticleType>();
            var data = types.ToList();
            var children = data.FindAll(w => w.ParentId == parentId);
            if (children.Count > 0)
            {
                children.ForEach(e =>
                {
                    var deepChildren = FindAllChildren(types, e.Id);
                    if (deepChildren.Any()) result.AddRange(deepChildren);
                });
            }
            return result;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="user">当前登录用户</param>
        /// <param name="tenantId">租户id</param>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> AddAsync(SysLoginUserAggr user, Guid tenantId, SysArticleForm form)
        {
            var type = await _typeRepository.FindAsync(form.TypeId);
            if (type == null) return BaseErrType.DataNotFound;

            var data = _mapper.Map<SysArticleForm, SysArticle>(form);
            data.SysTenantId = tenantId;
            data.CreatorId = user.Id;
            data.CreatorName = user.Name;
            data.CreateTime = DateTime.Now;
            return await ResultAsync(() => _articleRepository.AddAsync(data));
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> UpdateAsync(SysArticleForm form)
        {
            var menu = await _typeRepository.FindAsync(form.TypeId);
            if (menu == null) return BaseErrType.DataNotFound;

            var data = await _articleRepository.FindAsync(form.Id);
            if (data == null) return BaseErrType.DataNotFound;

            data.MapFrom(form);
            return await ResultAsync(() => _articleRepository.UpdateAsync(data));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">文章id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> DeleteAsync(Guid id)
        {
            var data = await _articleRepository.FindAsync(id);
            if (data == null) return BaseErrType.DataNotFound;

            return await ResultAsync(() => _articleRepository.DeleteAsync(data));
        }

        /// <summary>
        /// 上传封面
        /// </summary>
        /// <param name="tenantId">租户id</param>
        /// <param name="id">文章id</param>
        /// <param name="filename">文件名</param>
        /// <param name="file">文件流</param>
        /// <returns>结果</returns>
        public async Task<IUploadResult> UploadCoverAsync(Guid tenantId, Guid id, string filename, Stream file)
        {
            var maxSize = 500 * 1024;
            return await UploadAsync(tenantId, id, filename, file, maxSize, true);
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="tenantId">租户id</param>
        /// <param name="id">文章id</param>
        /// <param name="filename">文件名</param>
        /// <param name="file">文件流</param>
        /// <returns>结果</returns>
        public async Task<IUploadResult> UploadImageAsync(Guid tenantId, Guid id, string filename, Stream file)
        {
            var maxSize = 2 * 1024 * 1024;
            return await UploadAsync(tenantId, id, filename, file, maxSize, false);
        }

        private async Task<IUploadResult> UploadAsync(Guid tenantId, Guid id, string filename, Stream file, int maxSize, bool isCover)
        {
            var result = new UploadResult() { Original = filename, Title = filename };
            var data = await _articleRepository.FindAsync(id);
            if (data == null)
            {
                data = new SysArticle();
                data.Id = id;
                data.SysTenantId = tenantId;
            }
            if (new ValidateImageType().Validate(filename, file))
            {
                if (isCover) filename = "cover-".Append(filename);
                result = await _uploader.WriteAsync(file, UPLOAD_PATH.Fmt(data.SysTenantId, id), filename, maxSize) as UploadResult;
                // 设置返回虚拟路径
                if (result.State.Equals(UploadEnum.Success))
                {
                    result.Url = Path.Combine(VIRTUAL_PATH.Fmt(data.SysTenantId, id), result.Url);
                }
            }
            else
            {
                result.State = UploadEnum.TypeError;
            }
            return result;
        }


        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="id">文章id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> PublishAsync(Guid id)
        {
            var data = await _articleRepository.FindAsync(id);
            if (data == null) return BaseErrType.DataNotFound;

            data.IsPublish = true;
            return await ResultAsync(() => _articleRepository.UpdateAsync(data));
        }

        /// <summary>
        /// 阅读文章
        /// </summary>
        /// <param name="user">当前登录用户</param>
        /// <param name="id">文章id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> ReadAsync(SysLoginUserAggr user, Guid id)
        {
            var article = await _articleRepository.FindAsync(id);
            if (article == null) return BaseErrType.DataNotFound;

            var data = await _recordRepository.GetAsync(user.Id, article.Id, DateTime.Now);
            if (data != null) return BaseErrType.DataExist;

            data = new SysArticleRecord()
            {
                CreateTime = DateTime.Now,
                SysArticleId = article.Id,
                SysUserId = user.Id
            };
            return await ResultAsync(() => _recordRepository.AddAsync(data));
        }
    }
}
