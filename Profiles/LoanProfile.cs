using AutoMapper;
using MicroFinance.Dtos.LoanSetup;
using MicroFinance.Models.LoanSetup;

namespace MicroFinance.Profiles
{
    public class LoanProfile : Profile
    {
        public LoanProfile()
        {
            CreateMap<CreateLoanSchemeDto, LoanScheme>();
            CreateMap<CreateLoanAccountDto, LoanAccount>();

            CreateMap<LoanScheme, LoanSchemeDto>()
            .ForMember(dest=>dest.AssetsAccountLedgerName, opt=>opt.MapFrom(src=>src.AssetsAccountLedger.Name))
            .ForMember(dest=>dest.InterestAccountLedgerName, opt=>opt.MapFrom(src=>src.InterestAccountLedger.Name));

            CreateMap<LoanAccount, LoanAccountDto>()
            .ForMember(dest=>dest.LoanScheme, opt=>opt.MapFrom(src=>src.LoanScheme.Name))
            .ForMember(dest=>dest.clientName, opt=>opt.MapFrom(src=>$"{src.Client.ClientFirstName} {src.Client.ClientLastName}"))
            .ForMember(dest=>dest.UploadedDocument, opt=>opt.MapFrom(src=>src.UploadedDocument!=null?Convert.ToBase64String(src.UploadedDocument):null));
        }
    }
}