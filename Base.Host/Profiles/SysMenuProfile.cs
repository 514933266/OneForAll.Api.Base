using AutoMapper;
using Base.Application.Dtos;
using Base.Domain.AggregateRoots;
using Base.Domain.Aggregates;
using Base.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base.Host.Profiles
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
