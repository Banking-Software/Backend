using System.Linq.Expressions;
using AutoMapper;
using MicroFinance.Dtos;
using MicroFinance.Dtos.Reports;
using MicroFinance.Exceptions;
using MicroFinance.Helper;
using MicroFinance.Models.DepositSetup;
using MicroFinance.Models.Transactions;
using MicroFinance.Models.Wrapper;
using MicroFinance.Models.Wrapper.Reports;
using MicroFinance.Repository.DepositSetup;
using MicroFinance.Repository.Reports;
using MicroFinance.Services.Share;

namespace MicroFinance.Services.Reports;


public class TransactionReportService : ITransactionReportService
{
    private readonly ITransactionReportRepository _transactionReportrepository;
    private readonly IDepositSchemeRepository _depositSchemeRepository;
    private readonly IShareService _shareService;
    private readonly INepaliCalendarFormat _nepaliCalendarFormat;
    private readonly IMapper _mapper;

    public TransactionReportService
    (
        ITransactionReportRepository transactionReportrepository,
        IDepositSchemeRepository depositSchemeRepository,
        IShareService shareService,
        INepaliCalendarFormat nepaliCalendarFormat,
        IMapper mapper
    )
    {
        _transactionReportrepository = transactionReportrepository;
        _depositSchemeRepository = depositSchemeRepository;
        _shareService = shareService;
        _nepaliCalendarFormat=nepaliCalendarFormat;
        _mapper=mapper;
    }

    // private bool checkInputDateFormat(string input)
    // {
    //     bool result = false;
    //     foreach(char c in input)
    //     {
    //         if(c!='/' || !char.IsDigit(c))
    //             result = false;
    //         else result = true;
    //     }
    // }

    private DateWrapper GetDateWrapper(string date)
    {

        var splitedDate = date.Split("/");
        return new DateWrapper()
        {
            Year = Int32.Parse(splitedDate[0]),
            Month = Int32.Parse(splitedDate[1]),
            Day = Int32.Parse(splitedDate[2])
        };
    }

    private async Task<DateTime> GetEnglishDate(string nepaliDate)
    {
        string formattedDate = await _nepaliCalendarFormat.GetNepaliFormatDate(nepaliDate);
        if(string.IsNullOrEmpty(formattedDate)) 
            throw new Exception("Please enter date in right format. Correct Format is YYYY-MM-DD");
        return await _nepaliCalendarFormat.ConvertNepaliDateToEnglish(formattedDate);
    }

    public async Task<DepositAccountTransactionReportWrapperDto> GetDepositAccountTransactionReportService(GetDepositAccountTransactionReport getDepositAccountTransactionReport, TokenDto decodedToken)
    {
        DateTime fromDate = await GetEnglishDate(getDepositAccountTransactionReport.FromDate);
        DateTime toDate = await GetEnglishDate(getDepositAccountTransactionReport.ToDate);

        Expression<Func<DepositAccount, bool>> expressionOnDepositAccount; 
        Expression<Func<DepositAccountTransaction,bool>> expressionOnDepositAccountTransaction;
        if(getDepositAccountTransactionReport.AccountStatus!=null)
        {
            expressionOnDepositAccount = da => da.Id == getDepositAccountTransactionReport.DepositAccountId && da.Status==getDepositAccountTransactionReport.AccountStatus;
            
            expressionOnDepositAccountTransaction 
            = dat=>dat.DepositAccountId==getDepositAccountTransactionReport.DepositAccountId 
            && dat.DepositAccount.Status==getDepositAccountTransactionReport.AccountStatus
            && dat.Transaction.EnglishCreationDate >= fromDate && dat.Transaction.EnglishCreationDate <= toDate;
        }
        else
        {
            expressionOnDepositAccount=da=>da.Id==getDepositAccountTransactionReport.DepositAccountId;

            expressionOnDepositAccountTransaction 
            = dat=>dat.DepositAccountId==getDepositAccountTransactionReport.DepositAccountId
            && dat.Transaction.EnglishCreationDate >= fromDate && dat.Transaction.EnglishCreationDate <= toDate;
        }
        var depositAccount = await _depositSchemeRepository.GetDepositAccount(expressionOnDepositAccount);
        if (depositAccount == null) throw new NotFoundExceptionHandler("No Account Found");
        var depositTransactionReport = await _transactionReportrepository.GetDepositAccountTransactionReport(expressionOnDepositAccountTransaction);

        return new DepositAccountTransactionReportWrapperDto()
        {
            PreviousBalanceAfterTransaction = depositTransactionReport.PreviousBalanceAfterTransaction,
            DebitSum = depositTransactionReport.DebitSum,
            CreditSum = depositTransactionReport.CreditSum,
            DepositAccountTransactionDtos = _mapper.Map<List<DepositAccountTransactionReportDto>>(depositTransactionReport.DepositAccountTransactionReports)
        };
    }

    public Task<ShareTransactionReportWrapper> GetShareAccountTransactionReportService(string fromDate, string toDate, int shareAccountId, TokenDto decodedToken)
    {
        throw new NotImplementedException();
    }

}