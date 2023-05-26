using MicroFinance.Models.DepositSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroFinance.Configuration.DepositSetup
{
    public class DepositAccountConfiguration : IEntityTypeConfiguration<DepositAccount>
    {
        public void Configure(EntityTypeBuilder<DepositAccount> builder)
        {
            builder.HasOne(da=>da.DepositScheme)
            .WithMany(ds=>ds.DepositAccounts)
            .HasForeignKey(da=>da.DepositSchemeId)
            .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(da=>da.Client)
            .WithMany(c=>c.DepositAccountSelf)
            .HasForeignKey(da=>da.ClientId)
            .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(da=>da.JointClient)
            .WithMany(c=>c.DepositAccountJoint)
            .HasForeignKey(da=>da.JointClientId)
            .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasIndex(da=>da.AccountNumber).IsUnique();
        }
    }
}