using MicroFinance.Models.ClientSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroFinance.Configuration.ClientSetup
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasIndex(c=>c.ClientId).IsUnique();
            builder.HasOne(c=>c.ShareType).WithMany(l=>l.Client).HasForeignKey(c=>c.ClientShareTypeInfoId).IsRequired(false).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(c=>c.ClientType).WithMany(ct=>ct.Clients).HasForeignKey(c=>c.ClientTypeId).IsRequired(true).OnDelete(DeleteBehavior.ClientCascade);
            builder.HasOne(c=>c.ClientGroup).WithMany(cg=>cg.Clients).HasForeignKey(c=>c.ClientGroupId).IsRequired(false).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(c=>c.ClientUnit).WithMany(cu=>cu.Clients).HasForeignKey(c=>c.ClientUnitId).IsRequired(false).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(c=>c.KYMType).WithMany(cu=>cu.Clients).HasForeignKey(c=>c.KYMTypeId).IsRequired(false).OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}