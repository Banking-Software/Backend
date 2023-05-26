using AutoMapper;
using MicroFinance.Dtos.ClientSetup;
using MicroFinance.Models.ClientSetup;

namespace MicroFinance.Profiles
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<ClientNomineeDto, ClientNomineeInfo>().ReverseMap();
            CreateMap<ClientInfoDto, ClientInfo>().ReverseMap();
            CreateMap<ClientFamilyDto, ClientFamilyInfo>().ReverseMap();
            CreateMap<ClientContactDto, ClientContactInfo>().ReverseMap();
            CreateMap<ClientAddressDto, ClientAddressInfo>().ReverseMap();
            CreateMap<Client, ClientResponse>();
            CreateMap<ClientResponse,ClientDto >()
            .ForMember(dest=>dest.ClientAddress, opt=>opt.MapFrom(src=>src.ClientAddressInfo))
            .ForMember(dest=>dest.ClientFamily, opt=>opt.MapFrom(src=>src.ClientFamilyInfo))
            .ForMember(dest=>dest.ClientInfo, opt=>opt.MapFrom(src=>src.ClientInfo))
            .ForMember(dest=>dest.ClientContact, opt=>opt.MapFrom(src=>src.ClientContactInfo))
            .ForMember(dest=>dest.ClientNominee, opt=>opt.MapFrom(src=>src.ClientNomineeInfo))
            .ForMember(dest=>dest.CreatedOn, opt=>opt.MapFrom(src=>src.CreateOn))
            .ForMember(dest=>dest.EndedOn, opt=>opt.MapFrom(src=>src.EndedOn));

            // CreateMap<ClientDto, Client>()
            // .ForMember(dest=> dest.ClientInfo, opt=>opt.MapFrom(src=>src.ClientInfo))
            // .ForMember(dest=>dest.ClientContactInfo, opt=>opt.MapFrom(src=>src.ClientContact))
            // .ForMember(dest=> dest.ClientAddressInfo, opt=>opt.MapFrom(src=>src.ClientAddress))
            // .ForMember(dest=>dest.ClientAccountTypeInfo, opt=>opt.MapFrom(src=>src.ClientAccountType))
            // .ForMember(dest=> dest.ClientTypeInfo, opt=>opt.MapFrom(src=>src.ClientType))
            // .ForMember(dest=>dest.ClientFamilyInfo, opt=>opt.MapFrom(src=>src.ClientFamily))
            // .ForMember(dest=> dest.ClientNomineeInfo, opt=>opt.MapFrom(src=>src.ClientNominee))
        }
    }
}