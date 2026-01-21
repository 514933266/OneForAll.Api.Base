using AutoMapper;
using Base.Application.Dtos;
using Base.Domain.Entities;
using Base.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Host.Profiles
{
    public class SysArticleTypeProfile : Profile
    {
        public SysArticleTypeProfile()
        {
            CreateMap<SysArticleType, SysArticleTypeDto>();

            CreateMap<SysArticleTypeForm, SysArticleType>();
        }
    }
}