using MicroFinance.Models.AccountSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroFinance.Configuration.LedgerSetup
{
    public class AccountTypeConfiguration : IEntityTypeConfiguration<AccountType>
    {
        public void Configure(EntityTypeBuilder<AccountType> builder)
        {
            builder.HasIndex(at => at.Name).IsUnique();

            builder.Property(at=>at.Id).ValueGeneratedNever();

            builder.HasMany(at => at.GroupType)
            .WithOne(gt => gt.AccountType)
            .HasForeignKey(gt => gt.AccountTypeId)
            .OnDelete(DeleteBehavior.Cascade);

        }
    }
}