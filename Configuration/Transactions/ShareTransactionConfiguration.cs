using MicroFinance.Models.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroFinance.Configuration.Transactions
{
    public class ShareTransactionConfiguration : IEntityTypeConfiguration<ShareTransaction>
    {
        public void Configure(EntityTypeBuilder<ShareTransaction> builder)
        {
            builder.HasKey(st=>st.Id);
            builder.Property(st=>st.Id).ValueGeneratedOnAdd();
            builder.Property(st=>st.TransactionType).IsRequired(true);
            builder.Property(st=>st.BalanceAfterTransaction).HasPrecision(18,4).IsRequired(true);
            
            builder.HasOne(st=>st.Transaction)
            .WithOne(tsc=>tsc.ShareTransaction)
            .HasForeignKey<ShareTransaction>(st=>st.TransactionId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasOne(st=>st.ShareAccount)
            .WithMany(sa=>sa.ShareTransactions)
            .HasForeignKey(st=>st.ShareAccountId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.ClientSetNull);    

            builder.HasOne(st=>st.PaymentDepositAccount)
            .WithMany(da=>da.PaymentMethodShareTransaction)
            .HasForeignKey(st=>st.PaymentDepositAccountId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.ClientSetNull);   

             builder.HasOne(st=>st.TransferToAccount)
            .WithMany(da=>da.TransferToShareTransaction)
            .HasForeignKey(st=>st.TransferToDepositAccountId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.ClientSetNull);       
        }
    }
}