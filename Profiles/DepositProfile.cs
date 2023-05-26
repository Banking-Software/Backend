using AutoMapper;
using MicroFinance.Dtos.DepositSetup;
using MicroFinance.Models.DepositSetup;

namespace MicroFinance.Profiles
{
    public class DepositProfile : Profile
    {
        public DepositProfile()
        {
            CreateMap<DepositScheme, ResponseDepositScheme>()
            .ForMember(dest=>dest.PostingScheme, opt=>opt.MapFrom(src=>src.PostingScheme.Name));
            CreateMap<CreateDepositSchemeDto, DepositScheme>();
            CreateMap<UpdateDepositSchemeDto, UpdateDepositScheme>();
            CreateMap<ResponseDepositScheme,DepositSchemeDto>();

            // Deposit Account
            CreateMap<DepositAccount, DepositAccountDto>()
            .ForMember(dest => dest.DepositScheme, opt => opt.Ignore())
            .ForMember(dest=>dest.Client, opt=>opt.Ignore())
            .ForMember(dest=>dest.JointClient, opt=>opt.Ignore())
            .ForMember(dest=>dest.PeriodType, opt=>opt.Ignore())
            .ForMember(dest=>dest.AccountType, opt=>opt.Ignore())
            .ForMember(dest=>dest.Status, opt=>opt.Ignore());

            // Flexible Interest Rate

            CreateMap<FlexibleInterestRateSetupDto, FlexibleInterestRate>()
            .ForMember(dest=>dest.Id, opt=>opt.Ignore());

        }
    }
}
