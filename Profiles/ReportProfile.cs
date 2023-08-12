
using AutoMapper;
using MicroFinance.Dtos.Reports;
using MicroFinance.Models.Wrapper.Reports;

namespace MicroFinance.Profiles;

public class ReportProfile : Profile
{
    public ReportProfile()
    {
        CreateMap<DepositAccountTransactionReport, DepositAccountTransactionReportDto>();
    }
}