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
        }
    }
}