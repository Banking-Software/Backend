using MicroFinance.Models.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroFinance.Configuration.Transactions
{
    public class LedgerTransactionConfiguration : IEntityTypeConfiguration<LedgerTransaction>
    {
        public void Configure(EntityTypeBuilder<LedgerTransaction> builder)
        {
            builder.HasKey(lt=>lt.Id);
            builder.Property(lt=>lt.Id).ValueGeneratedOnAdd();
            builder.Property(lt=>lt.TransactionType).IsRequired(true);
            builder.Property(lt=>lt.BalanceAfterTransaction).HasPrecision(18,2).IsRequired(true);
            builder.HasOne(lt=>lt.Transaction)
            .WithMany(tsc=>tsc.LedgerTransaction)
            .HasForeignKey(lt=>lt.TransactionId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasOne(lt=>lt.Ledger)
            .WithMany(l=>l.LedgerTransactions)
            .HasForeignKey(lt=>lt.LedgerId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.ClientSetNull);            
        }
    }
}