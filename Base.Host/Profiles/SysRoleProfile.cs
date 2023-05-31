using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Base.Domain.AggregateRoots;
using Base.Application.Dtos;
using Base.Domain.Models;
using Base.Domain.Aggregates;

namespace Base.Host.Profiles
{
    public class SysRoleProfile : Profile
    {
        public SysRoleProfile()
        {
            CreateMap<SysRole, SysRoleDto>();
            CreateMap<SysRole, SysRoleAggr>();
            CreateMap<SysRoleAggr, SysRoleDto>();

            CreateMap<SysRoleForm, SysRole>();
        }
    }
}
