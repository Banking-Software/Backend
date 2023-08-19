namespace MicroFinance.Repository.DayEnd;

public interface IDayEndTaskRepository
{
    Task<int> CalculateDailyInterest();
    Task<int> InterestPositing();
    Task<int> CheckMaturityOfAccountAndUpdate();
    Task<int> UpdateCalendar();
}