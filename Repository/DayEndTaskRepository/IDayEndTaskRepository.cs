namespace MicroFinance.Repository.DayEndTaskRepository;

public interface IDayEndTaskRepository
{
    Task<int> CalculateDailyInterest();
    Task<int> InterestPositing();
    Task<int> CheckMaturityOfAccountAndUpdate();
    Task<int> UpdateCalendar();
}