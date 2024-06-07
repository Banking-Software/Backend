using MicroFinance.Models.LoanSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroFinance.Configuration.LoanSetup;

public class LoanAccountConfiguration : IEntityTypeConfiguration<LoanAccount>
{
    public void Configure(EntityTypeBuilder<LoanAccount> builder)
    {
        builder.HasKey(la=>la.Id);
        builder.Property(la=>la.Id).ValueGeneratedOnAdd();
        builder.HasIndex(la=>la.AccountNumber).IsUnique();
        builder.Property(la=>la.AccountNumber).IsRequired(true);
        builder.Property(la=>la.WithDrawalBlockedDepositAccountIds)
        .HasConversion(
            v => string.Join(",", v),
            v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                   .Select(int.Parse)
                   .ToList()     
        );

        builder.Property(la=>la.LoanLimit).HasPrecision(18,2).IsRequired(true);
        builder.Property(la=>la.InterestRate).HasPrecision(5,2).IsRequired(true);

        builder.HasOne(la=>la.LoanScheme)
        .WithMany(ls=>ls.LoanAccounts)
        .HasForeignKey(la=>la.LoanSchemeId)
        .OnDelete(DeleteBehavior.ClientSetNull);

        builder.HasOne(la=>la.Client)
        .WithMany(c=>c.LoanAccounts)
        .HasForeignKey(la=>la.ClientId)
        .OnDelete(DeleteBehavior.ClientSetNull);
    }
}