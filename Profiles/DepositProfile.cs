using AutoMapper;
using MicroFinance.Dtos.DepositSetup;
using MicroFinance.Dtos.DepositSetup.Account;
using MicroFinance.Models.DepositSetup;

namespace MicroFinance.Profiles
{
    public class DepositProfile : Profile
    {
        public DepositProfile()
        {
            // CreateMap<DepositScheme, ResponseDepositScheme>()
            // .ForMember(dest=>dest.PostingScheme, opt=>opt.MapFrom(src=>src.PostingScheme.));
            CreateMap<CreateDepositSchemeDto, DepositScheme>()
            .ForMember(dest=>dest.TaxSubledger, opt=>opt.Ignore())
            .ForMember(dest=>dest.InterestSubledger, opt=>opt.Ignore())
            .ForMember(dest=>dest.DepositSubLedger, opt=>opt.Ignore())
            .ForMember(dest=>dest.SchemeType, opt=>opt.Ignore());

            CreateMap<DepositScheme, DepositSchemeDto>()
            .ForMember(dest=>dest.SchemeType, opt=>opt.MapFrom(src=>src.SchemeType.Name))
            .ForMember(dest=>dest.DepositSubLedger, opt=>opt.MapFrom(src=>src.DepositSubLedger.Name))
            .ForMember(dest=>dest.InterestSubledger, opt=>opt.MapFrom(src=>src.InterestSubledger.Name))
            .ForMember(dest=>dest.TaxSubledger, opt=>opt.MapFrom(src=>src.TaxSubledger.Name));

            CreateMap<CreateDepositAccountDto, DepositAccount>()
            .ForMember(dest=>dest.Id, opt=>opt.Ignore())
            .ForMember(dest=>dest.DepositScheme, opt=>opt.Ignore())
            .ForMember(dest=>dest.Client, opt=>opt.Ignore())
            .ForMember(dest=>dest.ReferredByEmployeeId, opt=>opt.Ignore())
            .ForMember(dest=>dest.InterestPostingAccountNumber, opt=>opt.Ignore())
            .ForMember(dest=>dest.MatureInterestPostingAccountNumber, opt=>opt.Ignore());

            // Deposit Account
            CreateMap<DepositAccount, DepositAccountDto>()
            .ForMember(dest=>dest.InterestPostingAccountNumber, opt=>opt.MapFrom(src=>src.InterestPostingAccountNumber!=null?src.InterestPostingAccountNumber.AccountNumber:null))
            .ForMember(dest=>dest.MatureInterestPostingAccountNumber, opt=>opt.MapFrom(src=>src.MatureInterestPostingAccountNumber!=null?src.MatureInterestPostingAccountNumber.AccountNumber:null))
            .ForMember(dest=>dest.SignatureFileData, opt=>opt.MapFrom(src=>(src.SignatureFileData!=null?Convert.ToBase64String(src.SignatureFileData):null)));

            CreateMap<JointAccount, JointAccountDto>();

            // Flexible Interest Rate

            CreateMap<FlexibleInterestRateSetupDto, FlexibleInterestRate>()
            .ForMember(dest=>dest.Id, opt=>opt.Ignore());

        }
    }
}
