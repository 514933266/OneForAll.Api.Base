using AutoMapper;
using Base.Application.Dtos;
using Base.Domain.AggregateRoots;
using Base.Domain.Aggregates;

namespace Base.Host.Profiles
{
    public class SysWxgzhSubscribeUserProfile : Profile
    {
        public SysWxgzhSubscribeUserProfile()
        {
            CreateMap<SysWxgzhSubscribeUser, SysWxgzhSubscribeUserDto>();
        }
    }
}
