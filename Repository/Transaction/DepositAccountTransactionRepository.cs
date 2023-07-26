using AutoMapper;
using MicroFinance.DBContext;
using MicroFinance.Enums.Transaction;
using MicroFinance.Models.AccountSetup;
using MicroFinance.Models.Transactions;
using MicroFinance.Models.Wrapper.TrasactionWrapper;
using Microsoft.EntityFrameworkCore;



namespace MicroFinance.Repository.Transaction
{
    public class DepositAccountTransactionRepository : IDepositAccountTransactionRepository
    {
        private readonly ILogger<DepositAccountTransactionRepository> _logger;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _transactionDbContext;

        public DepositAccountTransactionRepository
        (
            ILogger<DepositAccountTransactionRepository> logger, 
            ApplicationDbContext transactionDbContext,
            IMapper mapper
        )
        {
            _logger = logger;
            _mapper = mapper;
            _transactionDbContext = transactionDbContext;
        }

        private async Task<BaseTransaction> GenerateVoucherNumber(BaseTransaction baseTransaction)
        {
            var existingBaseTransaction = await _transactionDbContext.Transactions.FindAsync(baseTransaction.Id);
            _transactionDbContext.Entry(existingBaseTransaction).State = EntityState.Detached;
            baseTransaction.VoucherNumber = "080/081"+"VCH"+baseTransaction.Id+baseTransaction.BranchCode;
            _transactionDbContext.Transactions.Attach(baseTransaction);
            _transactionDbContext.Entry(baseTransaction).State = EntityState.Modified;
            var voucherStatus = await _transactionDbContext.SaveChangesAsync();
            if(voucherStatus<1) throw new Exception("Failed to Create Voucher Number");
            return baseTransaction;

        }
        private async Task<BaseTransaction> MakeBaseTransaction(MakeDepositWrapper depositWrapper)
        {
            BaseTransaction baseTransaction = _mapper.Map<BaseTransaction>(depositWrapper);
            baseTransaction.Remarks=$"Deposit Transaction on {depositWrapper.AccountNumber}";
            await _transactionDbContext.Transactions.AddAsync(baseTransaction);
            var transactionAddStatus = await _transactionDbContext.SaveChangesAsync();
            if (transactionAddStatus < 1) throw new Exception("Failed to create Transaction");
            return await GenerateVoucherNumber(baseTransaction);
        }
        private async Task<DepositAccountTransaction> MakeDepositAccountTransaction(MakeDepositWrapper depositWrapper, BaseTransaction baseTransaction)
        {
            var depositAccount = await _transactionDbContext.DepositAccounts.FindAsync(depositWrapper.DepositAccountId);
            depositAccount.PrincipalAmount +=depositWrapper.TransactionAmount;
            DepositAccountTransaction depositAccountTransaction = new DepositAccountTransaction()
            {
                Transaction=baseTransaction,
                DepositAccount = depositAccount,
                TransactionType = depositWrapper.TransactionType,
                PaymentType = depositWrapper.PaymentType,
                CollectedByEmployeeId = depositWrapper.CollectedByEmployeeId,
                Narration = depositWrapper.Narration,
                Source = depositWrapper.Source,
                Remarks =$"{baseTransaction.TransactionAmount} is deposited on {depositAccount.AccountNumber}",
                BalanceAfterTransaction=depositAccount.PrincipalAmount
            };
            if(depositWrapper.PaymentType==PaymentTypeEnum.Bank)
            {
                var bankDetail  = await _transactionDbContext.BankSetups.FindAsync(depositWrapper.BankDetailId);
                depositAccountTransaction.BankDetail = bankDetail;
                depositAccountTransaction.BankChequeNumber = depositWrapper.BankChequeNumber;
            }
            await _transactionDbContext.DepositAccountTransactions.AddAsync(depositAccountTransaction);
            // var depositTransactionStatus = await _transactionDbContext.SaveChangesAsync();
            // if(depositTransactionStatus<1) throw new Exception($"Deposit of amount {depositWrapper.TransactionAmount} failed for account {depositAccount.AccountNumber}");
            return depositAccountTransaction;
        }

