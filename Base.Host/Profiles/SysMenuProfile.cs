using AutoMapper;
using Sys.Application.Dtos;
using Sys.Domain.AggregateRoots;
using Sys.Domain.Aggregates;
using Sys.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sys.Host.Profiles
{
    public class SysMenuProfile : Profile
    {
        public SysMenuProfile()
        {
            CreateMap<SysMenu, SysMenuTreeDto>();
            CreateMap<SysMenu, SysMenuDto>();
            CreateMap<SysMenu, SysMenuPermissionAggr>();
            CreateMap<SysLoginUserMenu, SysPersonalMenuDto>()
                .ForMember(t => t.Permissions, a => a.MapFrom(e => e.SysLoginUserPermissions.Select(s => s.Code)));
            CreateMap<SysLoginUserMenu, SysTenantUserMenuDto>()
                .ForMember(t => t.Permissions, a => a.MapFrom(e => e.SysLoginUserPermissions.Select(s => s.Name)));
            CreateMap<SysMenuPermissionAggr, SysMenuTreeDto>()
                .ForMember(t => t.Permissions, a => a.MapFrom(e => e.SysPermissions));
        }
    }
}
