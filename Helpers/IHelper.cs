using MicroFinance.Dtos.CompanyProfile;
using MicroFinance.Dtos.DepositSetup.Account;
using MicroFinance.Enums;

namespace MicroFinance.Helpers
{
    public interface IHelper
    {
        // CALENDAR SUPPORT
        Task<bool> VerifyNepaliDateFormat(string nepaliDate);
        Task<bool> VerifyNepaliDate(string nepaliDate);
        Task<DateTime> ConvertNepaliDateToEnglish(string nepaliDate);
        Task<string> ConvertEnglishDateToNepali(DateTime englishDate);
        Task<string> GetNepaliFormatDate(int year, int month, int day);
        Task<string> GetNepaliFormatDate(string nepaliDate);
        Task<List<int>> GetYearMonthDayFromStringDate(string date);

        Task<DateTime> GetCompanyActiveCalendarInAD(CalendarDto calendarDto);
        Task<DateTime> GenerateNextInterestPostingDate(DateTime lastInterestPostedDate, DateTime accountMaturityDate, CalendarDto currentActiveCalendar, PostingSchemeEnum postingScheme, bool? isExactMonth);
        Task<MatureDateDto> GenerateMatureDateOfAccount(GenerateMatureDateDto generateMatureDateDto);
        Task<string> HumanizeAmount(decimal amount);

    }
}