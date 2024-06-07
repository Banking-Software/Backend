using MicroFinance.Models.DepositSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroFinance.Configuration.DepositSetup
{
    public class DepositSchemeConfiguration : IEntityTypeConfiguration<DepositScheme>
    {
        public void Configure(EntityTypeBuilder<DepositScheme> builder)
        {
            builder.HasKey(ds=>ds.Id);
            builder.Property(da=>da.BranchCode).IsRequired(true);
            builder.Property(ds=>ds.Id).ValueGeneratedOnAdd();
            builder.HasIndex(ds=>ds.SchemeName).IsUnique();
            builder.HasIndex(ds=>ds.Symbol).IsUnique();
            builder.Property(ds=>ds.InterestRateOnMinimumBalance).HasPrecision(5,2).IsRequired(true);
            builder.Property(ds=>ds.InterestRate).HasPrecision(5,2).IsRequired(true);
            builder.Property(ds=>ds.MinimumInterestRate).HasPrecision(5,2).IsRequired(true);
            builder.Property(ds=>ds.MaximumInterestRate).HasPrecision(5,2).IsRequired(true);

            builder.HasOne(ds=>ds.SchemeType).WithMany(l=>l.DepositSchemes)
            .HasForeignKey(ds=>ds.SchemeTypeId).IsRequired(true).OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(ds=>ds.DepositSubLedger).WithOne(sl=>sl.DepositSchemes)
            .HasForeignKey<DepositScheme>(ds=>ds.DepositSubledgerId).IsRequired(true).OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(ds=>ds.InterestSubledger).WithOne(sl=>sl.InterestSchemes)
            .HasForeignKey<DepositScheme>(ds=>ds.InterestSubLedgerId).IsRequired(true).OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(ds=>ds.TaxSubledger).WithOne(sl=>sl.TaxSchemes)
            .HasForeignKey<DepositScheme>(ds=>ds.TaxSubledgerId).IsRequired(true).OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}