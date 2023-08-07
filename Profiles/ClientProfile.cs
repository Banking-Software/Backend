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
            .ForMember(dest=>dest.ClientTypeId, opt=>opt.MapFrom(src=>(int) src.ClientType))
            .ForMember(dest=>dest.ShareType, opt=>opt.Ignore())
            .ForMember(dest=>dest.ClientShareTypeInfoId, opt=>opt.MapFrom(src=>((int?)src.ShareType)))
            .ForMember(dest=>dest.KYMType, opt=>opt.Ignore())
            .ForMember(dest=>dest.KYMTypeId, opt=>opt.MapFrom(src=>(int?)src.KYMType))
            .ForMember(dest=>dest.ClientGroup, opt=>opt.Ignore())
            .ForMember(dest=>dest.ClientUnit, opt=>opt.Ignore());
            
            CreateMap<Client, ClientDto>()
            .ForMember(dest=>dest.ClientType, opt=>opt.MapFrom(src=>src.ClientType.Type))
            .ForMember(dest=>dest.ShareType, opt=>opt.MapFrom(src=>src.ShareType.Name))
            .ForMember(dest=>dest.ClientGroup, opt=>opt.MapFrom(src=>src.ClientGroup.Code))
            .ForMember(dest=>dest.ClientUnit, opt=>opt.MapFrom(src=>src.ClientUnit.Code))
            .ForMember(dest=>dest.KYMType, opt=>opt.MapFrom(src=>src.KYMType.Type))
            .ForMember(dest=>dest.ClientPhotoFileData,opt=>opt.MapFrom(src=>(src.ClientPhotoFileData!=null?Convert.ToBase64String(src.ClientPhotoFileData):null)))
            .ForMember(dest=>dest.ClientCitizenshipFileData, opt=>opt.MapFrom(src=>(src.ClientCitizenshipFileData!=null?Convert.ToBase64String(src.ClientCitizenshipFileData):null)))
            .ForMember(dest=>dest.ClientSignatureFileData, opt=>opt.MapFrom(src=>(src.ClientSignatureFileData!=null?Convert.ToBase64String(src.ClientSignatureFileData):null)))
            .ForMember(dest=>dest.NomineePhotoFileData, opt=>opt.MapFrom(src=>(src.NomineePhotoFileData!=null?Convert.ToBase64String(src.NomineePhotoFileData):null)));


            CreateMap<UpdateClientDto, Client>()
            .ForMember(dest=>dest.Id, opt=>opt.Ignore())
            .ForMember(dest=>dest.ClientId, opt=>opt.Ignore())
            .ForMember(dest=>dest.ClientType, opt=>opt.Ignore())
            .ForMember(dest=>dest.ClientTypeId, opt=>opt.Ignore())
            .ForMember(dest=>dest.ShareType, opt=>opt.Ignore())
            .ForMember(dest=>dest.ClientShareTypeInfoId, opt=>opt.Ignore())
            .ForMember(dest=>dest.KYMType, opt=>opt.Ignore())
            .ForMember(dest=>dest.KYMTypeId, opt=>opt.Ignore())
            .ForMember(dest=>dest.ClientGroup, opt=>opt.Ignore())
            .ForMember(dest=>dest.ClientGroupId, opt=>opt.Ignore())
            .ForMember(dest=>dest.ClientUnitId, opt=>opt.Ignore())
            .ForMember(dest=>dest.ClientUnit, opt=>opt.Ignore())
            .ForMember(dest=>dest.ClientPhotoFileData, opt=>opt.Ignore())
            .ForMember(dest=>dest.ClientSignatureFileData, opt=>opt.Ignore())
            .ForMember(dest=>dest.ClientCitizenshipFileData, opt=>opt.Ignore())
            .ForMember(dest=>dest.NomineePhotoFileData, opt=>opt.Ignore());
        }
    }
}