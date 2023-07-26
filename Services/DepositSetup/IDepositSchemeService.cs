using MicroFinance.Dtos;
using MicroFinance.Dtos.DepositSetup;
using MicroFinance.Dtos.DepositSetup.Account;
using MicroFinance.Models.Wrapper;

namespace MicroFinance.Services.DepositSetup
{
    public interface IDepositSchemeService
    {
        // Deposit Scheme
        Task<ResponseDto> CreateDepositSchemeService(CreateDepositSchemeDto createDepositScheme, TokenDto decodedToken);
        Task<ResponseDto> UpdateDepositSchemeService(UpdateDepositSchemeDto updateDepositScheme, TokenDto decodedToken);
        Task<DepositSchemeDto> GetDepositSchemeByIdService(int id);
        Task<List<DepositSchemeDto>> GetAllDepositSchemeService();
        

        // // Deposit Account
        Task<ResponseDto> CreateDepositAccountService(CreateDepositAccountDto createDepositAccountDto, TokenDto decodedToken);
        Task<List<DepositAccountWrapperDto>> GetAllNonClosedDepositAccountService(TokenDto decodedToken);
        Task<DepositAccountWrapperDto> GetNonClosedDepositAccountByIdService(int depositAccountId, TokenDto decodedToken);
        Task<ResponseDto> UpdateNonClosedDepositAccountService(UpdateDepositAccountDto updateDepositAccountDto, TokenDto decodedToken);
        Task<DepositAccountWrapperDto> GetNonClosedDepositAccountByAccountNumberService(string accountNumber, TokenDto decodedToken);
        Task<List<DepositAccountWrapperDto>> GetNonClosedDepositAccountByDepositSchemeService(int depositSchemeId, TokenDto decodedToken);
        // Task<AccountNumberDto> GetUniqueAccountNumberService(int depositSchemeId); 
        // Task<DepositAccountDto> GetDepositAccountByIdService(int id);
        // Task<DepositAccountDto> GetDepositAccountByAccountNumberService(string accountNumber);
        // Task<List<DepositAccountDto>> GetAllDepositAccountService();
        // Task<List<DepositAccountDto>> GetAllDepositAccountByDepositSchemeService(int depositSchemeId);

        // // Flexible Interest Rate

        // Task<ResponseDto> UpdateInterestRateAccordingToFlexibleInterestRateService(FlexibleInterestRateSetupDto flexibleInterestRateSetupDto);
        // Task<ResponseDto> IncrementOrDecrementOfInterestRateService(UpdateInterestRateByDepositSchemeDto updateInterestRateByDepositSchemeDto);
        // Task<ResponseDto> ChangeInterestRateAccordingToPastInterestRateService(ChangeInterestRateByDepositSchemeDto changeInterestRateByDepositSchemeDto);
        
    }
}