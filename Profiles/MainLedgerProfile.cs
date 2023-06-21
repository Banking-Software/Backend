using AutoMapper;
using MicroFinance.Dtos.AccountSetup.MainLedger;
using MicroFinance.Models.AccountSetup;

namespace MicroFinance.Profiles
{
    public class MainLedgerProfile : Profile
    {
        public MainLedgerProfile()
        {
            // START: Account Type
            CreateMap<AccountTypeDto, AccountType>().ReverseMap();
            // END

            // START: Group Type
            CreateMap<CreateGroupTypeDto, GroupType>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.AccountType, opt => opt.Ignore());
            // .ForMember(dest=>dest.DebitOrCredit, opt=>opt.Ignore());

            CreateMap<GroupType, GroupTypeDto>();

            // END

            // START: Bank Setup Details
            CreateMap<CreateBankSetupDto, BankSetup>()
            .ForMember(dest=>dest.Ledger, opt=>opt.Ignore())
            .ForMember(dest=>dest.BankType, opt=>opt.Ignore());

            CreateMap<BankSetup, BankSetupDto>()
            .ForMember(dest=>dest.BankType, opt=> opt.MapFrom(src=>src.BankType.Name));
            
            CreateMap<BankType, BankTypeDto>().ReverseMap();
            // END

            // START: Ledger
            CreateMap<CreateLedgerDto, Ledger>()
            .ForMember(dest=>dest.GroupType, opt=>opt.Ignore())
            .ForMember(dest=>dest.IsBank, opt=>opt.Ignore());

            CreateMap<Ledger, LedgerDto>()
            .ForMember(dest=>dest.Schedule, opt=>opt.MapFrom(src=>src.GroupType.Schedule))
            .ForMember(dest=>dest.AccountTypeName, opt=>opt.MapFrom(src=>src.GroupType.AccountType.Name));
            
           
            // END

            // START: Subledger
            CreateMap<CreateSubLedgerDto, SubLedger>();
            CreateMap<SubLedger, SubLedgerDto>()
            .ForMember(dest=>dest.LedgerName, opt=>opt.MapFrom(src=>src.Ledger.Name))
            .ForMember(dest=>dest.AccountTypeName, opt=>opt.MapFrom(src=>src.Ledger.GroupType.AccountType.Name))
            .ForMember(dest=>dest.GroupTypeName, opt=>opt.MapFrom(src=>src.Ledger.GroupType.Name));
            // END


        }
    }
}