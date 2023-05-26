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
            .ForMember(dest => dest.AccountType, opt => opt.Ignore())
            .ForMember(dest => dest.GroupTypeDetails, opt => opt.Ignore())
            .ForMember(dest => dest.GroupTypeAndLedgerMap, opt => opt.Ignore());

            CreateMap<GroupType, GroupTypeAccounTypeDetailsDto>()
            .ForMember(dest => dest.GroupType, opt => opt.MapFrom(src => new GroupTypeDto
            {
                Id = src.Id,
                Name = src.Name,
                NepaliName = src.NepaliName,
                EntryDate = src.EntryDate,
                Schedule = src.Schedule
            }))
            .ForMember(dest => dest.AccountType, opt => opt.MapFrom(src => src.AccountType));

            // END

            // START: Group Details
            CreateMap<CreateGroupTypeDetailsDto, GroupTypeDetails>()
            ;
            CreateMap<GroupTypeDetailsDto, GroupTypeDetails>().ReverseMap();

            CreateMap<GroupTypeDetails, GroupTypeDetailsMappingDetailsDto>()
           .ForMember(dest => dest.AccountType, opt => opt.MapFrom(src => new AccountTypeDto
           {
               Id = src.GroupType.AccountTypeId,
               Name = src.GroupType.AccountType.Name
           }))
           .ForMember(dest => dest.GroupType, opt => opt.MapFrom(src => new GroupTypeDto
           {
               Id = src.GroupTypeId,
               Name = src.GroupType.Name,
               NepaliName = src.GroupType.NepaliName,
               EntryDate = src.GroupType.EntryDate,
               Schedule = src.GroupType.Schedule
           }))
           .ForMember(dest => dest.GroupTypeDetails, opt => opt.MapFrom(src => new GroupTypeDetailsDto
           {
               Id = src.Id,
               Name = src.Name,
               NepaliName = src.NepaliName,
               BankBranch = src.BankBranch,
               AccountNumber = src.AccountNumber,
               BankType = src.BankType,
               InterestRate = src.InterestRate,
               Branch = src.Branch
           }));

            // END

            // START: Ledger
            CreateMap<LedgerDto, Ledger>().ReverseMap();
            CreateMap<CreateLedgerDto, Ledger>();
            CreateMap<GroupTypeAndLedgerMap, LedgerDetailsDto>()
            .ForMember(dest => dest.AccountType, opt => opt.MapFrom(src => new AccountTypeDto
            {
                Id = src.GroupType.AccountTypeId,
                Name = src.GroupType.AccountType.Name
            }))
            .ForMember(dest => dest.GroupType, opt => opt.MapFrom(src => new GroupTypeDto
            {
                Id = src.GroupTypeId,
                Name = src.GroupType.Name,
                NepaliName = src.GroupType.NepaliName,
                EntryDate = src.GroupType.EntryDate,
                Schedule = src.GroupType.Schedule
            }))
            .ForMember(dest => dest.Ledger, opt => opt.MapFrom(src => new LedgerDto
            {
                Id = src.LedgerId,
                Name = src.Ledger.Name,
                NepaliName = src.Ledger.NepaliName,
                EntryDate = src.Ledger.EntryDate,
                HisabNumber = src.Ledger.HisabNumber,
                IsSubLedgerActive = src.Ledger.IsSubLedgerActive,
                DepreciationRate = src.Ledger.DepreciationRate
            }));

            // END

            // START: Subledger
            CreateMap<GroupSubLedger, SubLedgerDetailsDto>()
            .ForMember(dest => dest.AccountType, opt => opt.MapFrom(src => new AccountTypeDto
            {
                Id = src.GroupType.AccountTypeId,
                Name = src.GroupType.AccountType.Name
            }))
            .ForMember(dest => dest.SubLedger, opt => opt.MapFrom(src => new SubLedgerDto
            {
                Id = src.SubLedger.Id,
                Name = src.SubLedger.Name,
                Description = src.SubLedger.Description
            }))
            .ForMember(dest => dest.GroupType, opt => opt.MapFrom(src => new GroupTypeDto
            {
                Id = src.GroupType.Id,
                Name = src.GroupType.Name,
                NepaliName = src.GroupType.NepaliName,
                EntryDate = src.GroupType.EntryDate,
                Schedule = src.GroupType.Schedule
            }))
            .ForMember(dest => dest.Ledger, opt => opt.MapFrom(src => new LedgerDto
            {
                Id = src.SubLedger.LedgerId,
                Name = src.SubLedger.Ledger.Name,
                NepaliName = src.SubLedger.Ledger.NepaliName,
                EntryDate = src.SubLedger.Ledger.EntryDate,
                HisabNumber = src.SubLedger.Ledger.HisabNumber,
                IsSubLedgerActive = src.SubLedger.Ledger.IsSubLedgerActive,
                DepreciationRate = src.SubLedger.Ledger.DepreciationRate
            }));

            CreateMap<CreateSubLedgerDto, SubLedger>();
            // END


        }
    }
}