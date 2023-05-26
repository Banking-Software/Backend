using MicroFinance.Models.AccountSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroFinance.Configuration.LedgerSetup
{
    public class GroupTypeAndLedgerMapConfiguration : IEntityTypeConfiguration<GroupTypeAndLedgerMap>
    {
        public void Configure(EntityTypeBuilder<GroupTypeAndLedgerMap> builder)
        {
            builder.HasIndex(agl => new { agl.GroupTypeId, agl.LedgerId })
            .IsUnique();
        }
    }
}