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
        int numberAfftectedAccount = await _dayEndTaskRepository.CalculateDailyInterest();
        if(numberAfftectedAccount<0)
        {
            return new ResponseDto(){Message="Operation Failed", Status=false, StatusCode="200"};
        }
        return new ResponseDto(){Message=$"Successfully calculated interest of {numberAfftectedAccount} account", Status=true, StatusCode="200"};
    }

    public async Task<ResponseDto> CheckMaturityOfAccountAndUpdateService()
    {
        int numberOfAffectedRows = await _dayEndTaskRepository.CheckMaturityOfAccountAndUpdate();
        if(numberOfAffectedRows<0)
        {
            return new ResponseDto(){Message="Operation Failed", Status=false, StatusCode="200"};
        }
        return new ResponseDto(){Message=$"Successfully update maturity of {numberOfAffectedRows} account", Status=true, StatusCode="200"};
    }

    public Task<ResponseDto> InterestPostingService()
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> UpdateCalendarService()
    {
        throw new NotImplementedException();
    }
}