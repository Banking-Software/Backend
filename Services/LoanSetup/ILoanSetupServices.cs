using MicroFinance.Dtos;
using MicroFinance.Dtos.LoanSetup;

namespace MicroFinance.Services.LoanSetup
{
    public interface ILoanSetupServices
    {
        Task<ResponseDto> CreateLoanSchemeService(CreateLoanSchemeDto createLoanScheme, TokenDto decodedToken);
        Task<ResponseDto> CreateLoanAccountService(CreateLoanAccountDto createLoanAccount, TokenDto decodedToken);

        Task<List<LoanSchemeDto>> GetLoanSchemeService(int? loanSchemeId);
        Task<List<LoanAccountDto>> GetLoanAccountService(int? loanAccountId);
        Task<LoanScheduleDtos> GenerateScheduleService(GenerateLoanScheduleDto generateLoanSchedule);

    }
}