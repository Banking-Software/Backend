using MicroFinance.Models.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroFinance.Configuration.Transactions
{
    public class DepositAccountTransactionConfiguration : IEntityTypeConfiguration<DepositAccountTransaction>
    {
        public void Configure(EntityTypeBuilder<DepositAccountTransaction> builder)
        {
            builder.HasIndex(dt=>dt.Id);
            builder.Property(dt=>dt.Id).ValueGeneratedOnAdd();
            builder.Property(dt=>dt.BalanceAfterTransaction).HasPrecision(18,2).IsRequired(true);
            builder.Property(dt=>dt.Source).IsRequired(true);
            builder.Property(dt=>dt.TransactionType).HasPrecision(18,2).IsRequired(true);

            builder.HasOne(dt=>dt.Transaction)
            .WithOne(tsc=>tsc.DepositAccountTransaction)
            .HasForeignKey<DepositAccountTransaction>(dt=>dt.TransactionId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.ClientCascade);
        
            builder.HasOne(dt=>dt.DepositAccount)
            .WithMany(da=>da.DepositAccountTransactions)
            .HasForeignKey(dt=>dt.DepositAccountId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}