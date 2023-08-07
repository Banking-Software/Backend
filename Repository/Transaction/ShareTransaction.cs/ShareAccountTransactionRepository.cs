using AutoMapper;
using MicroFinance.DBContext;
using MicroFinance.Enums;
using MicroFinance.Enums.Transaction;
using MicroFinance.Enums.Transaction.ShareTransaction;
using MicroFinance.Exceptions;
using MicroFinance.Helper;
using MicroFinance.Models.AccountSetup;
using MicroFinance.Models.DepositSetup;
using MicroFinance.Models.Share;
using MicroFinance.Models.Transactions;
using MicroFinance.Models.Wrapper.TrasactionWrapper;
using Microsoft.EntityFrameworkCore;

namespace MicroFinance.Repository.Transaction
{
    public class ShareAccountTransactionRepository : IShareAccountTransactionRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ShareAccountTransactionRepository> _logger;
        private readonly IMapper _mapper;
        private readonly IDepositAccountTransactionRepository _depositAccountTransactionRepository;

        public ShareAccountTransactionRepository
        (
            ApplicationDbContext dbContext,
            ILogger<ShareAccountTransactionRepository> logger,
            IMapper mapper,
            IDepositAccountTransactionRepository depositAccountTransactionRepository
        )
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
            _depositAccountTransactionRepository=depositAccountTransactionRepository;
        }
        public async Task<string> LockAndMakeShareTransaction(ShareAccountTransactionWrapper shareAccountTransactionWrapper)
        {
            _dbContext.ChangeTracker.Clear();
            using (var processTransaction = await _dbContext.Database.BeginTransactionAsync())
            {
                // Lock the transaction

                SemaphoreSlim shareAccountLock = LockManager.Instance.GetShareAccountLock(shareAccountTransactionWrapper.ShareAccountId);
                await shareAccountLock.WaitAsync();
                SemaphoreSlim shareKittaLock = LockManager.Instance.GetShareAccountLock(shareAccountTransactionWrapper.ShareKittaId);
                await shareKittaLock.WaitAsync();

                SemaphoreSlim transferAccountLock = null;
                SemaphoreSlim transferAccountSubledgerLock = null;
                SemaphoreSlim transferAccountLedgerLock = null;
                SemaphoreSlim cashLedgerLock = null;
                SemaphoreSlim bankLedgerLock = null;
                SemaphoreSlim paymentAccountLock = null;
                SemaphoreSlim paymentAccountSubledgerLock = null;
                SemaphoreSlim paymentAccountLedgerLock = null;

                if (shareAccountTransactionWrapper.ShareTransactionType == ShareTransactionTypeEnum.Transfer)
                {
                    transferAccountLock = LockManager.Instance.GetAccountLock((int)shareAccountTransactionWrapper.TransferToDepositAccountId);
                    await transferAccountLock.WaitAsync();
                    transferAccountSubledgerLock = LockManager.Instance.GetSubLedgerLock((int)shareAccountTransactionWrapper.TransferToDepositSchemeSubLedgerId);
                    await transferAccountSubledgerLock.WaitAsync();
                    transferAccountLedgerLock = LockManager.Instance.GetLedgerLock((int)shareAccountTransactionWrapper.TransferToDepositSchemeLedgerId);
                    await transferAccountLedgerLock.WaitAsync();
                }
                else
                {
                    if (shareAccountTransactionWrapper.PaymentType == PaymentTypeEnum.Cash)
                    {
                        cashLedgerLock = LockManager.Instance.GetLedgerLock(1);
                        await cashLedgerLock.WaitAsync();
                    }
                    else if (shareAccountTransactionWrapper.PaymentType == PaymentTypeEnum.Bank)
                    {
                        bankLedgerLock = LockManager.Instance.GetLedgerLock((int)shareAccountTransactionWrapper.BankLedgerId);
                        await bankLedgerLock.WaitAsync();
                    }
                    else
                    {
                        // Account
                        paymentAccountLock = LockManager.Instance.GetAccountLock((int)shareAccountTransactionWrapper.PaymentDepositAccountId);
                        await paymentAccountLock.WaitAsync();
                        paymentAccountSubledgerLock = LockManager.Instance.GetSubLedgerLock((int)shareAccountTransactionWrapper.PaymentDepositSchemeSubLedgerId);
                        await paymentAccountSubledgerLock.WaitAsync();
                        paymentAccountLedgerLock = LockManager.Instance.GetLedgerLock((int)shareAccountTransactionWrapper.PaymentDepositSchemeLedgerId);
                        await paymentAccountLedgerLock.WaitAsync();
                    }
                }
                try
                {
                    var baseTransaction = await MakeTransaction(shareAccountTransactionWrapper);
                    var transactionStatus = await _dbContext.SaveChangesAsync();
                    if(transactionStatus<1) throw new Exception("Not able to Make the transaction");
                    await processTransaction.CommitAsync();
                    return baseTransaction.VoucherNumber;
                }
                catch (Exception ex)
                {
                    await processTransaction.RollbackAsync();
                    _logger.LogError($"{DateTime.Now}: {ex.Message} {ex.InnerException}");
                    throw new Exception(ex.Message);
                }
                finally
                {
                    shareAccountLock.Release();
                    shareKittaLock.Release();
                    transferAccountLock?.Release();
                    transferAccountSubledgerLock?.Release();
                    transferAccountLedgerLock?.Release();
                    cashLedgerLock?.Release();
                    bankLedgerLock?.Release();
                    paymentAccountLock?.Release();
                    paymentAccountSubledgerLock?.Release();
                    paymentAccountLedgerLock?.Release();
                }
            }

        }


        private async Task<BaseTransaction> MakeTransaction(ShareAccountTransactionWrapper shareAccountTransactionWrapper)
        {
            bool isDepositInShareAccount = false;
            TransactionTypeEnum ledgerTransactionType = TransactionTypeEnum.Credit;
            if(shareAccountTransactionWrapper.ShareTransactionType == ShareTransactionTypeEnum.Issue)
            {
                isDepositInShareAccount = true;
                ledgerTransactionType = TransactionTypeEnum.Debit;
            }
            var baseTransaction = await MakeBaseTransaction(shareAccountTransactionWrapper);
            DepositAccount depositAccountForTransferOrPayment = null;
            if(shareAccountTransactionWrapper.ShareTransactionType == ShareTransactionTypeEnum.Transfer || shareAccountTransactionWrapper.PaymentType==PaymentTypeEnum.Account)
                depositAccountForTransferOrPayment = await BaseTransactionOnDepositAccount(baseTransaction, shareAccountTransactionWrapper);
            await BaseTransactionOnShareAccount(baseTransaction, shareAccountTransactionWrapper, depositAccountForTransferOrPayment);
            // When ShareTransaction = Transfer then no Payment methods are allowed
            if(shareAccountTransactionWrapper.ShareTransactionType!=ShareTransactionTypeEnum.Transfer)
                await _depositAccountTransactionRepository.BaseTransactionOnLedger(baseTransaction, shareAccountTransactionWrapper.PaymentType, ledgerTransactionType, isDepositInShareAccount);
            await ModifyShareKittaNumber(shareAccountTransactionWrapper.ShareAccountId, shareAccountTransactionWrapper.TransactionAmount);
            return baseTransaction;
        }

        

        private async Task<DepositAccount> BaseTransactionOnDepositAccount(BaseTransaction baseTransaction, ShareAccountTransactionWrapper shareAccountTransactionWrapper)
        {
            var depositAccountTransactionWrapper = await GetDepositAccountTransactionWrapper(baseTransaction, shareAccountTransactionWrapper);
            var depositAccount = await _depositAccountTransactionRepository.BaseTransactionOnDepositAccount(depositAccountTransactionWrapper, baseTransaction);
            // var depositAccount = await TransactionOnDepositAccount(depositAccountTransactionWrapper);
            // await CreateDepositAccountTransactionEntry(depositAccountTransactionWrapper, baseTransaction, depositAccount);
            // await BaseTransactionOnSubLedger(baseTransaction, depositAccountTransactionWrapper);
            return depositAccount;
        }
        private async Task<DepositAccountTransactionWrapper> GetDepositAccountTransactionWrapper(BaseTransaction baseTransaction, ShareAccountTransactionWrapper shareAccountTransactionWrapper)
        {
            bool isDeposit = shareAccountTransactionWrapper.ShareTransactionType == ShareTransactionTypeEnum.Refund || shareAccountTransactionWrapper.ShareTransactionType == ShareTransactionTypeEnum.Transfer;
            bool isTransfer = shareAccountTransactionWrapper.ShareTransactionType == ShareTransactionTypeEnum.Transfer;
            DepositAccountTransactionWrapper depositAccountTransactionWrapper = new()
            {
                TransactionAmount = shareAccountTransactionWrapper.TransactionAmount,
                AmountInWords = shareAccountTransactionWrapper.AmountInWords,
                TransactionType = isDeposit ? TransactionTypeEnum.Credit : TransactionTypeEnum.Debit,
                Narration = shareAccountTransactionWrapper.Narration,
                WithDrawalType = !isDeposit ? WithDrawalTypeEnum.ByShare : null,
                Source="FROM SHARE TRANSACTION"
            };
            int depositAccountId;
            int depositSchemeId;
            int depositSchemeSubLedgerId;
            if(isTransfer)
            {
                depositAccountId = (int)shareAccountTransactionWrapper.TransferToDepositAccountId;
                depositSchemeId = (int) shareAccountTransactionWrapper.TransferToDepositSchemeId;
                depositSchemeSubLedgerId = (int) shareAccountTransactionWrapper.TransferToDepositSchemeSubLedgerId;
            }
            else
            {
                depositAccountId = (int)shareAccountTransactionWrapper.PaymentDepositAccountId;
                depositSchemeId = (int) shareAccountTransactionWrapper.PaymentDepositSchemeId;
                depositSchemeSubLedgerId = (int) shareAccountTransactionWrapper.PaymentDepositSchemeSubLedgerId; 
            }
            depositAccountTransactionWrapper.DepositAccountId = depositAccountId;
            depositAccountTransactionWrapper.DepositSchemeSubLedgerId = depositSchemeSubLedgerId;
            depositAccountTransactionWrapper.DepositSchemeId = depositSchemeId;
            return depositAccountTransactionWrapper;
        }
        // private async Task<DepositAccount> TransactionOnDepositAccount(DepositAccountTransactionWrapper depositAccountTransactionWrapper)
        // {
        //     DepositAccount depositAccount = await _dbContext.DepositAccounts.FindAsync(depositAccountTransactionWrapper.DepositAccountId);
        //     if (depositAccount != null)
        //     {
        //         depositAccount.PrincipalAmount = depositAccountTransactionWrapper.TransactionType==TransactionTypeEnum.Credit
        //         ?
        //         depositAccount.PrincipalAmount + depositAccountTransactionWrapper.TransactionAmount
        //         :
        //         depositAccount.PrincipalAmount - depositAccountTransactionWrapper.TransactionAmount;
        //         if (depositAccount.PrincipalAmount < 0)
        //         {
        //             throw new BadRequestExceptionHandler($"Negative Transaction: current balance of {depositAccount.AccountNumber} will go to negative");
        //         }
        //         return depositAccount;
        //     }
        //     throw new Exception("No Deposit Account found");
        // }
        // private async Task CreateDepositAccountTransactionEntry(DepositAccountTransactionWrapper depositAccountTransactionWrapper, BaseTransaction baseTransaction, DepositAccount depositAccount)
        // {
        //     DepositAccountTransaction depositAccountTransaction = new DepositAccountTransaction()
        //     {
        //         Transaction = baseTransaction,
        //         DepositAccount = depositAccount,
        //         TransactionType = depositAccountTransactionWrapper.TransactionType,
        //         WithDrawalType = depositAccountTransactionWrapper.WithDrawalType,
        //         WithDrawalChequeNumber = depositAccountTransactionWrapper.WithDrawalChequeNumber,
        //         CollectedByEmployeeId = depositAccountTransactionWrapper.CollectedByEmployeeId,
        //         Narration = depositAccountTransactionWrapper.Narration,
        //         Source = depositAccountTransactionWrapper.Source,
        //         BalanceAfterTransaction = depositAccount.PrincipalAmount
        //     };
        //     await _dbContext.DepositAccountTransactions.AddAsync(depositAccountTransaction);
        // }

        // private async Task BaseTransactionOnSubLedger(BaseTransaction baseTransaction, DepositAccountTransactionWrapper depositAccountTransactionWrapper)
        // {
        //     var depositSchemeSubLedger = await TransactionOnSubLedger(baseTransaction, depositAccountTransactionWrapper);
        //     await CreateDepositSchemeSubLedgerTransactionEntry(baseTransaction, depositSchemeSubLedger, depositAccountTransactionWrapper);
        // }

        // private async Task<SubLedger> TransactionOnSubLedger(BaseTransaction baseTransaction, DepositAccountTransactionWrapper depositAccountTransactionWrapper)
        // {
        //     SubLedger depositSchemeSubledger = await _dbContext.SubLedgers
        //     .Include(sl=>sl.Ledger)
        //     .Where(sl=>sl.Id==depositAccountTransactionWrapper.DepositSchemeSubLedgerId)
        //     .SingleOrDefaultAsync();
        //     if(depositSchemeSubledger!=null)
        //     {
        //         if(depositAccountTransactionWrapper.TransactionType==TransactionTypeEnum.Credit)
        //         {
        //             depositSchemeSubledger.CurrentBalance+=depositAccountTransactionWrapper.TransactionAmount;
        //             depositSchemeSubledger.Ledger.CurrentBalance+=depositAccountTransactionWrapper.TransactionAmount;
        //         }
        //         else
        //         {
        //             depositSchemeSubledger.CurrentBalance-=depositAccountTransactionWrapper.TransactionAmount;
        //             depositSchemeSubledger.Ledger.CurrentBalance-=depositAccountTransactionWrapper.TransactionAmount;
        //         }
        //         if(depositSchemeSubledger.CurrentBalance<0 || depositSchemeSubledger.Ledger.CurrentBalance<0)
        //         {
        //             throw new Exception("(Negative Balance: Current Transaction will lead SubLedger and Ledger to negative balance)");
        //         }
        //         return depositSchemeSubledger;
        //     }
        //     throw new Exception("No subLedger found for given deposit account");
        // }
        // private async Task CreateDepositSchemeSubLedgerTransactionEntry(BaseTransaction baseTransaction, SubLedger depositSchemeSubLedger ,DepositAccountTransactionWrapper depositAccountTransactionWrapper)
        // {
        //     SubLedgerTransaction subLedgerTransaction = new()
        //     {
        //         Transaction = baseTransaction,
        //         SubLedger = depositSchemeSubLedger,
        //         TransactionType = depositAccountTransactionWrapper.TransactionType,
        //         Remarks = "Update on Subledger during share transaction",
        //         BalanceAfterTransaction = depositSchemeSubLedger.CurrentBalance
        //     };
        //     await _dbContext.SubLedgerTransactions.AddAsync(subLedgerTransaction);
        // }

        private async Task BaseTransactionOnShareAccount(BaseTransaction baseTransaction,ShareAccountTransactionWrapper shareAccountTransactionWrapper, DepositAccount paymentOrTransferDepositAccount)
        {
            var shareAccount = await TransactionOnShareAccount(shareAccountTransactionWrapper);
            await CreateShareTransactionEntry(shareAccountTransactionWrapper, baseTransaction, shareAccount, paymentOrTransferDepositAccount);
            //return shareAccount;
        }

        private async Task CreateShareTransactionEntry(ShareAccountTransactionWrapper shareAccountTransactionWrapper, BaseTransaction baseTransaction, ShareAccount shareAccount, DepositAccount paymentOrTransferDepositAccount)
        {
            ShareTransaction shareTransaction = new()
            {
                ShareTransactionType=shareAccountTransactionWrapper.ShareTransactionType,
                ShareCertificateNumber = shareAccountTransactionWrapper.ShareCertificateNumber,
                Narration = shareAccountTransactionWrapper.Narration,
                TransactionType = shareAccountTransactionWrapper.ShareTransactionType==ShareTransactionTypeEnum.Issue?TransactionTypeEnum.Credit:TransactionTypeEnum.Debit,
                Transaction = baseTransaction,
                ShareAccount = shareAccount,
                Remarks = $"Share is {shareAccountTransactionWrapper.ShareTransactionType}",
                BalanceAfterTransaction = shareAccount.CurrentShareBalance
            };
            if(shareAccountTransactionWrapper.ShareTransactionType == ShareTransactionTypeEnum.Transfer)
            {
                shareTransaction.TransferToAccount = paymentOrTransferDepositAccount;
                shareTransaction.Remarks =$"Share is Transfered to {paymentOrTransferDepositAccount.AccountNumber}";
            }
            else if(shareAccountTransactionWrapper.PaymentType == PaymentTypeEnum.Account)
            {
                shareTransaction.PaymentDepositAccount = paymentOrTransferDepositAccount;
                shareTransaction.Remarks =$"Share is {shareAccountTransactionWrapper.ShareTransactionType} through {paymentOrTransferDepositAccount.AccountNumber} account";
            }
            
            await _dbContext.ShareTransactions.AddAsync(shareTransaction);
        }

        private async Task<ShareAccount> TransactionOnShareAccount(ShareAccountTransactionWrapper shareAccountTransactionWrapper)
        {
            ShareAccount transactionShareAccount = await _dbContext.ShareAccounts.Where(sa=>sa.Id==shareAccountTransactionWrapper.ShareAccountId && sa.IsActive).SingleOrDefaultAsync();
            if (transactionShareAccount != null)
            {
                transactionShareAccount.CurrentShareBalance =
                shareAccountTransactionWrapper.ShareTransactionType == ShareTransactionTypeEnum.Issue
                ?
                transactionShareAccount.CurrentShareBalance + shareAccountTransactionWrapper.TransactionAmount
                :
                transactionShareAccount.CurrentShareBalance - shareAccountTransactionWrapper.TransactionAmount;

                if (transactionShareAccount.CurrentShareBalance < 0)
                {
                    _logger.LogError($"{DateTime.Now}: Try to make negative transaction on share account {transactionShareAccount.ClientId}");
                    throw new BadRequestExceptionHandler("Negative Transaction: Given Transaction make share balance less than 0.");
                }

                return transactionShareAccount;
            }
            throw new Exception("Share Account Not Found");
        }

        private async Task ModifyShareKittaNumber(int shareKittaId, decimal TransactionAmount)
        {
            var shareKitta = await _dbContext.ShareKittas.FindAsync(shareKittaId);
            if(shareKitta==null) throw new Exception("No any available kitta");
            shareKitta.CurrentKitta += TransactionAmount / shareKitta.PriceOfOneKitta ;
        }
        // private async Task BaseTransactionOnLedger(BaseTransaction baseTransaction, PaymentTypeEnum paymentType, TransactionTypeEnum ledgerTransactionType)
        // {
        //     Ledger paymentMethodLedger =  await TransactionOnLedger( baseTransaction,  paymentType, ledgerTransactionType);
        //     await CreateLedgerTransactionEntry(baseTransaction, paymentMethodLedger, ledgerTransactionType);
        // }
        // private async Task<Ledger> TransactionOnLedger(BaseTransaction baseTransaction, PaymentTypeEnum paymentType, TransactionTypeEnum ledgerTransactionType)
        // {
        //     Ledger ledger = paymentType==PaymentTypeEnum.Cash
        //     ? 
        //     await _dbContext.Ledgers.Where(l=>l.LedgerCode==1).SingleOrDefaultAsync() // In Cash of Cash Transaction
        //     :
        //     await _dbContext.Ledgers.Where(l=>l.Id == baseTransaction.BankDetail.LedgerId).SingleOrDefaultAsync();
            
        //     if(ledger!=null)
        //     {
        //         ledger.CurrentBalance = ledgerTransactionType==TransactionTypeEnum.Credit
        //         ?
        //         ledger.CurrentBalance+baseTransaction.TransactionAmount
        //         :
        //         ledger.CurrentBalance-baseTransaction.TransactionAmount;
        //         if(ledger.CurrentBalance<0)
        //         {
        //             throw new Exception("Negative Balance: Your payment method leads to negative balance in ledger");
        //         }
        //         return ledger;
        //     }
        //     throw new Exception("No Ledger Found that satisfy your payment method");
        // }

        // private async Task CreateLedgerTransactionEntry(BaseTransaction baseTransaction, Ledger paymentMethodLedger, TransactionTypeEnum ledgerTransactionType)
        // {
        //     LedgerTransaction ledgerTransaction = new()
        //     {
        //         Transaction = baseTransaction,
        //         Ledger = paymentMethodLedger,
        //         TransactionType = ledgerTransactionType,
        //         Remarks = $"{baseTransaction.TransactionAmount} is {ledgerTransactionType}",
        //         BalanceAfterTransaction = paymentMethodLedger.CurrentBalance
        //     };
        //     await _dbContext.LedgerTransactions.AddAsync(ledgerTransaction);
        // }
        private async Task<BaseTransaction> GenerateVoucherNumber(BaseTransaction baseTransaction)
        {
            var existingBaseTransaction = await _dbContext.Transactions.FindAsync(baseTransaction.Id);
            _dbContext.Entry(existingBaseTransaction).State = EntityState.Detached;
            baseTransaction.VoucherNumber = "080/081" + "VCH" + baseTransaction.Id + baseTransaction.BranchCode;
            _dbContext.Transactions.Attach(baseTransaction);
            _dbContext.Entry(baseTransaction).State = EntityState.Modified;
            var voucherStatus = await _dbContext.SaveChangesAsync();
            if (voucherStatus < 1) throw new Exception("Failed to Create Voucher Number");
            return baseTransaction;
        }
        private async Task<BaseTransaction> MakeBaseTransaction(ShareAccountTransactionWrapper shareAccountTransactionWrapper)
        {
            BaseTransaction baseTransaction = new BaseTransaction();
            baseTransaction = _mapper.Map<BaseTransaction>(shareAccountTransactionWrapper);
            if (shareAccountTransactionWrapper.ShareTransactionType == ShareTransactionTypeEnum.Issue)
                baseTransaction.Remarks = TransactionRemarks.ShareIssueTransaction.ToString();
            else if (shareAccountTransactionWrapper.ShareTransactionType == ShareTransactionTypeEnum.Refund)
                baseTransaction.Remarks = TransactionRemarks.ShareRefundTransaction.ToString();
            else
                baseTransaction.Remarks = TransactionRemarks.ShareTransferTransaction.ToString();
            if (shareAccountTransactionWrapper.PaymentType == PaymentTypeEnum.Bank)
            {
                var bankDetail = await _dbContext.BankSetups.FindAsync(shareAccountTransactionWrapper.BankDetailId);
                baseTransaction.BankDetail = bankDetail;
                baseTransaction.BankChequeNumber = shareAccountTransactionWrapper.BankChequeNumber;
            }
            await _dbContext.Transactions.AddAsync(baseTransaction);
            var transactionAddStatus = await _dbContext.SaveChangesAsync();
            if (transactionAddStatus < 1) throw new Exception("Failed to create Transaction");
            return await GenerateVoucherNumber(baseTransaction);
        }
    }
}