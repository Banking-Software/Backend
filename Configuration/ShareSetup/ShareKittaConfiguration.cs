using MicroFinance.Models.Share;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroFinance.Configuration.ShareSetup
{
    public class ShareKittaConfiguration : IEntityTypeConfiguration<ShareKitta>
    {
        public void Configure(EntityTypeBuilder<ShareKitta> builder)
        {
            builder.HasKey(sk=>sk.Id);
            builder.Property(sk=>sk.Id).ValueGeneratedOnAdd();
            builder.Property(sk=>sk.PriceOfOneKitta).HasPrecision(5,2).IsRequired(true);
            builder.Property(sk=>sk.CurrentKitta).HasPrecision(18,2).IsRequired(true);
        }
    }
}