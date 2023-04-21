using AutoMapper;
using Sys.Application.Dtos;
using Sys.Domain;
using Sys.Domain.AggregateRoots;
using Sys.Domain.Aggregates;
using Sys.Domain.Models;
using OneForAll.Core.Extension;
using OneForAll.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sys.Host.Profiles
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
                .ForMember(t => t.Menus, a => a.MapFrom(s => s.SysLoginUserMenus));
            CreateMap<SysLoginUserAggr, SysTenantUserDto>()
                .ForMember(t => t.Menus, a => a.MapFrom(s => s.SysLoginUserMenus));
            CreateMap<SysUser, SysTenantUserDto>();

            CreateMap<SysUserForm, SysUser>();
            CreateMap<SysPersonalForm, SysUser>();

            CreateMap<SysUser, SysRoleMemberDto>();
            CreateMap<SysUser, SysRoleSelectionMemberDto>();
            CreateMap<SysUser, SysAreaGroupMemberDto>();
            CreateMap<SysUser, SysAreaGroupSelectionMemberDto>();
            CreateMap<SysUser, SysJobUserSelectionDto>();
        }
    }
}
