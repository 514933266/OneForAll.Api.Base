using AutoMapper;
using Base.Application.Dtos;
using Base.Domain;
using Base.Domain.AggregateRoots;
using System;
using System.Collections.Generic;
using System.Text;
using Base.Domain.Models;

namespace Base.Host.Profiles
{
    public class UmsMessageProfile : Profile
    {
        public UmsMessageProfile()
        {
            CreateMap<UmsMessage, SysPersonalMessageDto>();
        }
    }
}
