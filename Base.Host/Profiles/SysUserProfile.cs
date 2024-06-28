using AutoMapper;
using Base.Application.Dtos;
using Base.Domain;
using Base.Domain.AggregateRoots;
using Base.Domain.Aggregates;
using Base.Domain.Models;
using OneForAll.Core.Extension;
using OneForAll.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base.Host.Profiles
{
    public class SysUserProfile : Profile
    {
        public SysUserProfile()
        {
            CreateMap<SysUser, SysLoginUserAggr>();
            CreateMap<SysTenantUserAggr, SysLoginUserAggr>()
                .ForMember(t => t.SysTenant, a => a.MapFrom(s => s.SysTenant)); 
            CreateMap<SysLoginUserAggr, SysPersonalDto>()
                .ForMember(t => t.TenantId, a => a.MapFrom(s => s.SysTenant.Id))
                .ForMember(t => t.TenantName, a => a.MapFrom(s => s.SysTenant.Name))
                .ForMember(t => t.TenantLogoUrl, a => a.MapFrom(s => s.SysTenant.LogoUrl))
                .ForMember(t => t.Menus, a => a.MapFrom(s => s.SysLoginUserMenus));
            CreateMap<SysLoginUserAggr, SysTenantUserDto>()
                .ForMember(t => t.Menus, a => a.MapFrom(s => s.SysLoginUserMenus));
            CreateMap<SysUser, SysTenantUserDto>();

            CreateMap<SysUserForm, SysUser>();
            CreateMap<SysUserUpdateForm, SysUser>();
            CreateMap<SysPersonalForm, SysUser>();

            CreateMap<SysUser, SysRoleMemberDto>();
            CreateMap<SysUser, SysRoleSelectionMemberDto>();
            CreateMap<SysUser, SysAreaGroupMemberDto>();
            CreateMap<SysUser, SysAreaGroupSelectionMemberDto>();
        }
    }
}