        private async Task MakeSubLedgerTransaction(MakeDepositWrapper depositWrapper, BaseTransaction baseTransaction)
        {
            //var depositScheme = await _transactionDbContext.DepositSchemes.FindAsync(depositWrapper.DepositSchemeId);
            var depositSchemeDepositSubledger = await _transactionDbContext.SubLedgers.Include(sl=>sl.Ledger).Where(sl=>sl.Id==depositWrapper.DepositSchemeSubLedgerId).FirstOrDefaultAsync();
            depositSchemeDepositSubledger.CurrentBalance+=depositWrapper.TransactionAmount;
            depositSchemeDepositSubledger.Ledger.CurrentBalance+=depositWrapper.TransactionAmount;
            SubLedgerTransaction subLedgerTransaction = new SubLedgerTransaction()
            {
                Transaction=baseTransaction,
                SubLedger=depositSchemeDepositSubledger,
                TransactionType = depositWrapper.TransactionType,
                Remarks=$"Deposit of {depositWrapper.TransactionAmount} on {depositWrapper.AccountNumber}",
                BalanceAfterTransaction = depositSchemeDepositSubledger.CurrentBalance
            };
            await _transactionDbContext.SubLedgerTransactions.AddAsync(subLedgerTransaction);
            //var subLedgerTransactionStatus = await _transactionDbContext.SaveChangesAsync();
        }

        private async Task MakeLedgerTransaction(MakeDepositWrapper depositWrapper, BaseTransaction baseTransaction)
        {
            
            Ledger ledger = new Ledger();
            if(depositWrapper.PaymentType==PaymentTypeEnum.Cash)
            {
                ledger = await _transactionDbContext.Ledgers.Where(l=>l.LedgerCode==1).SingleOrDefaultAsync();
            }
            else
            {
                //var bankDetail = await _transactionDbContext.BankSetups.FindAsync(depositWrapper.BankDetailId);
                ledger = await _transactionDbContext.Ledgers.FindAsync(depositWrapper.BankLedgerId);
            }
            ledger.CurrentBalance+=baseTransaction.TransactionAmount;
            LedgerTransaction ledgerTransaction = new LedgerTransaction()
            {
                Ledger = ledger,
                Transaction = baseTransaction,
                TransactionType = TransactionTypeEnum.Debit,
                Remarks = $"Deposit Transaction of {baseTransaction.TransactionAmount} on {depositWrapper.AccountNumber}",
                BalanceAfterTransaction=ledger.CurrentBalance
            };
            await _transactionDbContext.LedgerTransactions.AddAsync(ledgerTransaction);
            //var ledgerTransactionStatus = await _transactionDbContext.SaveChangesAsync();
        }

        public async Task<string> MakeDeposit(MakeDepositWrapper depositWrapper)
        {
            _logger.LogInformation($"{DateTime.Now}: Depositing {depositWrapper.TransactionAmount} by {depositWrapper.CreatedBy} on Deposit Account Id: {depositWrapper.DepositAccountId}");
            using (var processTransaction = await _transactionDbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    BaseTransaction baseTransaction = await MakeBaseTransaction(depositWrapper);
                    DepositAccountTransaction depositAccountTransaction = await MakeDepositAccountTransaction(depositWrapper, baseTransaction);
                    await MakeSubLedgerTransaction(depositWrapper, baseTransaction);
                    await MakeLedgerTransaction(depositWrapper, baseTransaction);
                    int transactionStatus = await _transactionDbContext.SaveChangesAsync();
                    if(transactionStatus<1) throw new Exception("Unable to make Deposit Transaction");
                    await processTransaction.CommitAsync();
                    return baseTransaction.VoucherNumber;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"{DateTime.Now}: {ex.Message} {ex?.InnerException?.Message}");
                    await processTransaction.RollbackAsync();
                    throw new Exception(ex.Message);
                }

            }
        }

        public Task<string> MakeWithDrawal(DepositAccountTransaction withDrawalTransaction)
        {
            throw new NotImplementedException();
        }
    }
}