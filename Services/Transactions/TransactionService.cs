using MicroFinance.Dto.Transactions;
using MicroFinance.Dtos;
using MicroFinance.Enums.Transaction;
using MicroFinance.Exceptions;
using MicroFinance.Repository.Transaction;

namespace MicroFinance.Services.Transactions
{
    public class TransactionService : ITransactionService
    {
        private readonly IManualVoucherTransactionRepository _manualVoucherTransaction;
        private readonly IBaseTransactionRepository _transactionRepository;

        public TransactionService(IManualVoucherTransactionRepository manualVoucherTransaction, IBaseTransactionRepository transactionRepository)
        {
            _manualVoucherTransaction = manualVoucherTransaction;
            _transactionRepository = transactionRepository;
        }

        private async Task<bool> IsDebitCreditSumEqual(List<ManualVoucherDto> manualVouchers)
        {
            var transactionWiseSum = manualVouchers.GroupBy(manualVoucher => manualVoucher.TransactionType)
           .Select(mv => new
           {
               TransactionType = mv.Key,
               TransactionSum = mv.Sum(manualVoucher => manualVoucher.TransactionAmount)
           });
            decimal? creditSum = transactionWiseSum.Where(ts => ts.TransactionType == TransactionTypeEnum.Credit).SingleOrDefault()?.TransactionSum;
            decimal? debitSum = transactionWiseSum.Where(ts => ts.TransactionType == TransactionTypeEnum.Debit).SingleOrDefault()?.TransactionSum;
            if (creditSum == null || debitSum == null || creditSum != debitSum)
                return false;
            return true;
        }

        public async Task<ResponseDto> ManualVoucherTransactionService(List<ManualVoucherDto> manualVouchers, TokenDto decodedToken)
        {
            if(await IsDebitCreditSumEqual(manualVouchers))
            {
                await _transactionRepository.ManualVoucherTransaction(manualVouchers, decodedToken.BranchCode);
                return new ResponseDto()
                {
                    Message="Successfully updated provided ledger or subledger",
                    Status = true,
                    StatusCode = "200"
                };
            }
            throw new BadRequestExceptionHandler("DR and CR amount should be equal");
        }
    }
}