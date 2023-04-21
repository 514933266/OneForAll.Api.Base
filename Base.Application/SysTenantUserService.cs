using AutoMapper;
using Sys.Application.Dtos;
using Sys.Application.Interfaces;
using Sys.Domain;
using Sys.Domain.AggregateRoots;
using Sys.Domain.Interfaces;
using Sys.Domain.Models;
using Sys.Domain.Repositorys;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sys.Domain.Aggregates;

namespace Sys.Application
{
    /// <summary>
    /// 用户
    /// </summary>
    public class SysTenantUserService : ISysTenantUserService
    {
        private readonly IMapper _mapper;
        private readonly ISysTenantUserManager _userManager;
        private readonly ISysUserRepository _userRepository;

        public SysTenantUserService(
            IMapper mapper,
            ISysTenantUserManager userManager,
            ISysUserRepository userRepository
            )
        {
            _mapper = mapper;
            _userManager = userManager;
            _userRepository = userRepository;
        }

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <returns>分页</returns>
        public async Task<PageList<SysTenantUserDto>> GetPageAsync(int pageIndex, int pageSize, string key)
        {
            var data = await _userManager.GetPageAsync(pageIndex, pageSize, key);
            var items = _mapper.Map<IEnumerable<SysLoginUserAggr>, IEnumerable<SysTenantUserDto>>(data.Items);
            return new PageList<SysTenantUserDto>(data.Total, data.PageSize, data.PageIndex, items);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="ids">实体id</param>
        /// <returns>结果</returns>
        public async Task<IEnumerable<SysTenantUserDto>> GetListAsync(IEnumerable<Guid> ids)
        {
            var data = new List<SysUser>();
            if (ids.Any())
            {
                data = await _userRepository.GetListAsync(w => ids.Contains(w.Id)) as List<SysUser>;
            }
            else
            {
                data = await _userRepository.GetListAsync() as List<SysUser>;
            }
            return _mapper.Map<IEnumerable<SysUser>, IEnumerable<SysTenantUserDto>>(data);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="form">用户</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> AddAsync(SysUserForm form)
        {
            return await _userManager.AddAsync(form);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="form">用户</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> UpdateAsync(SysUserUpdateForm form)
        {
            return await _userManager.UpdateAsync(form);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">用户id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> DeleteAsync(IEnumerable<Guid> ids)
        {
            return await _userManager.DeleteAsync(ids);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="ids">用户id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> ResetPasswordAsync(IEnumerable<Guid> ids)
        {
            return await _userManager.ResetPasswordAsync(ids);
        }
    }
}
