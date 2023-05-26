using MicroFinance.DBContext;
using MicroFinance.Models.ClientSetup;

namespace MicroFinance.SeedData  
{
    public class ClientDbContextSeed
    {
        public static async Task SeedClientInfoAsync(ApplicationDbContext context)
        {
            if (!context.ClientAccountTypeInfos.Any())
            {
                await context.ClientAccountTypeInfos.AddRangeAsync(new List<ClientAccountTypeInfo>
                {
                        new ClientAccountTypeInfo {Type="Share Holder"},
                        new ClientAccountTypeInfo {Type="Ideal Share Holder"}
                });
                await context.SaveChangesAsync();
            }

            if (!context.ClientTypeInfos.Any())
            {
                await context.ClientTypeInfos.AddRangeAsync(new List<ClientTypeInfo>
                {
                        new ClientTypeInfo {Type="Indivisual"},
                        new ClientTypeInfo {Type="Organization"},
                        new ClientTypeInfo {Type="Minor"}
                });
                await context.SaveChangesAsync();
            }

            if (!context.ClientKYMTypeInfos.Any())
            {
                await context.ClientKYMTypeInfos.AddRangeAsync(new List<ClientKYMTypeInfo>
                {
                        new ClientKYMTypeInfo {Type="High Risk"},
                        new ClientKYMTypeInfo {Type="Medium Risk"},
                        new ClientKYMTypeInfo {Type="Low Risk"}
                });
                await context.SaveChangesAsync();
            }

            if (!context.ClientShareTypeInfos.Any())
            {
                await context.ClientShareTypeInfos.AddRangeAsync(new List<ClientShareTypeInfo>
                {
                        new ClientShareTypeInfo {Type="Ordinary Share"},
                        new ClientShareTypeInfo {Type="Promoter Share"}
                });
                await context.SaveChangesAsync();
            }

        }
    }
}