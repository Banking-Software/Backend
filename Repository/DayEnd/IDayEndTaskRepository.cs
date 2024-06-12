namespace MicroFinance.Repository.DayEnd;

public interface IDayEndTaskRepository
{
    Task<int> CalculateDailyInterest();
    Task<int> CheckInterestPositingAndUpdate();
    Task<int> CheckMaturityOfAccountAndUpdate();
    Task<string> UpdateCalendar();
}