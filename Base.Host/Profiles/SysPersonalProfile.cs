using AutoMapper;
using Base.Application.Dtos;
using Base.Domain.AggregateRoots;
using Base.Domain.Models;
using Base.HttpService.Models;

namespace Base.Host.Profiles
{
    public class SysPersonalProfile : Profile
    {
        public SysPersonalProfile()
        {
            CreateMap<SysPersonalLoginLogForm, SysLoginLogForm>();
        }
    }
}
