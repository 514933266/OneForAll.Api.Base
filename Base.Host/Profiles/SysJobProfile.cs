using AutoMapper;
using Sys.Application.Dtos;
using Sys.Domain.AggregateRoots;
using Sys.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sys.Host.Profiles
{
    public class SysJobProfile : Profile
    {
        public SysJobProfile()
        {
            CreateMap<SysJob, SysJobDto>()
                .ForMember(t => t.DepartmentId, a => a.MapFrom(e => e.SysDepartmentId))
                .ForMember(t => t.Roles, a => a.MapFrom(e => e.SysJobRoleContacts))
                .ForMember(t => t.Users, a => a.MapFrom(e => e.SysJobUserContacts));

            CreateMap<SysJobForm, SysJob>();
        }
    }
}
