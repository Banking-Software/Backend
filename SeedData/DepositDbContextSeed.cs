using MicroFinance.DBContext;
using MicroFinance.Enums;
using MicroFinance.Enums.Deposit.Account;
using MicroFinance.Enums.Deposit.Scheme;
using MicroFinance.Models.DepositSetup;
using MicroFinance.Models.DepositSetup.HelperTable;

namespace MicroFinance.SeedData
{
    public class DepositDbContextSeed
    {

        public static async Task SeedDepositAsync(ApplicationDbContext depositContext)
        {
            await SeedPostSchemeAsync(depositContext);
            await SeedDepositAccountStatus(depositContext);
            await SeedDepositAccountType(depositContext);
            await SeedDepositSchemeCalculationType(depositContext);
            //await depositContext.SaveChangesAsync();
        }
        private static async Task SeedPostSchemeAsync(ApplicationDbContext depositDbContext)
        {
            if (!depositDbContext.DepositPostingSchemes.Any())
            {
                await depositDbContext.DepositPostingSchemes.AddRangeAsync(new List<DepositPostingScheme>
                {
                        new DepositPostingScheme {Id=(int)PostingSchemeEnum.Yearly, Title=PostingSchemeEnum.Yearly.ToString()},
                        new DepositPostingScheme {Id=(int)PostingSchemeEnum.HalfYearly, Title=PostingSchemeEnum.HalfYearly.ToString()},
                        new DepositPostingScheme {Id=(int)PostingSchemeEnum.Quarterly, Title=PostingSchemeEnum.Quarterly.ToString()},
                        new DepositPostingScheme {Id=(int)PostingSchemeEnum.Monthly, Title=PostingSchemeEnum.Monthly.ToString()},
                        new DepositPostingScheme {Id=(int)PostingSchemeEnum.None, Title=PostingSchemeEnum.None.ToString()}
                });
                await depositDbContext.SaveChangesAsync();
            }
        }

        private static async Task SeedDepositAccountStatus(ApplicationDbContext depositDbContext)
        {
            if (!depositDbContext.DepositAccountStatuses.Any())
            {
                await depositDbContext.DepositAccountStatuses.AddRangeAsync(new List<DepositAccountStatus>
                {
                    new DepositAccountStatus {Id=(int)AccountStatusEnum.Active, Title=AccountStatusEnum.Active.ToString()},
                    new DepositAccountStatus {Id=(int)AccountStatusEnum.Mature, Title=AccountStatusEnum.Mature.ToString()},
                    new DepositAccountStatus {Id=(int)AccountStatusEnum.Suspend, Title=AccountStatusEnum.Suspend.ToString()},
                    new DepositAccountStatus {Id=(int)AccountStatusEnum.Close, Title=AccountStatusEnum.Close.ToString()},

                });
                await depositDbContext.SaveChangesAsync();
            }
        }

        private static async Task SeedDepositAccountType(ApplicationDbContext depositDbContext)
        {
            if (!depositDbContext.DepositAccountTypes.Any())
            {
                await depositDbContext.DepositAccountTypes.AddRangeAsync(new List<DepositAccountType>
                {
                    new DepositAccountType {Id=(int)AccountTypeEnum.Single, Title=AccountTypeEnum.Single.ToString()},
                    new DepositAccountType {Id=(int)AccountTypeEnum.Joint, Title=AccountTypeEnum.Joint.ToString()},
                });
                await depositDbContext.SaveChangesAsync();
            }
        }

        private static async Task SeedDepositSchemeCalculationType(ApplicationDbContext depositDbContext)
        {
            if(!depositDbContext.DepositSchemeCalculationTypes.Any())
            {
                await depositDbContext.DepositSchemeCalculationTypes.AddRangeAsync(new List<DepositSchemeCalculationType>
                {
                    new DepositSchemeCalculationType {Id=(int)CalculationTypeEnum.DailyBalance, Title=CalculationTypeEnum.DailyBalance.ToString()},
                    new DepositSchemeCalculationType {Id=(int)CalculationTypeEnum.DailyMinimumBalance, Title=CalculationTypeEnum.DailyMinimumBalance.ToString()},
                    new DepositSchemeCalculationType {Id=(int)CalculationTypeEnum.WeeklyMinimumBalance, Title=CalculationTypeEnum.WeeklyMinimumBalance.ToString()},
                    new DepositSchemeCalculationType {Id=(int)CalculationTypeEnum.Fornight, Title=CalculationTypeEnum.Fornight.ToString()},
                    new DepositSchemeCalculationType {Id=(int)CalculationTypeEnum.MonthlyMinimumBalance, Title=CalculationTypeEnum.MonthlyMinimumBalance.ToString()},
                    new DepositSchemeCalculationType {Id=(int)CalculationTypeEnum.Flexible, Title=CalculationTypeEnum.Flexible.ToString()},
                    new DepositSchemeCalculationType {Id=(int)CalculationTypeEnum.None, Title=CalculationTypeEnum.None.ToString()},
                });
            }
            await depositDbContext.SaveChangesAsync();
        }

    }
}