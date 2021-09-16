using AutoMapper;
using LinkFinder.DbWorker.Models;
using LinkFinder.WebApi.Logic.Response;

namespace LinkFinder.WebApi.Logic.Mappers.Profiles
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
