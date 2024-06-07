using MicroFinance.Models.DepositSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroFinance.Configuration.DepositSetup
{
    public class JointAccountConfiguration : IEntityTypeConfiguration<JointAccount>
    {
        public void Configure(EntityTypeBuilder<JointAccount> builder)
        {
            builder.HasOne(ja=>ja.JointClient)
            .WithMany(c=>c.JointAccounts)
            .HasForeignKey(ja=>ja.JointClientId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(ja=>ja.DepositAccount).WithMany(da=>da.JointAccounts)
            .HasForeignKey(ja=>ja.DepositAccountId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}