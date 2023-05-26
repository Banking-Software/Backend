using MicroFinance.DBContext;
using MicroFinance.Models.DepositSetup;

namespace MicroFinance.SeedData
{
    public class DepositDbContextSeed
    {
        public static async Task SeedPostSchemeAsync(ApplicationDbContext depositDbContext)
        {
            if (!depositDbContext.PostingSchemes.Any())
            {
                await depositDbContext.PostingSchemes.AddRangeAsync(new List<PostingScheme>
                {
                        new PostingScheme {Id=1, Name="Yearly"},
                        new PostingScheme {Id=2, Name="Half-Yearly"},
                        new PostingScheme {Id=3, Name="Quarterly"},
                        new PostingScheme {Id=4, Name="Monthly"},
                        new PostingScheme {Id=5, Name="None"}
                });
                await depositDbContext.SaveChangesAsync();
            }
        }
    }
}