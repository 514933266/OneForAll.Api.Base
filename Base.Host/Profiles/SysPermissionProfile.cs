using AutoMapper;
using Base.Application.Dtos;
using Base.Domain;
using Base.Domain.AggregateRoots;
using Base.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Host.Profiles
{
    public class SysPermissionProfile : Profile
    {
        public SysPermissionProfile()
        {
            CreateMap<SysPermission, SysMenuPermissionDto>();
            CreateMap<SysMenuPermissionForm, SysPermission>();
        }
    }
}
