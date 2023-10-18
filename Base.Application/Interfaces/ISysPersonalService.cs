﻿using Base.Application.Dtos;
using Base.Domain.Enums;
using Base.Domain.Models;
using Base.Domain.ValueObjects;
using OneForAll.Core;
using OneForAll.Core.Upload;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Base.Application.Interfaces
{
    /// <summary>
    /// 个人中心
    /// </summary>
    public interface ISysPersonalService
    {
        /// <summary>
        /// 获取个人信息
        /// </summary>
        /// <returns>实体</returns>
        Task<SysPersonalDto> GetAsync();

        /// <summary>
        /// 修改个人信息
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        Task<BaseErrType> UpdateAsync(SysPersonalForm form);

        /// <summary>
        /// 上传头像
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="file">文件流</param>
        /// <returns>上传结果</returns>
        Task<IUploadResult> UploadHeaderAsync(string filename, Stream file);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="password">密码</param>
        /// <returns>结果</returns>
        Task<BaseErrType> ChangePasswordAsync(Password password);

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns>结果</returns>
        Task<IEnumerable<SysMenuTreeDto>> GetListMenuTreeAsync();

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns>结果</returns>
        Task<IEnumerable<SysMenuDto>> GetListMenuAsync();

        /// <summary>
        /// 登录日志
        /// </summary>
        /// <param name="form">表单</param>
        /// <returns>结果</returns>
        Task<BaseErrType> LoginAsync(SysPersonalLoginLogForm form);

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns>结果</returns>
        Task<BaseErrType> LoginoutAsync();
    }
}
