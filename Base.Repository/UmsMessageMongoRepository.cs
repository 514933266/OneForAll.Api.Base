using Base.Domain.AggregateRoots;
using Base.Domain.Enums;
using Base.Domain.Repositorys;
using MongoDB.Driver;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Repository
{
    /// <summary>
    /// 系统消息
    /// </summary>
    public class UmsMessageMongoRepository : IUmsMessageMongoRepository
    {
        private readonly string _name = "Ums_Message";
        private readonly IMongoDatabase _dbSet;
        public UmsMessageMongoRepository(IMongoDatabase dbSet)
        {
            _dbSet = dbSet;
        }


        /// <summary>
        /// 查询用户前5条未读消息
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="top">数量</param>
        /// <returns></returns>
        public async Task<IEnumerable<UmsMessage>> GetListAsync(Guid userId, int top)
        {
            var option = new FindOptions<UmsMessage>() { Limit = top, Sort = new SortDefinitionBuilder<UmsMessage>().Descending(o => o.CreateTime) };
            var predicate = Builders<UmsMessage>.Filter.Where(w => w.ToAccountId == userId && !w.IsRead);
            return (await _dbSet.GetCollection<UmsMessage>(_name).FindAsync(w => w.ToAccountId == userId && !w.IsRead, option)).ToList();
        }

        /// <summary>
        /// 查询用户消息分页列表
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <param name="status">状态</param>
        /// <returns>分页列表</returns>
        public async Task<PageList<UmsMessage>> GetPageAsync(Guid userId, int pageIndex, int pageSize, string key, UmsMessageStatusEnum status)
        {
            var option = new FindOptions<UmsMessage>() { Limit = pageSize, Skip = pageSize * (pageIndex - 1), Sort = new SortDefinitionBuilder<UmsMessage>().Ascending(o => o.IsRead).Descending(o => o.CreateTime) };

            var predicate = Builders<UmsMessage>.Filter.Where(w => w.ToAccountId == userId);
            switch (status)
            {
                case UmsMessageStatusEnum.UnRead:
                    predicate = Builders<UmsMessage>.Filter.And(predicate, Builders<UmsMessage>.Filter.Where(w => !w.IsRead));
                    break;
                case UmsMessageStatusEnum.Readed:
                    predicate = Builders<UmsMessage>.Filter.And(predicate, Builders<UmsMessage>.Filter.Where(w => w.IsRead));
                    break;
            }

            if (!string.IsNullOrEmpty(key))
            {
                predicate = Builders<UmsMessage>.Filter.And(predicate, Builders<UmsMessage>.Filter.Where(w => w.Title.Contains(key)));
            }

            var items = (await _dbSet.GetCollection<UmsMessage>(_name).FindAsync(predicate, option)).ToList();
            var total = await _dbSet.GetCollection<UmsMessage>(_name).CountDocumentsAsync(predicate);
            return new PageList<UmsMessage>((int)total, pageSize, pageIndex, items);
        }

        /// <summary>
        /// 查询用户未读数量
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        public async Task<int> GetUnReadCountAsync(Guid userId)
        {
            return (int)(await _dbSet.GetCollection<UmsMessage>(_name).CountDocumentsAsync(w => w.ToAccountId == userId && !w.IsRead));
        }

        /// <summary>
        /// 查询用户未读列表
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        public async Task<IEnumerable<UmsMessage>> GetUnReadListAsync(Guid userId)
        {
            return (await _dbSet.GetCollection<UmsMessage>(_name).FindAsync(w => w.ToAccountId == userId && !w.IsRead)).ToList();
        }

        /// <summary>
        /// 已读
        /// </summary>
        /// <returns>结果</returns>
        public async Task<int> UpdateIsReadAsync(Guid userId, IEnumerable<Guid> ids)
        {
            var update = Builders<UmsMessage>.Update.Set(w => w.IsRead, true);
            var result = await _dbSet.GetCollection<UmsMessage>(_name).UpdateManyAsync(w => ids.Contains(w.Id) && w.ToAccountId == userId && !w.IsRead, update);
            return (int)result.ModifiedCount;
        }

        /// <summary>
        /// 全部已读
        /// </summary>
        /// <returns>结果</returns>
        public async Task<int> UpdateIsReadAsync(Guid userId)
        {
            var update = Builders<UmsMessage>.Update.Set(w => w.IsRead, true);
            var result = await _dbSet.GetCollection<UmsMessage>(_name).UpdateManyAsync(w => w.ToAccountId == userId && !w.IsRead, update);
            return (int)result.ModifiedCount;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns>结果</returns>
        public async Task<int> DeleteAsync(Guid userId, IEnumerable<Guid> ids)
        {
            var result = await _dbSet.GetCollection<UmsMessage>(_name).DeleteManyAsync(w => ids.Contains(w.Id) && w.ToAccountId == userId);
            return (int)result.DeletedCount;
        }

        /// <summary>
        /// 全部删除
        /// </summary>
        /// <returns>结果</returns>
        public async Task<int> DeleteAsync(Guid userId)
        {
            var result = await _dbSet.GetCollection<UmsMessage>(_name).DeleteManyAsync(w => w.ToAccountId == userId);
            return (int)result.DeletedCount;
        }
    }
}
