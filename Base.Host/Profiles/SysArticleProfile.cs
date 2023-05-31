using AutoMapper;
using Base.Application.Dtos;
using Base.Domain.AggregateRoots;
using Base.Domain.Models;
using OneForAll.Core.Extension;
using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Host.Profiles
{
    public class SysArticleProfile : Profile
    {
        public SysArticleProfile()
        {
            CreateMap<SysArticle, SysArticleDto>()
                .ForMember(e => e.TypeId, a => a.MapFrom(e => e.SysArticleTypeId))
                .ForMember(e => e.TypeName, a => a.MapFrom(e => e.SysArticleType.Name))
                .ForMember(e => e.CreateTime, a => a.MapFrom(e => GetCreateTimeStr(e.CreateTime)));

            CreateMap<SysArticle, SysReadArticleDto>()
                .ForMember(e => e.TypeName, a => a.MapFrom(e => e.SysArticleType.Name))
                .ForMember(e => e.CreateTime, a => a.MapFrom(e => GetCreateTimeStr(e.CreateTime)));

            CreateMap<SysArticleForm, SysArticle>()
                .ForMember(e => e.SysArticleTypeId, a => a.MapFrom(e => e.TypeId));
        }

        private string GetCreateTimeStr(DateTime createTime)
        {
            var timeSpan = DateTime.Now - createTime;
            if (timeSpan.TotalDays >= 30 && timeSpan.TotalDays < 90)
            {
                var months = timeSpan.TotalDays / 30;
                return "{0}月前".Fmt(Math.Floor(months));
            }
            else if (timeSpan.TotalDays >= 7)
            {
                var weeks = timeSpan.TotalDays / 7;
                return "{0}周前".Fmt(Math.Floor(weeks));
            }
            else if (timeSpan.TotalDays >= 1 && timeSpan.TotalDays < 7)
            {
                return "{0}天前".Fmt(Math.Floor(timeSpan.TotalDays));
            }
            else if (timeSpan.TotalHours >= 1 && timeSpan.TotalHours < 24)
            {
                return "{0}小时前".Fmt(Math.Floor(timeSpan.TotalHours));
            }
            else if (timeSpan.TotalMinutes >= 0 && timeSpan.TotalMinutes < 60)
            {
                if (timeSpan.TotalMinutes < 1) return "{0}分钟前".Fmt(1);
                return "{0}分钟前".Fmt(Math.Floor(timeSpan.TotalMinutes));
            }
            else
            {
                return createTime.ToString("yyyy年MM月dd日 hh:mm");
            }
        }
    }
}