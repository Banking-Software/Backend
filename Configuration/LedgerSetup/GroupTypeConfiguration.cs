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
            builder.HasIndex(gt=>gt.CharKhataNumber).IsUnique();
            builder.Property(gt=>gt.Name).HasConversion(name=>name.ToUpper(), name=>name);

            //  builder.HasOne(gt=>gt.DebitOrCredit)
            //  .WithMany(dc=>dc.GroupTypes)
            //  .HasForeignKey(gt=>gt.DebitOrCreditId)
            //  .IsRequired()
            //  .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}