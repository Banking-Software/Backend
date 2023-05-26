using MicroFinance.Dtos.DepositSetup;
using MicroFinance.Models.DepositSetup;

namespace MicroFinance.Repository.DepositSetup
{
    public interface IDepositSchemeRepository
    {
        Task<int> CreateDepositScheme(DepositScheme depositScheme);
        Task<int> UpdateDepositScheme(UpdateDepositScheme depositScheme);
        Task<List<ResponseDepositScheme>> GetAllDepositScheme();
        Task<DepositScheme> GetDepositSchemeByName(string name);
        Task<DepositScheme> GetDepositScheme(int id);
        Task<List<ResponseDepositScheme>> GetDepositSchemeByPostingScheme(int id);
        Task<PostingScheme> GetPositingScheme(int id);

        // Deposit Account

        Task<int> CreateDepositAccount(DepositAccount depositAccount);
        Task<int> UpdateDepositAccount(UpdateDepositAccountDto updateDepositAccount, string modifiedBy);
        Task<List<DepositAccount>> GetDepositAccountListAsync();
        Task<DepositAccount> GetDepositAccountByAccountNumber(string accountNumber);
        Task<List<DepositAccount>> GetDepositAccountByDepositScheme(int depositSchemeId);
        Task<DepositAccount> GetDepositAccountById(int id);

        // Flexible Interest Rate

        Task<int> CreateFlexibleInterestRate(FlexibleInterestRate flexibleInterestRate);
        Task<int> UpdateInterestRateAccordingToFlexibleInterestRate(FlexibleInterestRate flexibleInterestRate);
        Task<int> IncrementOrDecrementOfInterestRate(UpdateInterestRateByDepositSchemeDto updateInterestRateByDepositSchemeDto);
        Task<int> ChangeInterestRateAccordingToPastInterestRate(ChangeInterestRateByDepositSchemeDto changeInterestRateByDepositSchemeDto);

    }
}