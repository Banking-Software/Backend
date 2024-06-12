using System.Linq.Expressions;
using MicroFinance.Dtos.LoanSetup;
using MicroFinance.Models.LoanSetup;

namespace MicroFinance.Repository.LoanSetup
{
    public interface ILoanSetupRepository
    {
        Task<int> CreateLoanScheme(LoanScheme loanScheme, string assetsAccountLedgerName, string interestAccountLedgerName);
        Task<List<LoanScheme>> GetSchemes(Expression<Func<LoanScheme, bool>> expression);
        Task<List<LoanAccount>> GetLoanAccounts(int? loanAccountId);
        Task<bool> ValidateAliasCode(string aliasCode);
        Task<int> CreateLoanAccount(LoanAccount loanAccount);
        Task<LoanScheduleDtos> GenerateLoanSchedule(GenerateLoanScheduleDto generateLoanSchedule);

    }
}