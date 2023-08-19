using MicroFinance.DBContext;
using MicroFinance.Services.CompanyProfile;
using Microsoft.EntityFrameworkCore;

namespace MicroFinance.Helper;

public class NepaliCalendarFormat : INepaliCalendarFormat
{
    private readonly ILogger<NepaliCalendarFormat> _logger;
    private readonly ApplicationDbContext _dbContext;

    // private readonly ICompanyProfileService _companyProfileService;

    public NepaliCalendarFormat(ILogger<NepaliCalendarFormat> logger, ApplicationDbContext dbContext)
    {
        _logger= logger;
        _dbContext=dbContext;
        // _companyProfileService=companyProfileService;
    }

    public Task<string> ConvertEnglishDateToNepali(DateTime englishDate)
    {
        return Task.FromResult(NepaliCalendar.Convert.ToNepali(englishDate));
    }

    public Task<DateTime> ConvertNepaliDateToEnglish(string nepaliDate)
    {
        DateTime englishDate = NepaliCalendar.Convert.ToEnglish(nepaliDate);
        return Task.FromResult(englishDate);
    }

    public async Task<bool> VerifyNepaliDate(string nepaliDate)
    {
        try
        {
            await ConvertNepaliDateToEnglish(nepaliDate);
            return true;
        }
        catch(Exception ex)
        {
            _logger.LogError($"{DateTime.Now}: Nepali Date received: {nepaliDate}. Wrong Nepali Date Encountered");
            return false;
        }
    }

    public async Task<bool> VerifyNepaliDateFormat(string nepaliDate)
    {
        var nepaliDateStringSplited = nepaliDate.Split("-");
        int year;
        int month;
        int day;
        bool isYearConvertable = int.TryParse(nepaliDateStringSplited[0], out year);
        bool isMonthConvertable = int.TryParse(nepaliDateStringSplited[1], out month);
        bool isDayConvertable = int.TryParse(nepaliDateStringSplited[2], out day);
        if (!isYearConvertable || !isMonthConvertable || !isDayConvertable)
            return false;
        return true;
    }

    public async Task<string> GetNepaliFormatDate(int year, int month, int day)
    {
        string monthString = month.ToString();
        string dayString = day.ToString();
        string yearString = year.ToString();

        if(monthString.Length==1)
            monthString = $"0{monthString}";
        if(dayString.Length==1)
            dayString = $"0{dayString}";
        string nepaliDate = $"{yearString}-{monthString}-{dayString}";
        // bool isDateFormatValid = await VerifyNepaliDateFormat(nepaliDate);
        bool isDateValid = await VerifyNepaliDate(nepaliDate);
        if( isDateValid )
            return nepaliDate;
        return string.Empty;
    }

    public async Task<string> GetNepaliFormatDate(string nepaliDate)
    {
        bool isFormatCorrect = await VerifyNepaliDateFormat(nepaliDate);
        var splitedDate = nepaliDate.Split("-");
        string year = splitedDate[0];
        string month = splitedDate[1].Length==1?$"0{splitedDate[1]}":splitedDate[1];
        string day = splitedDate[2].Length==1?$"0{splitedDate[2]}":splitedDate[2];
        string finalizeFormat = $"{year}-{month}-{day}";
        bool isDateValid = await VerifyNepaliDate(finalizeFormat);
        if(isDateValid)
            return finalizeFormat;
        return string.Empty;
    }

    public async Task<DateTime> GetCurrentCompanyDate()
    {
        var activeCalendar = await _dbContext.Calendars.Where(c=>c.IsActive).SingleOrDefaultAsync();
        if(activeCalendar==null) throw new Exception("No Active Calendar Found");
        string companyCalendarNepaliDate = await GetNepaliFormatDate(activeCalendar.Year, activeCalendar.Month, activeCalendar.RunningDay);
        DateTime companyCalendarEnglishDate = await ConvertNepaliDateToEnglish(companyCalendarNepaliDate);
        return companyCalendarEnglishDate;
        
    }
}