using MicroFinance.Dtos;
using MicroFinance.Dtos.Reports;
using MicroFinance.Models.Wrapper.Reports;
using MicroFinance.Models.Wrapper.Reports.TrailBalance;
using MicroFinance.Services.Reports;
using MicroFinance.Token;
using Microsoft.AspNetCore.Mvc;

namespace MicroFinance.Controllers.Reports;

public class TransactionReportController : BaseApiController
{
    private readonly ITransactionReportService _transactionReportService;
    private readonly ITokenService _tokenService;

    public TransactionReportController(ITransactionReportService transactionReportService, ITokenService tokenService)
    {
        _transactionReportService = transactionReportService;
        _tokenService = tokenService;
    }

    private TokenDto GetDecodedToken()
    {
        string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var decodedToken = _tokenService.DecodeJWT(token);
        return decodedToken;
    }

    [HttpGet("depositAccount")]
    public async Task<ActionResult<DepositAccountTransactionReportWrapperDto>> GetDepositAccountTransactionReport([FromQuery] DepositAccountTransactionReportParams depositAccountTransactionReportParams)
    {
        var decodedToken = GetDecodedToken();
        return Ok(await _transactionReportService.GetDepositAccountTransactionReportService(depositAccountTransactionReportParams, decodedToken));
    }
    [HttpGet("share")]
    public async Task<ActionResult<ShareAccountTransactionReportWrapperDto>> GetShareAccountTransactionReport([FromQuery] ShareTransactionReportParams shareTransactionReportParams)
    {
        var decodedToken = GetDecodedToken();
        return Ok(await _transactionReportService.GetShareAccountTransactionReportService(shareTransactionReportParams, decodedToken));
    }
    [HttpGet("ledger")]
    public async Task<ActionResult<LedgerTransactionReportWrapperDto>> GetLedgerTransactionReport([FromQuery] LedgerTransactionReportParams ledgerTransactionReportParams)
    {
        var decodedToken = GetDecodedToken();
        return Ok(await _transactionReportService.GetLedgerTransactionReportService(ledgerTransactionReportParams, decodedToken));
    }
    [HttpGet("subledger")]
    public async Task<ActionResult<SubLedgerTransactionReportWrapperDto>> GetSubLedgerTransactionReport([FromQuery] SubLedgerTransactionReportParams subLedgerTransactionReportParams)
    {
        var decodedToken = GetDecodedToken();
        return Ok(await _transactionReportService.GetSubLedgerTransactionReportService(subLedgerTransactionReportParams, decodedToken));
    }

    [HttpGet("trailbalance")]
    public async Task<ActionResult<TrailBalance>> GetTrailBalance([FromQuery] string fromDate, [FromQuery] string toDate)
    {
        var decodedToken = GetDecodedToken();
        return Ok(await _transactionReportService.GetTrailBalanceService(fromDate, toDate));
    }
}