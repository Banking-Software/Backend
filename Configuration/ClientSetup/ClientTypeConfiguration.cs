using MicroFinance.Models.ClientSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroFinance.Configuration.ClientSetup
{
    public class ClientTypeConfiguration : IEntityTypeConfiguration<ClientType>
    {
        public void Configure(EntityTypeBuilder<ClientType> builder)
        {
            builder.Property(ct=>ct.Id).ValueGeneratedNever();
            builder.HasIndex(ct=>ct.Type).IsUnique();
        }
    }
}