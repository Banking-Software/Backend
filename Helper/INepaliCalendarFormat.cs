namespace MicroFinance.Helper;

public interface INepaliCalendarFormat
{
    Task<bool> VerifyNepaliDateFormat(string nepaliDate);
    Task<bool> VerifyNepaliDate(string nepaliDate);
    Task<DateTime> ConvertNepaliDateToEnglish(string nepaliDate);
    Task<string> ConvertEnglishDateToNepali(DateTime englishDate);
    Task<string> GetNepaliFormatDate(int year, int month, int day);
    Task<string> GetNepaliFormatDate(string nepaliDate);
}