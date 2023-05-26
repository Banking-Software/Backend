using MicroFinance.Models.DepositSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroFinance.Configuration.DepositSetup
{
    public class DepositSchemeConfiguration : IEntityTypeConfiguration<DepositScheme>
    {
        public void Configure(EntityTypeBuilder<DepositScheme> builder)
        {
            builder.HasIndex(ds=>ds.Name).IsUnique();

            builder.HasOne(ds=>ds.PostingScheme)
            .WithMany(ps=>ps.DepositScheme)
            .HasForeignKey(ds=>ds.PostingSchemeId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(ds=>ds.LedgerAsLiabilityAccount)
            .WithOne(l=>l.LiabilityAccount)
            .HasForeignKey<DepositScheme>(ds=>ds.LedgerAsLiabilityAccountId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(ds=>ds.LedgerAsInterestAccount)
            .WithOne(l=>l.InterestAccount)
            .HasForeignKey<DepositScheme>(ds=>ds.LedgerAsInterestAccountId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}