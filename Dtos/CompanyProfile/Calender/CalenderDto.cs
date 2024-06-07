namespace MicroFinance.Dtos.CompanyProfile
{   
    public class  CalendarDto
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }
        public int NumberOfDay { get; set; }
        public int RunningDay { get; set; }
        public bool IsActive { get; set; }
        public bool? IsLocked { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
    }
}