using MicroFinance.Models.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroFinance.Configuration.Transactions
{
    public class SubLedgerTransactionConfiguration : IEntityTypeConfiguration<SubLedgerTransaction>
    {
        public void Configure(EntityTypeBuilder<SubLedgerTransaction> builder)
        {
            builder.HasKey(slt=>slt.Id);
            builder.Property(slt=>slt.Id).ValueGeneratedOnAdd();
            builder.Property(slt=>slt.TransactionType).IsRequired(true);
            builder.Property(slt=>slt.BalanceAfterTransaction).HasPrecision(18,4).IsRequired(true);
            builder.HasOne(slt=>slt.Transaction)
            .WithOne(tsc=>tsc.SubLedgerTransaction)
            .HasForeignKey<SubLedgerTransaction>(slt=>slt.TransactionId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasOne(slt=>slt.SubLedger)
            .WithMany(sl=>sl.SubLedgerTransactions)
            .HasForeignKey(slt=>slt.SubLedgerId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}