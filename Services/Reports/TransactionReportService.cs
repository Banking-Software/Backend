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
    private readonly ICommonExpression _commonExpression;

    public TransactionReportService
    (
        ITransactionReportRepository transactionReportrepository,
        IDepositSchemeRepository depositSchemeRepository,
        IShareService shareService,
        INepaliCalendarFormat nepaliCalendarFormat,
        ICommonExpression commonExpression,
        IMapper mapper
    )
    {
        _transactionReportrepository = transactionReportrepository;
        _depositSchemeRepository = depositSchemeRepository;
        _shareService = shareService;
        _nepaliCalendarFormat=nepaliCalendarFormat;
        _mapper=mapper;
        _commonExpression=commonExpression;
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

    public async Task<DepositAccountTransactionReportWrapperDto> GetDepositAccountTransactionReportService(DepositAccountTransactionReportParams depositAccountTransactionReportParams, TokenDto decodedToken)
    {
        DateTime fromDate = await GetEnglishDate(depositAccountTransactionReportParams.FromDate);
        DateTime toDate = await GetEnglishDate(depositAccountTransactionReportParams.ToDate);
        if(fromDate>toDate)
            throw new BadRequestExceptionHandler("From Date connot be greater than To Date");

        var depositTransactionReport = await _transactionReportrepository.GetDepositAccountTransactionReport(await _commonExpression.GetExpressionForDepositAccountTransactionReport(depositAccountTransactionReportParams, fromDate, toDate));
        return new DepositAccountTransactionReportWrapperDto()
        {
            PreviousBalanceAfterTransaction = depositTransactionReport.PreviousBalanceAfterTransaction,
            DebitSum = depositTransactionReport.DebitSum,
            CreditSum = depositTransactionReport.CreditSum,
            DepositAccountTransactionDtos = _mapper.Map<List<DepositAccountTransactionReportDto>>(depositTransactionReport.DepositAccountTransactionReports)
        };
    }

    public async Task<LedgerTransactionReportWrapperDto> GetLedgerTransactionReportService(LedgerTransactionReportParams ledgerTransactionReportParams, TokenDto decodedToken)
    {
        DateTime fromDate = await GetEnglishDate(ledgerTransactionReportParams.FromDate);
        DateTime toDate = await GetEnglishDate(ledgerTransactionReportParams.ToDate);
        if(fromDate>toDate)
            throw new BadRequestExceptionHandler("From Date connot be greater than To Date");
        var ledgerTransactionReport = await _transactionReportrepository.GetLedgerTransactionReport(await _commonExpression.GetExpressionForLedgerTransactionReport(ledgerTransactionReportParams, fromDate, toDate));
        return new LedgerTransactionReportWrapperDto()
        {
            PreviousBalanceAfterTransaction = ledgerTransactionReport.PreviousBalanceAfterTransaction,
            DebitSum = ledgerTransactionReport.DebitSum,
            CreditSum = ledgerTransactionReport.CreditSum,
            LedgerTransactionReportDtos = _mapper.Map<List<LedgerTransactionReportDto>>(ledgerTransactionReport.LedgerTransactionReports)
        };
    }

    public async Task<ShareAccountTransactionReportWrapperDto> GetShareAccountTransactionReportService(ShareTransactionReportParams shareTransactionReportParams, TokenDto decodedToken)
    {
        DateTime fromDate = await GetEnglishDate(shareTransactionReportParams.FromDate);
        DateTime toDate = await GetEnglishDate(shareTransactionReportParams.ToDate);
        if(fromDate>toDate)
            throw new BadRequestExceptionHandler("From Date connot be greater than To Date");
        var shareTransactionReport = await _transactionReportrepository.GetShareTransactionReport(await _commonExpression.GetExpressionForShareTransactionReport(shareTransactionReportParams, fromDate, toDate));
        return new ShareAccountTransactionReportWrapperDto()
        {
            PreviousBalanceAfterTransaction = shareTransactionReport.PreviousBalanceAfterTransaction,
            DebitSum = shareTransactionReport.DebitSum,
            CreditSum = shareTransactionReport.CreditSum,
            ShareAccountTransactionReportDtos = _mapper.Map<List<ShareAccountTransactionReportDto>>(shareTransactionReport.ShareTransactionReports)
        };
    }

    public async Task<SubLedgerTransactionReportWrapperDto> GetSubLedgerTransactionReportService(SubLedgerTransactionReportParams suLedgerTransactionReportParams, TokenDto decodedToken)
    {
        DateTime fromDate = await GetEnglishDate(suLedgerTransactionReportParams.FromDate);
        DateTime toDate = await GetEnglishDate(suLedgerTransactionReportParams.ToDate);
        if(fromDate>toDate)
            throw new BadRequestExceptionHandler("From Date connot be greater than To Date");
        var subLedgerTransactionReport = await _transactionReportrepository.GetSubLedgerTransactionReport(await _commonExpression.GetExpressionForSubLedgerTransactionReport(suLedgerTransactionReportParams, fromDate, toDate));
        return new SubLedgerTransactionReportWrapperDto()
        {
            PreviousBalanceAfterTransaction = subLedgerTransactionReport.PreviousBalanceAfterTransaction,
            DebitSum = subLedgerTransactionReport.DebitSum,
            CreditSum = subLedgerTransactionReport.CreditSum,
            SubLedgerTransactionReportDtos = _mapper.Map<List<SubLedgerTransactionReportDto>>(subLedgerTransactionReport.SubLedgerTransactionReports)
        };
    }
}