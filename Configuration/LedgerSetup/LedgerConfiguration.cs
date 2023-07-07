using System.Reflection;
using MicroFinance.Models.AccountSetup;
using MicroFinance.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroFinance.Configuration.LedgerSetup
{
    public class LedgerConfiguration : IEntityTypeConfiguration<Ledger>
    {
        public void Configure(EntityTypeBuilder<Ledger> builder)
        {
            // builder.Property(at=>at.Id).ValueGeneratedNever();
            
            builder.HasKey(l=>l.Id);
            builder.Property(l=>l.Id).ValueGeneratedOnAdd();

            builder.HasIndex(l => new { l.GroupTypeId, l.Name }).IsUnique();
            builder.HasIndex(l=>l.LedgerCode).IsUnique();
            builder.Property(l=>l.Name).HasConversion(name=>name.ToUpper(), name=>name);

            builder.HasOne(l => l.GroupType)
             .WithMany(gt => gt.Ledgers)
             .HasForeignKey(l=>l.GroupTypeId)
             .IsRequired()
             .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(l => l.SubLedger)
            .WithOne(sl => sl.Ledger)
            .HasForeignKey(sl => sl.LedgerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}