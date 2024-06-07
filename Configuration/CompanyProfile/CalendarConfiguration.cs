using MicroFinance.Models.CompanyProfile;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroFinance.Configuration.CompanyProfile
{
    public class CalendarConfiguration : IEntityTypeConfiguration<Calendar>
    {
        public void Configure(EntityTypeBuilder<Calendar> builder)
        {
            builder.HasIndex(cal => new { cal.Year, cal.Month }).IsUnique();
        }
    }
}