using AutoMapper;
using Sys.Application.Dtos;
using Sys.Domain.AggregateRoots;
using Sys.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sys.Host.Profiles
{
    public class SysDepartmentProfile : Profile
    {
        public SysDepartmentProfile()
        {
            CreateMap<SysDepartment, SysDepartmentDto>();

            CreateMap<SysDepartmentForm, SysDepartment>();
        }
    }
}
