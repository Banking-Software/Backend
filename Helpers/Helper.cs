using Humanizer;
using MicroFinance.Dtos.CompanyProfile;
using MicroFinance.Dtos.DepositSetup.Account;
using MicroFinance.Enums;

namespace MicroFinance.Helpers
{
    public class Helper : IHelper
    {
        private readonly ILogger<Helper> _logger;

        public Helper(ILogger<Helper> logger)
        {
            _logger = logger;
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
            catch (Exception ex)
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

            if (monthString.Length == 1)
                monthString = $"0{monthString}";
            if (dayString.Length == 1)
                dayString = $"0{dayString}";
            string nepaliDate = $"{yearString}-{monthString}-{dayString}";
            bool isDateValid = await VerifyNepaliDate(nepaliDate);
            if (isDateValid)
                return nepaliDate;
            return string.Empty;
        }

        public async Task<string> GetNepaliFormatDate(string nepaliDate)
        {
            bool isFormatCorrect = await VerifyNepaliDateFormat(nepaliDate);
            var splitedDate = nepaliDate.Split("-");
            string year = splitedDate[0];
            string month = splitedDate[1].Length == 1 ? $"0{splitedDate[1]}" : splitedDate[1];
            string day = splitedDate[2].Length == 1 ? $"0{splitedDate[2]}" : splitedDate[2];
            string finalizeFormat = $"{year}-{month}-{day}";
            bool isDateValid = await VerifyNepaliDate(finalizeFormat);
            if (isDateValid)
                return finalizeFormat;
            return string.Empty;
        }
        public async Task<List<int>> GetYearMonthDayFromStringDate(string date)
        {
            var nepaliDateStringSplited = date.Split("-");
            int year;
            int month;
            int day;
            bool isYearConvertable = int.TryParse(nepaliDateStringSplited[0], out year);
            bool isMonthConvertable = int.TryParse(nepaliDateStringSplited[1], out month);
            bool isDayConvertable = int.TryParse(nepaliDateStringSplited[2], out day);
            if (!isYearConvertable || !isMonthConvertable || !isDayConvertable)
                return new List<int>();
            return new List<int>() { year, month, day };
        }

        public async Task<DateTime> GenerateNextInterestPostingDate(DateTime lastInterestPostedDate, DateTime accountMaturityDate, CalendarDto currentActiveCalendar, PostingSchemeEnum postingScheme, bool? isExactMonth)
        {
            DateTime nextPostingDateinEnglish;
            if (postingScheme == PostingSchemeEnum.Yearly)
                nextPostingDateinEnglish = lastInterestPostedDate.AddYears(1);
            else if (postingScheme == PostingSchemeEnum.HalfYearly)
                nextPostingDateinEnglish = lastInterestPostedDate.AddMonths(6);
            else if (postingScheme == PostingSchemeEnum.Quarterly)
                nextPostingDateinEnglish = lastInterestPostedDate.AddMonths(3);
            else if (postingScheme == PostingSchemeEnum.Monthly)
            {
                if(isExactMonth!=null && isExactMonth==true)
                    nextPostingDateinEnglish = lastInterestPostedDate.AddMonths(1);
                else
                {
                    string formatedBSDate = await GetNepaliFormatDate($"{currentActiveCalendar.Year}-{currentActiveCalendar.Month}-{currentActiveCalendar.NumberOfDay}");
                    nextPostingDateinEnglish = await ConvertNepaliDateToEnglish(formatedBSDate);
                }
            }
            else
                nextPostingDateinEnglish = accountMaturityDate;
            return nextPostingDateinEnglish;
        }
        public async Task<DateTime> GetCompanyActiveCalendarInAD(CalendarDto calendarDto)
        {
            string calendarInBS = $"{calendarDto.Year}-{calendarDto.Month}-{calendarDto.RunningDay}";
            string formatedBS = await GetNepaliFormatDate(calendarInBS);
            DateTime calendarInAD = await ConvertNepaliDateToEnglish(formatedBS);
            return calendarInAD;
        }

        public async Task<MatureDateDto> GenerateMatureDateOfAccount(GenerateMatureDateDto generateMatureDateDto)
        {
            DateTime openingDate;
            if(generateMatureDateDto.OpeningDateEnglish==null)
            {
                bool isOpeningDateFormatCorrect = await VerifyNepaliDateFormat(generateMatureDateDto.OpeningDate);
                openingDate = await ConvertNepaliDateToEnglish(generateMatureDateDto.OpeningDate);
            }
            else
                openingDate=(DateTime) generateMatureDateDto.OpeningDateEnglish;

            DateTime maturePeriodInEnglishFormat;
            if (generateMatureDateDto.PeriodType == PeriodTypeEnum.Year)
                maturePeriodInEnglishFormat = openingDate.AddYears(generateMatureDateDto.Period).AddDays(-1);

            else if (generateMatureDateDto.PeriodType == PeriodTypeEnum.Month)
                maturePeriodInEnglishFormat = openingDate.AddMonths(generateMatureDateDto.Period).AddDays(-1);

            else
                maturePeriodInEnglishFormat = openingDate.AddDays(generateMatureDateDto.Period).AddDays(-1);

            string nepaliMatureDate = await ConvertEnglishDateToNepali(maturePeriodInEnglishFormat);
            return new MatureDateDto()
            {
                NepaliMatureDate = nepaliMatureDate,
                EnglishMatureDate = maturePeriodInEnglishFormat
            };
        }

        public Task<string> HumanizeAmount(decimal amount)
        {
            int firstHalfInterestAmonut = (int)amount;
            string unFormatedSecondHalfInterestAmount = ((int)(amount - firstHalfInterestAmonut) * 100).ToString();
            unFormatedSecondHalfInterestAmount = unFormatedSecondHalfInterestAmount.TrimEnd(new char[] { '0' });
            int secondHalfInterestAmount = 0;
            _ = int.TryParse(unFormatedSecondHalfInterestAmount, out secondHalfInterestAmount);
            return Task.FromResult($"{firstHalfInterestAmonut.ToWords()} point {secondHalfInterestAmount.ToWords()}");
        }
    }
}