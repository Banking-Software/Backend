using MicroFinance.Dtos;
using MicroFinance.Repository.DayEnd;

namespace MicroFinance.Services.DayEnd;

public class DayEndTaskService : IDayEndTaskService
{
    private readonly IDayEndTaskRepository _dayEndTaskRepository;

    public DayEndTaskService(IDayEndTaskRepository dayEndTaskRepository)
    {
        _dayEndTaskRepository=dayEndTaskRepository;
    }
    public async Task<ResponseDto> CalculateDailyInterestService()
    {
        int numberOfInterestCalculatedAccount = await _dayEndTaskRepository.CalculateDailyInterest();
        return new ResponseDto(){Message=$"Sucecessfully calculated interest rate of {numberOfInterestCalculatedAccount} account", Status=true, StatusCode="200"};
    }

    public async Task<ResponseDto> CheckMaturityOfAccountAndUpdateService()
    {
        int numberOfMatureAccount = await _dayEndTaskRepository.CheckMaturityOfAccountAndUpdate();
        return new ResponseDto(){Message=$"Successfully Updated Maturity of {numberOfMatureAccount} account.", Status=true, StatusCode="200"};
    }

    public async Task<ResponseDto> InterestPostingService()
    {
       int numberOfInterestCalculated = await _dayEndTaskRepository.CheckInterestPositingAndUpdate();
       return new ResponseDto(){Message=$"Successfully update interest posting of {numberOfInterestCalculated} account", Status=true, StatusCode="200"};
    }

    public async Task<ResponseDto> UpdateCalendarService()
    {
        string newDate = await _dayEndTaskRepository.UpdateCalendar();
        return new ResponseDto(){Message=$"Calendar Updated to {newDate}", Status=true, StatusCode="200"};
    }
}