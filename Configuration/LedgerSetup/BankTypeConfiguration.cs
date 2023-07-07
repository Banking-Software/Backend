using MicroFinance.Models.AccountSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroFinance.Configuration.LedgerSetup
{
    public class BankTypeConfiguration : IEntityTypeConfiguration<BankType>
    {
        public void Configure(EntityTypeBuilder<BankType> builder)
        {
            builder.Property(at=>at.Id).ValueGeneratedNever();
        }
    }
}