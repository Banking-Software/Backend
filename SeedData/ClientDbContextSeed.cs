using MicroFinance.DBContext;
using MicroFinance.Models.ClientSetup;

namespace MicroFinance.SeedData  
{
    public class ClientDbContextSeed
    {
        public static async Task SeedClientInfoAsync(ApplicationDbContext context)
        {
            
            if (!context.ClientTypes.Any())
            {
                await context.ClientTypes.AddRangeAsync(new List<ClientType>
                {
                        new ClientType {Id=1, Type="Indivisual"},
                        new ClientType {Id=2, Type="Organization"},
                        new ClientType {Id=3, Type="Minor"}
                });
                await context.SaveChangesAsync();
            }

            if (!context.ClientKYMTypes.Any())
            {
                await context.ClientKYMTypes.AddRangeAsync(new List<ClientKYMType>
                {
                        new ClientKYMType {Id=1 ,Type="High Risk"},
                        new ClientKYMType {Id=2 ,Type="Medium Risk"},
                        new ClientKYMType {Id=3, Type="Low Risk"}
                });
                await context.SaveChangesAsync();
            }
            if(!context.ClientUnits.Any())
            {
                List<ClientUnit> clientUnits = new List<ClientUnit>();
                for (int i = 1; i < 31; i++)
                {
                    ClientUnit clientUnit = new ClientUnit(){Code =i};
                    clientUnits.Add(clientUnit);
                }
                await context.ClientUnits.AddRangeAsync(clientUnits);
            }
            if(!context.ClientGroups.Any())
            {
                List<ClientGroup> clientGroups = new List<ClientGroup>();
                for (int i = 65; i <= 90; i++)
                {
                    ClientGroup clientGroup = new ClientGroup(){Code=((char)i).ToString()};
                    clientGroups.Add(clientGroup);
                }
                await  context.ClientGroups.AddRangeAsync(clientGroups);
            }
        }
    }
}