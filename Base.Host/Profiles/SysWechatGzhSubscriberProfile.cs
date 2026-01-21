using AutoMapper;
using Base.Application.Dtos;
using Base.Domain.Entities;
using Base.Domain.Aggregates;

namespace Base.Host.Profiles
{
    public class SysWechatGzhSubscriberProfile : Profile
    {
        public SysWechatGzhSubscriberProfile()
        {
            CreateMap<SysWechatGzhSubscriber, SysWxgzhSubscribeUserDto>();
        }
    }
}
