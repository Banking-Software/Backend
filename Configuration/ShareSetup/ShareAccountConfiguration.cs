using MicroFinance.Models.Share;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroFinance.Configuration.ShareSetup
{
    public class ShareAccountConfiguration : IEntityTypeConfiguration<ShareAccount>
    {
        public void Configure(EntityTypeBuilder<ShareAccount> builder)
        {
            builder.HasKey(sa=>sa.Id);
            builder.Property(sa=>sa.Id).ValueGeneratedOnAdd();
            builder.HasOne(sa=>sa.Client)
            .WithMany(c=>c.ShareAccounts)
            .HasForeignKey(sa=>sa.AccountNumber)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}