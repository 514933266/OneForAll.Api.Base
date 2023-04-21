using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Sys.Domain.AggregateRoots;
using Sys.Application.Dtos;
using Sys.Domain.Models;
using Sys.Domain.Aggregates;

namespace Sys.Host.Profiles
{
    public class SysRoleProfile : Profile
    {
        public SysRoleProfile()
        {
            CreateMap<SysRole, SysRoleDto>();
            CreateMap<SysRole, SysRoleAggr>();
            CreateMap<SysRoleAggr, SysRoleDto>();
            CreateMap<SysRole, SysJobRoleSelectionDto>();

            CreateMap<SysRoleForm, SysRole>();
        }
    }
}
