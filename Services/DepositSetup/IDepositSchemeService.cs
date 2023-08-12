using System.Linq.Expressions;
using MicroFinance.Dtos;
using MicroFinance.Dtos.DepositSetup;
using MicroFinance.Dtos.DepositSetup.Account;
using MicroFinance.Enums;
using MicroFinance.Models.DepositSetup;
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
        Task<MatureDateDto> GenerateMatureDateOfDepositAccountService(GenerateMatureDateDto generateMatureDateDto);
        Task<DateTime> GenerateNextInterestPostingDate(DateTime englishCurrentDate, PostingSchemeEnum postingScheme, DateTime englishMatureDate);
        Task<List<DepositAccountWrapperDto>> GetAllDepositAccountWrapperService(TokenDto decodedToken);
        Task<DepositAccountWrapperDto> GetDepositAccountWrapperByIdService(int? depositAccountId, Expression<Func<DepositAccount, bool>>? expression, TokenDto decodedToken);
        Task<ResponseDto> UpdateNonClosedDepositAccountService(UpdateDepositAccountDto updateDepositAccountDto, TokenDto decodedToken);
        Task<DepositAccountWrapperDto> GetDepositAccountWrapperByAccountNumberService(string? accountNumber, Expression<Func<DepositAccount, bool>>? expression, TokenDto decodedToken);
        Task<List<DepositAccountWrapperDto>> GetDepositAccountWrapperByDepositSchemeService(int depositSchemeId, TokenDto decodedToken);
        Task<DepositAccountDto> GetDepositAccount(Expression<Func<DepositAccount, bool>> expression);
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