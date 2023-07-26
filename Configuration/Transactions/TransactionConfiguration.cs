using MicroFinance.Models.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroFinance.Configuration.Transactions
{
    public class TransactionConfiguration : IEntityTypeConfiguration<BaseTransaction>
    {
        public void Configure(EntityTypeBuilder<BaseTransaction> builder)
        {
            builder.HasKey(tsc=>tsc.Id);
            builder.Property(tsc=>tsc.Id).ValueGeneratedOnAdd();
            builder.HasIndex(tsc=>tsc.VoucherNumber).IsUnique();
            builder.Property(tsc=>tsc.TransactionAmount).HasPrecision(18,4).IsRequired(true);
            builder.Property(tsc=>tsc.RealWorldCreationDate).IsRequired(true);
            builder.Property(tsc=>tsc.CompanyCalendarCreationDate).IsRequired(true);
        }
    }
}