using MicroFinance.Models.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroFinance.Configuration.Transactions
{
    public class DepositTransactionConfiguration : IEntityTypeConfiguration<DepositTransaction>
    {
        public void Configure(EntityTypeBuilder<DepositTransaction> builder)
        {
            builder.HasOne(dt=>dt.Transaction)
            .WithOne(t=>t.DepositTransaction)
            .HasForeignKey<DepositTransaction>(dt=>dt.TransactionId)
            .IsRequired()
            .OnDelete(DeleteBehavior.ClientSetNull);

        }
    }
}