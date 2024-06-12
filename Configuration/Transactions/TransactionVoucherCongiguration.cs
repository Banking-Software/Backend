using MicroFinance.Models.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroFinance.Configuration.Transactions   
{
    public class TransactionVoucherCongiguration : IEntityTypeConfiguration<TransactionVoucher>
    {
        public void Configure(EntityTypeBuilder<TransactionVoucher> builder)
        {
            builder.HasKey(tv=>tv.Id);
            builder.Property(tv=>tv.Id).ValueGeneratedOnAdd();
            builder.HasIndex(tv=>tv.VoucherNumber).IsUnique();
        }
    }
}