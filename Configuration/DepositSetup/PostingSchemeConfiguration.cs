using MicroFinance.Models.DepositSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroFinance.Configuration.DepositSetup
{
    public class PostingSchemeConfiguration : IEntityTypeConfiguration<PostingScheme>
    {
        public void Configure(EntityTypeBuilder<PostingScheme> builder)
        {
            builder.HasIndex(ps=>ps.Name).IsUnique();

            builder.Property(ps=>ps.Id).ValueGeneratedNever();
        }
    }
}