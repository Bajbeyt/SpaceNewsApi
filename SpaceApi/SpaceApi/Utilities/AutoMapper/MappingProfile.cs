using AutoMapper;
using Entities.ModelDTO;
using Entities.Models;

namespace SpaceApi.Utilities.AutoMapper;

public class MappingProfile:Profile
{
    public MappingProfile()
    {
        CreateMap<RocketDto,Rocket>();
        CreateMap<NewsDto, News>();
        CreateMap<AxiomsDto, Axioms>();
        CreateMap<UserForRegistrationDto, User>();
        CreateMap<RolesDto, Roles>();
    }
}