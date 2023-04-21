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
    public class SysAreaGroupProfile : Profile
    {
        public SysAreaGroupProfile()
        {
            CreateMap<SysAreaGroup, SysAreaGroupDto>()
                .ForMember(d => d.MemberCount, a => a.MapFrom(s => s.SysAreaGroupUserContacts.Count));
            CreateMap<SysAreaGroupForm, SysAreaGroup>();
        }
    }
}
