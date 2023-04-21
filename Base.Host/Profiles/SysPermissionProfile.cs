using AutoMapper;
using Sys.Application.Dtos;
using Sys.Domain;
using Sys.Domain.AggregateRoots;
using Sys.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sys.Host.Profiles
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
