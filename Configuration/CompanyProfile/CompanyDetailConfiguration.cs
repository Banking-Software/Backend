using MicroFinance.Models.CompanyProfile;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroFinance.Configuration.CompanyProfile
{
    public class CompanyDetailConfiguration : IEntityTypeConfiguration<CompanyDetail>
    {
        public void Configure(EntityTypeBuilder<CompanyDetail> builder)
        {
            builder.HasKey(cd=>cd.Id);
            builder.Property(cd=>cd.Id).ValueGeneratedOnAdd();
            builder.Property(cd=>cd.CurrentTax).HasPrecision(4, 2);
        }
    }
}