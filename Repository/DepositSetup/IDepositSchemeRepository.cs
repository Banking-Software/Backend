using MicroFinance.Models.DepositSetup;
using MicroFinance.Models.Wrapper;

namespace MicroFinance.Repository.DepositSetup
{
    public interface IDepositSchemeRepository
    {
        Task<int> CreateDepositScheme(DepositScheme depositScheme);
        Task<int> UpdateDepositScheme(DepositScheme updateDepositScheme);
        Task<List<DepositScheme>> GetAllDepositScheme();
        Task<DepositScheme> GetDepositSchemeByName(string name);
        Task<DepositScheme> GetDepositSchemeById(int id);
        Task<DepositScheme> GetDepositSchemeBySymbol(string symbol);
        // Task<List<ResponseDepositScheme>> GetDepositSchemeByPostingScheme(int id);
        // Task<PostingScheme> GetPositingScheme(int id);

        // // Deposit Account

        Task<int> CreateDepositAccount(DepositAccount depositAccount);
        Task<int> CreateJointAccount(List<JointAccount> jointAccounts, DepositAccount depositAccount);
        // Task<int> UpdateDepositAccount(UpdateDepositAccountDto updateDepositAccount, string modifiedBy);
        Task<List<DepositAccountWrapper>> GetAllNonClosedDepositAccounts();
        Task<DepositAccountWrapper> GetNonCloseDepositAccountById(int id);
        // Task<DepositAccount> GetDepositAccountByAccountNumber(string accountNumber);
        // Task<List<DepositAccount>> GetDepositAccountByDepositScheme(int depositSchemeId);
        Task<DepositAccount> GetDepositAccountByDepositSchemeIdAndClientId(int depositSchemeId, int clientId);
        Task<DepositAccount> GetDepositAccountById(int id);

        // // Flexible Interest Rate

        // Task<int> CreateFlexibleInterestRate(FlexibleInterestRate flexibleInterestRate);
        // Task<int> UpdateInterestRateAccordingToFlexibleInterestRate(FlexibleInterestRate flexibleInterestRate);
        // Task<int> IncrementOrDecrementOfInterestRate(UpdateInterestRateByDepositSchemeDto updateInterestRateByDepositSchemeDto);
        // Task<int> ChangeInterestRateAccordingToPastInterestRate(ChangeInterestRateByDepositSchemeDto changeInterestRateByDepositSchemeDto);

    }
}