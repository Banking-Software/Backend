using MicroFinance.Models.CompanyProfile;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroFinance.Configuration.CompanyProfile
{
    public class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.HasIndex(b=>b.BranchCode).IsUnique();
            //builder.Property(b=>b.CreatedOn).HasColumnType("date");
        }
    }
}