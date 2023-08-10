using MicroFinance.Dtos;
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

    public TransactionReportService
    (
        ITransactionReportRepository transactionReportrepository,
        IDepositSchemeRepository depositSchemeRepository,
        IShareService shareService
    )
    {
        _transactionReportrepository=transactionReportrepository; 
        _depositSchemeRepository=depositSchemeRepository;
        _shareService=shareService;
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
            Year=Int32.Parse(splitedDate[0]), 
            Month = Int32.Parse(splitedDate[1]), 
            Day = Int32.Parse(splitedDate[2])
        };
    }

    public async Task<DepositAccountTransactionReportWrapper> GetDepositAccountTransactionReportService(string fromDate, string toDate, int depositAccountId, TokenDto decodedToken)
    {
      var depositAccount = await _depositSchemeRepository.GetNonClosedDepositAccountById(depositAccountId);
      if(depositAccount==null) throw new Exception("Your account might be closed");
      DateWrapper fromDateWrapper = GetDateWrapper(fromDate);
      DateWrapper toDateWrapper = GetDateWrapper(toDate);
      if(fromDateWrapper.Month > 12 || toDateWrapper.Month > 12 || fromDateWrapper.Day > 32 || toDateWrapper.Day > 32)
      {
        throw new Exception("Date Invalid");
      }

      return await _transactionReportrepository.GetDepositAccountTransactionReport(fromDateWrapper, toDateWrapper, depositAccountId);
    }

    public Task<ShareTransactionReportWrapper> GetShareAccountTransactionReportService(string fromDate, string toDate, int shareAccountId, TokenDto decodedToken)
    {
        throw new NotImplementedException();
    }

}