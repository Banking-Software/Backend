using MicroFinance.Models.AccountSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroFinance.Configuration.LedgerSetup
{
    public class SubLedgerConfiguration : IEntityTypeConfiguration<SubLedger>
    {
        public void Configure(EntityTypeBuilder<SubLedger> builder)
        {
            builder.Property(at=>at.Id).ValueGeneratedNever();
            builder.Property(sl=>sl.Name).HasConversion(name=>name.ToUpper(), name=>name);
            builder.HasIndex(sl => new {sl.Name, sl.LedgerId}).IsUnique();
        }
    }
}