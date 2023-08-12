using System.Linq.Expressions;
using MicroFinance.Models.DepositSetup;
using MicroFinance.Models.Wrapper;

namespace MicroFinance.Repository.DepositSetup
{
    public interface IDepositSchemeRepository
    {
        Task<int> CreateDepositScheme(DepositScheme depositScheme, List<string> subLedgerNamesForDepositScheme);
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
        Task<int> UpdateDepositAccount(DepositAccount updateDepositAccount);
        Task<List<DepositAccountWrapper>> GetAllDepositAccountsWrapper(Expression<Func<DepositAccount, bool>> expression);
        Task<DepositAccountWrapper> GetDepositAccountWrapper(Expression<Func<DepositAccount, bool>> expression);
        // Task<DepositAccountWrapper> GetDepositAccountByAccountNumber(string accountNumber);
        // Task<List<DepositAccountWrapper>> GetNonClosedDepositAccountByDepositScheme(int depositSchemeId);
        //Task<DepositAccount> GetDepositAccountByDepositSchemeIdAndClientId(int depositSchemeId, int clientId);
        Task<DepositAccount> GetDepositAccount(Expression<Func<DepositAccount, bool>> expression);
        //Task<DepositAccount> GetNonClosedDepositAccountById(int id);

        // // Flexible Interest Rate

        // Task<int> CreateFlexibleInterestRate(FlexibleInterestRate flexibleInterestRate);
        // Task<int> UpdateInterestRateAccordingToFlexibleInterestRate(FlexibleInterestRate flexibleInterestRate);
        // Task<int> IncrementOrDecrementOfInterestRate(UpdateInterestRateByDepositSchemeDto updateInterestRateByDepositSchemeDto);
        // Task<int> ChangeInterestRateAccordingToPastInterestRate(ChangeInterestRateByDepositSchemeDto changeInterestRateByDepositSchemeDto);

    }
}