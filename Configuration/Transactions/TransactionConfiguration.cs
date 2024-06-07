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
            
            builder.Property(tsc=>tsc.TransactionAmount).HasPrecision(18,2).IsRequired(true);
            builder.Property(tsc=>tsc.RealWorldCreationDate).IsRequired(true);
            builder.Property(tsc=>tsc.EnglishCreationDate).IsRequired(true);
            builder.Property(tsc=>tsc.NepaliCreationDate).IsRequired(true);

            builder.HasOne(dt=>dt.BankDetail)
            .WithMany(bnk=>bnk.BaseTransactions)
            .HasForeignKey(dt=>dt.BankDetailId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.ClientSetNull);

             builder.HasOne(bt=>bt.TransactionVoucher)
            .WithMany(tv=>tv.BaseTransactions)
            .HasForeignKey(bt=>bt.VoucherId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}