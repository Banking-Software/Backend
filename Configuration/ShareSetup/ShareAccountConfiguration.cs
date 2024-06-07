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
            builder.Property(sk=>sk.CurrentShareBalance).HasPrecision(18,2).IsRequired(true);
            builder.HasOne(sa=>sa.Client)
            .WithOne(c=>c.ShareAccount)
            .HasForeignKey<ShareAccount>(sa=>sa.ClientId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}