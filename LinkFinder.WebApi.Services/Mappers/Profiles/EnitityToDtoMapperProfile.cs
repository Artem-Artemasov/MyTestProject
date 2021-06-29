using AutoMapper;
using LinkFinder.DbWorker.Models;
using LinkFinder.WebApi.Services.Response;

namespace LinkFinder.WebApi.Services.Mappers.Profiles
{
    public class EnitityToDtoMapperProfile : Profile
    {
        public EnitityToDtoMapperProfile()
        {
            CreateMap<Test, TestDto>();
            CreateMap<Result, ResultDto>();
            CreateMap<Test, DetailTestDto>();
        }
    }
}
