using AutoMapper;
using Sys.Application.Dtos;
using Sys.Domain.AggregateRoots;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sys.Host.Profiles
{
    public class SysJobRoleContactProfile : Profile
    {
        public SysJobRoleContactProfile()
        {
            CreateMap<SysJobRoleContact, SysJobRoleDto>()
                .ForMember(t => t.Name, a => a.MapFrom(e => e.SysRole.Name))
                .ForMember(t => t.Remark, a => a.MapFrom(e => e.SysRole.Remark));

            CreateMap<SysJobRoleContact, SysDepartmentRoleDto>()
                .ForMember(t => t.DepartmentId, a => a.MapFrom(e => e.SysJob.SysDepartmentId))
                .ForMember(t => t.Name, a => a.MapFrom(e => e.SysRole.Name))
                .ForMember(t => t.Remark, a => a.MapFrom(e => e.SysRole.Remark));
        }
    }
}