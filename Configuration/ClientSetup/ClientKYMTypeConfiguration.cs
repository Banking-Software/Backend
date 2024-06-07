using MicroFinance.Models.ClientSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroFinance.Configuration.ClientSetup
{
    public class ClientKYMTypeConfiguration : IEntityTypeConfiguration<ClientKYMType>
    {
        public void Configure(EntityTypeBuilder<ClientKYMType> builder)
        {
            builder.HasIndex(kym=>kym.Type).IsUnique();
            builder.Property(kym=>kym.Id).ValueGeneratedNever();
        }
    }
}