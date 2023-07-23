using MicroFinance.Models.DepositSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroFinance.Configuration.DepositSetup
{
    public class DepositAccountConfiguration : IEntityTypeConfiguration<DepositAccount>
    {
        public void Configure(EntityTypeBuilder<DepositAccount> builder)
        {
            builder.HasKey(da=>da.Id);
            builder.Property(da=>da.BranchCode).IsRequired(true);
            builder.Property(da=>da.Id).ValueGeneratedOnAdd();
            builder.Property(da=>da.InterestRate).HasPrecision(5,2).IsRequired(true);
            builder.Property(da=>da.PrincipalAmount).HasPrecision(18,4);
            builder.Property(da=>da.InterestAmount).HasPrecision(18, 4);
            builder.HasOne(da=>da.DepositScheme)
            .WithMany(ds=>ds.DepositAccounts)
            .HasForeignKey(da=>da.DepositSchemeId)
            .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(da=>da.Client)
            .WithMany(c=>c.DepositAccountSelf)
            .HasForeignKey(da=>da.ClientId)
            .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(da=>da.InterestPostingAccountNumber)
            .WithMany()
            .HasForeignKey(da=>da.InterestPostingAccountNumberId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(da=>da.MatureInterestPostingAccountNumber)
            .WithMany()
            .HasForeignKey(da=>da.MatureInterestPostingAccountNumberId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasIndex(da=>da.AccountNumber).IsUnique();
        }
    }
}