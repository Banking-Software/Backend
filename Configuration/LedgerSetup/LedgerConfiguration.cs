using MicroFinance.Models.AccountSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroFinance.Configuration.LedgerSetup
{
    public class LedgerConfiguration : IEntityTypeConfiguration<Ledger>
    {
        public void Configure(EntityTypeBuilder<Ledger> builder)
        {
            builder.HasOne(l => l.GroupTypeAndLedgerMap)
             .WithOne(gtl => gtl.Ledger)
             .HasForeignKey<GroupTypeAndLedgerMap>(gtl => gtl.LedgerId)
             .IsRequired()
             .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(l => l.SubLedger)
            .WithOne(sl => sl.Ledger)
            .HasForeignKey(sl => sl.LedgerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}