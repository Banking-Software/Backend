using MicroFinance.Models.AccountSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroFinance.Configuration.LedgerSetup
{
    public class DebitOrCreditConfiguration : IEntityTypeConfiguration<DebitOrCredit>
    {
        public void Configure(EntityTypeBuilder<DebitOrCredit> builder)
        {
            builder.Property(dc=>dc.Id).ValueGeneratedNever();
        }
    }
}