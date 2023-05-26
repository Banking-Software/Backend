using MicroFinance.Dtos;
using MicroFinance.Dtos.DepositSetup;

namespace MicroFinance.Services.DepositSetup
{
    public interface IDepositSchemeService
    {
        // Deposit Scheme
        Task<ResponseDto> CreateDepositSchemeService(CreateDepositSchemeDto createDepositScheme, string currentUser);
        Task<ResponseDto> UpdateDepositSchemeService(UpdateDepositSchemeDto updateDepositScheme, string currentUser);
        Task<DepositSchemeDto> GetDepositSchemeService(int id);
        Task<List<DepositSchemeDto>> GetAllDepositSchemeService();

        // Deposit Account
        Task<ResponseDto> CreateDepositAccountService(CreateDepositAccountDto createDepositAccountDto, string createdBy);
        Task<ResponseDto> UpdateDepositAccountService(UpdateDepositAccountDto updateDepositAccountDto, string modifiedBy);
        Task<AccountNumberDto> GetUniqueAccountNumberService(int depositSchemeId); 
        Task<DepositAccountDto> GetDepositAccountByIdService(int id);
        Task<DepositAccountDto> GetDepositAccountByAccountNumberService(string accountNumber);
        Task<List<DepositAccountDto>> GetAllDepositAccountService();
        Task<List<DepositAccountDto>> GetAllDepositAccountByDepositSchemeService(int depositSchemeId);

        // Flexible Interest Rate

        Task<ResponseDto> UpdateInterestRateAccordingToFlexibleInterestRateService(FlexibleInterestRateSetupDto flexibleInterestRateSetupDto);
        Task<ResponseDto> IncrementOrDecrementOfInterestRateService(UpdateInterestRateByDepositSchemeDto updateInterestRateByDepositSchemeDto);
        Task<ResponseDto> ChangeInterestRateAccordingToPastInterestRateService(ChangeInterestRateByDepositSchemeDto changeInterestRateByDepositSchemeDto);
        
    }
}