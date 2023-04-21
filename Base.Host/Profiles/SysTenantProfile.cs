using AutoMapper;
using Sys.Application.Dtos;
using Sys.Domain.AggregateRoots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Domain.Models;

namespace Sys.Host.Profiles
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
