using MicroFinance.Models.AccountSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroFinance.Configuration.LedgerSetup
{
    public class GroupTypeConfiguration : IEntityTypeConfiguration<GroupType>
    {
        public void Configure(EntityTypeBuilder<GroupType> builder)
        {
             builder.HasIndex(gt => new { gt.AccountTypeId, gt.Name }).IsUnique();

            builder.HasMany(gt => gt.GroupTypeAndLedgerMap)
            .WithOne(gtl => gtl.GroupType)
            .HasForeignKey(gtl => gtl.GroupTypeId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(gt => gt.GroupTypeDetails)
            .WithOne(gtd => gtd.GroupType)
            .HasForeignKey(gtd => gtd.GroupTypeId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}