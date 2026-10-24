using Base.Domain.Entities;
using Base.Domain.Interfaces;
using Base.Domain.Repositorys;
using System;
using System.Threading.Tasks;

namespace Base.Domain.Querys
{
    /// <summary>
    /// 微信相关读路径服务实现
    /// </summary>
    public class SysWechatQueryService : ISysWechatQueryService
    {
        private readonly ISysWechatUserRepository _wxUserRepository;
        private readonly ISysWechatGzhSubscriberRepository _wxgzhRepository;

        public SysWechatQueryService(
            ISysWechatUserRepository wxUserRepository,
            ISysWechatGzhSubscriberRepository wxgzhRepository)
        {
            _wxUserRepository = wxUserRepository;
            _wxgzhRepository = wxgzhRepository;
        }

        /// <summary>
        /// 按系统用户id获取微信用户
        /// </summary>
        /// <param name="userId">系统用户id</param>
        /// <returns>微信用户，不存在返回 null</returns>
        public async Task<SysWechatUser> GetUserAsync(Guid userId)
        {
            return await _wxUserRepository.GetAsync(w => w.SysUserId == userId);
        }

        /// <summary>
        /// 按系统用户id获取微信公众号关注者
        /// </summary>
        /// <param name="userId">系统用户id</param>
        /// <returns>关注者，不存在返回 null</returns>
        public async Task<SysWechatGzhSubscriber> GetGzhSubscriberAsync(Guid userId)
        {
            return await _wxgzhRepository.GetAsync(w => w.SysUserId == userId);
        }
    }
}
