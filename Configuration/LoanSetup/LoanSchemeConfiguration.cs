using MicroFinance.Models.LoanSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroFinance.Configuration.LoanSetup 
{
    public class LoanSchemeConfiguration : IEntityTypeConfiguration<LoanScheme>
    {
        public void Configure(EntityTypeBuilder<LoanScheme> builder)
        {
            builder.HasKey(ls=>ls.Id);
            builder.Property(ls=>ls.Id).ValueGeneratedOnAdd();
            builder.HasIndex(ls=>ls.Name).IsUnique();
            builder.HasIndex(ls=>ls.AliasCode).IsUnique();
            builder.Property(ls=>ls.InterestRate).HasPrecision(5,2).IsRequired(true);
            builder.Property(ls=>ls.MinimumInterestRate).HasPrecision(5,2).IsRequired(true);
            builder.Property(ls=>ls.MaximumInterestRate).HasPrecision(5,2).IsRequired(true);
            builder.Property(ls=>ls.PenalInterest).HasPrecision(5,2).IsRequired(true);
            builder.Property(ls=>ls.InterestOnInterest).HasPrecision(5,2).IsRequired(true);
            builder.Property(ls=>ls.LoanInterestReceivable).HasPrecision(5,2).IsRequired(true);
            builder.Property(ls=>ls.OverDueInterest).HasPrecision(5,2).IsRequired(true);


            builder.HasOne(ls=>ls.AssetsAccountLedger)
            .WithOne(l=>l.AssetsLoanScheme)
            .HasForeignKey<LoanScheme>(ls=>ls.AssetsAccountLedgerId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(ls=>ls.InterestAccountLedger)
            .WithOne(l=>l.InterestLoanSchme)
            .HasForeignKey<LoanScheme>(ls=>ls.InterestAccountLedgerId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}