using MicroFinance.DBContext;
using MicroFinance.Dto.Transactions;
using MicroFinance.Dtos.CompanyProfile;
using MicroFinance.Enums;
using MicroFinance.Enums.Transaction;
using MicroFinance.Exceptions;
using MicroFinance.Helpers;
using MicroFinance.Models.AccountSetup;
using MicroFinance.Models.Transactions;
using MicroFinance.Services.CompanyProfile;
using Microsoft.EntityFrameworkCore;

namespace MicroFinance.Repository.Transaction;

public class ManualVoucherTransactionRepository : IManualVoucherTransactionRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<ManualVoucherTransactionRepository> _logger;
    private readonly ITransactions _transactions;
    private readonly IHelper _helper;
    private readonly ICompanyProfileService _companyProfileService;
    private Dictionary<int, Ledger> allLedger = new Dictionary<int, Ledger>();
    private Dictionary<int, SubLedger> allSubLedger = new Dictionary<int, SubLedger>();
    private string companyNepaliCalendar;
    private DateTime companyEnglishCalendar;

    public ManualVoucherTransactionRepository
    (
        ApplicationDbContext dbContext,
        ITransactions transactions,
        ILogger<ManualVoucherTransactionRepository> logger,
        IHelper helper,
        ICompanyProfileService companyProfileService
    )
    {
        _dbContext = dbContext;
        _logger = logger;
        _transactions = transactions;
        _helper = helper;
        _companyProfileService = companyProfileService;
    }

    public async Task ManualVoucherTransaction(List<ManualVoucherDto> manualVouchers, string userBranchCode)
    {
        TransactionVoucher transactionVoucher = await _transactions.GenerateTransactionVoucher(userBranchCode, 0);
        await SetCompanyCurrentDate();
        // If Ledger and SubLedger doesnot exist the dictionary only then we query in the database.
        foreach (var manualVoucher in manualVouchers)
        {
            BaseTransaction baseTransaction = await HandleBaseTransaction(transactionVoucher, manualVoucher.TransactionAmount, userBranchCode);
            if (manualVoucher.IsLedger)
                await HandleLedgerTransaction(baseTransaction, manualVoucher);
            else
                await HandleSubLedgerTransaction(baseTransaction, manualVoucher);
        }
        int numberOfRowsAfftected = await _dbContext.SaveChangesAsync();
        if(numberOfRowsAfftected<=0)
            throw new Exception("Failed to update for the provided ledgers or subledgers");
        allLedger.Clear();
        allSubLedger.Clear();
    }

    private async Task<bool> IsDeposit(int accountTypeId, TransactionTypeEnum transactionType)
    {
        bool isDeposit =
        ((accountTypeId == 1 || accountTypeId == 2) && transactionType == TransactionTypeEnum.Debit)
        ||
        ((accountTypeId==3||accountTypeId==4) && transactionType == TransactionTypeEnum.Credit);
        return isDeposit;
    }

    private async Task HandleLedgerTransaction(BaseTransaction baseTransaction, ManualVoucherDto manualVoucher)
    {
        bool isDeposit = true;
        int ledgerId = manualVoucher.LedgerdId;
        TransactionTypeEnum transactionType = manualVoucher.TransactionType;
        await UpdateLedgerEntryInDictionary(ledgerId);
        int groupTypeId = allLedger[ledgerId].GroupTypeId;
        int accountTypeId = (await _dbContext.GroupTypes.Where(gt=>gt.Id==groupTypeId).AsNoTracking().SingleOrDefaultAsync()).AccountTypeId;
        if(!(await IsDeposit(accountTypeId, transactionType)))
            isDeposit = false;
        LedgerTransaction ledgerTransaction = await LedgerTransactionEntry(baseTransaction, manualVoucher, ledgerId);
        await _transactions.TransactionOnLedger(ledgerTransaction, isDeposit);
        // await _transactions.TransactionEntryForLedger(ledgerTransaction);
    }
    private async Task HandleSubLedgerTransaction(BaseTransaction baseTransaction, ManualVoucherDto manualVoucher)
    {
        bool isDeposit=true;
        int subLedgerId = manualVoucher.LedgerdId;
        TransactionTypeEnum transactionType = manualVoucher.TransactionType;
        await UpdateSubLedgerInDictionary(subLedgerId);
        int ledgerId = allSubLedger[subLedgerId].LedgerId;
        int groupTypeId = allLedger[ledgerId].GroupTypeId;
        int accountTypeId = (await _dbContext.GroupTypes.Where(gt=>gt.Id==groupTypeId).AsNoTracking().SingleOrDefaultAsync()).AccountTypeId;
        if(!(await IsDeposit(accountTypeId, transactionType)))
            isDeposit = false;
        SubLedgerTransaction subLedgerTransaction = await SubLedgerTransactionEntry(baseTransaction, manualVoucher);
        LedgerTransaction ledgerTransaction = await LedgerTransactionEntry(baseTransaction, manualVoucher, ledgerId);
        await _transactions.TransactionOnSubLedger(subLedgerTransaction, isDeposit); 
        await _transactions.TransactionOnLedger(ledgerTransaction, isDeposit);
    }

    private async Task<LedgerTransaction> LedgerTransactionEntry( BaseTransaction baseTransaction, ManualVoucherDto manualVoucher, int ledgerId)
    {
        LedgerTransaction ledgerTransaction = new ()
        {
            Ledger = allLedger[ledgerId],
            Transaction = baseTransaction,
            TransactionType = manualVoucher.TransactionType,
            Remarks = manualVoucher.Description??"Transaction Done by Manual Voucher",
            Narration = "Manual Voucher Transaction",
            BalanceAfterTransaction = allLedger[ledgerId].CurrentBalance
        };
        return ledgerTransaction;
    }
    private async Task<SubLedgerTransaction> SubLedgerTransactionEntry(BaseTransaction baseTransaction, ManualVoucherDto manualVoucher)
    {
        SubLedgerTransaction subLedgerTransaction = new ()
        {
            SubLedger = allSubLedger[manualVoucher.LedgerdId],
            Transaction = baseTransaction,
            TransactionType = manualVoucher.TransactionType,
            Remarks = manualVoucher.Description??"Transaction Done by Manual Voucher",
            Narration = "Manual Voucher Transaction",
            BalanceAfterTransaction = allSubLedger[manualVoucher.LedgerdId].CurrentBalance
        };
        return subLedgerTransaction;
    }

    private async Task SetCompanyCurrentDate()
    {
        CalendarDto calendarDto = await _companyProfileService.GetCurrentActiveCalenderService();
        companyNepaliCalendar = await _helper.GetNepaliFormatDate(calendarDto.Year, calendarDto.Month, calendarDto.RunningDay);
        companyEnglishCalendar = await _helper.ConvertNepaliDateToEnglish(companyNepaliCalendar);
    }

    private async Task<BaseTransaction> HandleBaseTransaction(TransactionVoucher transactionVoucher, decimal transactionAmount, string userBranchCode)
    {
        BaseTransaction baseTransaction = new BaseTransaction()
        {
            TransactionVoucher = transactionVoucher,
            BranchCode = userBranchCode,
            TransactionAmount = transactionAmount,
            Remarks = TransactionRemarks.ManualVoucherTransaction.ToString(),
            NepaliCreationDate = companyNepaliCalendar,
            EnglishCreationDate = companyEnglishCalendar,
            RealWorldCreationDate = DateTime.Now,
            PaymentType = PaymentTypeEnum.Internal
        };
        await _transactions.GenerateBaseTransaction(baseTransaction);
        return baseTransaction;
    }

    private async Task UpdateSubLedgerInDictionary(int subLedgerId)
    {
        SubLedger subLedger = allSubLedger.ContainsKey(subLedgerId) ? allSubLedger[subLedgerId] : await _dbContext.SubLedgers.FindAsync(subLedgerId);
        if (subLedger == null)
            throw new Exception("No SubLedger Found");
        if (!allSubLedger.ContainsKey(subLedgerId))
            allSubLedger.Add(subLedgerId, subLedger);
        // Update Ledger Associate with this subledger
        await UpdateLedgerEntryInDictionary(subLedger.LedgerId);
    }
    private async Task UpdateLedgerEntryInDictionary(int ledgerId)
    {
        Ledger ledger = allLedger.ContainsKey(ledgerId) ? allLedger[ledgerId] : await _dbContext.Ledgers.FindAsync(ledgerId);
        if (ledger == null)
            throw new Exception("No Ledger Found");
        if (!allLedger.ContainsKey(ledgerId))
            allLedger.Add(ledgerId, ledger);
    }
}