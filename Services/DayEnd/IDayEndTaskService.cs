using MicroFinance.Dtos;

namespace MicroFinance.Services.DayEnd;

public interface IDayEndTaskService
{
    Task<ResponseDto> CheckMaturityOfAccountAndUpdateService();
    Task<ResponseDto> CalculateDailyInterestService();
    Task<ResponseDto> InterestPostingService();
    Task<ResponseDto> UpdateCalendarService();
}