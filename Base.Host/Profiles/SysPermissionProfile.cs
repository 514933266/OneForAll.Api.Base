using AutoMapper;
using Base.Application.Dtos;
using Base.Domain;
using Base.Domain.Entities;
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
