using AutoMapper;
using Base.Application.Dtos;
using Base.Domain.AggregateRoots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base.Domain.Models;

namespace Base.Host.Profiles
{
    public class SysTenantProfile : Profile
    {
        public SysTenantProfile()
        {
            CreateMap<SysTenant, SysTenantDto>();
            CreateMap<SysTenantForm, SysTenant>();
        }
    }
}
