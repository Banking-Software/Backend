using MicroFinance.Models.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroFinance.Configuration.Transactions
{
    public class InterestTransactionConfiguration : IEntityTypeConfiguration<InterestTransaction>
    {
        public void Configure(EntityTypeBuilder<InterestTransaction> builder)
        {
            builder.HasIndex(it=>it.Id);
            builder.Property(it=>it.Id).ValueGeneratedOnAdd();
            builder.Property(it=>it.CalculatedInterestAmount).HasPrecision(18,2).IsRequired(true);
            builder.Property(it=>it.CalculatedInterestRate).HasPrecision(5,2).IsRequired(true);
            builder.Property(it=>it.IsInterestPosted).IsRequired(true);

            builder.HasOne(it=>it.Transaction)
            .WithOne(bt=>bt.InterestTransactions)
            .HasForeignKey<InterestTransaction>(it=>it.TransactionId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasOne(it=>it.DepositAccount)
            .WithMany(da=>da.InterestTransactions)
            .HasForeignKey(it=>it.DepositAccountId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}