using AutoMapper;
using Sys.Application.Dtos;
using Sys.Domain.AggregateRoots;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sys.Host.Profiles
{
    public class SysJobUserContactProfile : Profile
    {
        public SysJobUserContactProfile()
        {
            CreateMap<SysJobUserContact, SysJobUserDto>()
                .ForMember(t => t.Name, a => a.MapFrom(e => e.SysUser.Name))
                .ForMember(t => t.UserName, a => a.MapFrom(e => e.SysUser.UserName))
                .ForMember(t => t.IconUrl, a => a.MapFrom(e => e.SysUser.IconUrl));

            CreateMap<SysJobUserContact, SysDepartmentUserDto>()
                .ForMember(t => t.DepartmentId, a => a.MapFrom(e => e.SysJob.SysDepartmentId))
                .ForMember(t => t.Name, a => a.MapFrom(e => e.SysUser.Name))
                .ForMember(t => t.UserName, a => a.MapFrom(e => e.SysUser.UserName))
                .ForMember(t => t.IconUrl, a => a.MapFrom(e => e.SysUser.IconUrl));
        }
    }
}
