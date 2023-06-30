using AutoMapper;
using MicroFinance.Dtos.RecordsWithCode;
using MicroFinance.Models.RecordsWithCode;

namespace MicroFinance.Profiles
{
    public class RecordsWithCode : Profile
    {
        public RecordsWithCode()
        {
            CreateMap<Cast, CastDto>();
            CreateMap<District, DistrictDto>();
            CreateMap<MaritalStatus, MaritalStatusDto>();
            CreateMap<Gender, GenderDto>();
            CreateMap<State, StateDto>();
        }
    }
}