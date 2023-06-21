using MicroFinance.Models.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroFinance.Configuration.Transactions
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasOne(t=>t.DepositAccount)
            .WithMany(da=>da.Transactions)
            .HasForeignKey(t=>t.DepositAccountId).IsRequired(false)
            .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}