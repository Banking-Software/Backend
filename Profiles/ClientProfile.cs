using AutoMapper;
using MicroFinance.Dtos.ClientSetup;
using MicroFinance.Models.ClientSetup;

namespace MicroFinance.Profiles
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<CreateClientDto, Client>()
            .ForMember(dest=>dest.Id, opt=>opt.Ignore())
            .ForMember(dest=>dest.ClientId, opt=>opt.Ignore())
            .ForMember(dest=>dest.ClientType, opt=>opt.Ignore())
            .ForMember(dest=>dest.ShareType, opt=>opt.Ignore())
            .ForMember(dest=>dest.KYMType, opt=>opt.Ignore())
            .ForMember(dest=>dest.ClientGroup, opt=>opt.Ignore())
            .ForMember(dest=>dest.ClientUnit, opt=>opt.Ignore());
            
            CreateMap<Client, ClientDto>()
            .ForMember(dest=>dest.ClientType, opt=>opt.MapFrom(src=>src.ClientType.Type))
            .ForMember(dest=>dest.ShareType, opt=>opt.MapFrom(src=>src.ShareType.Name))
            .ForMember(dest=>dest.ClientGroup, opt=>opt.MapFrom(src=>src.ClientGroup.Code))
            .ForMember(dest=>dest.ClientUnit, opt=>opt.MapFrom(src=>src.ClientUnit.Code))
            .ForMember(dest=>dest.KYMType, opt=>opt.MapFrom(src=>src.KYMType.Type));

            CreateMap<UpdateClientDto, Client>()
            .ForMember(dest=>dest.ClientType, opt=>opt.Ignore())
            .ForMember(dest=>dest.ShareType, opt=>opt.Ignore())
            .ForMember(dest=>dest.KYMType, opt=>opt.Ignore())
            .ForMember(dest=>dest.ClientGroup, opt=>opt.Ignore())
            .ForMember(dest=>dest.ClientUnit, opt=>opt.Ignore());

           

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