using System.Linq.Expressions;
using MicroFinance.DBContext;
using MicroFinance.Enums.Client;
using MicroFinance.Helper;
using MicroFinance.Models.ClientSetup;
using MicroFinance.Models.Share;
using MicroFinance.Models.Wrapper;
using Microsoft.EntityFrameworkCore;

namespace MicroFinance.Repository.Share
{
    public class ShareRepository : IShareRepository
    {
        private readonly ILogger<ShareRepository> _logger;
        private readonly ApplicationDbContext _dbContext;

        public ShareRepository(ILogger<ShareRepository> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext=dbContext;
        }
        public async Task<int> CreateShareAccount(Client client)
        {
            // var client =  await _dbContext.Clients.FindAsync(clientId);
            ShareAccount shareAccount = new ShareAccount()
            {
                Client = client,
                IsActive=true
            };
            await _dbContext.ShareAccounts.AddAsync(shareAccount);
            var status =  await _dbContext.SaveChangesAsync();
            if(status<1) throw new NotImplementedException("Unable to create Share Account");
            return shareAccount.Id;
        }
        public async Task<int> UpdateShareAccount(ShareAccount shareAccount)
        {
            string closeStatement = shareAccount.IsClose?"closing":"opening";
            string activeStatement = shareAccount.IsActive?"activating":"deactivating";
            _logger.LogInformation($"{DateTime.Now}: Share Account of client with Id '{shareAccount.ClientId} is {closeStatement}...");
            _logger.LogInformation($"{DateTime.Now}: Share Account of client with Id '{shareAccount.ClientId} is {activeStatement}...");
            int result = await _dbContext.SaveChangesAsync();
            if(result<1) throw new Exception("Unable to update Share Account");
            return result;
        }

        public async Task<List<ShareAccountWrapper>> GetAllActiveShareAccount()
        {
            var allActiveShareAccounts = await _dbContext.ShareAccounts
            .Include(sa=>sa.Client)
            .Where(sa=>sa.IsActive)
            .Select(sa=>new ShareAccountWrapper()
            {
                Id=sa.Id,
                ClientId = (int) sa.ClientId,
                ClientName = $"{sa.Client.ClientFirstName} {sa.Client.ClientLastName}",
                CurrentShareBalance = sa.CurrentShareBalance,
                IsActive = sa.IsActive,
                ShareType = (ShareTypeEnum) sa.Client.ClientShareTypeInfoId
            })
            .AsNoTracking().ToListAsync();
            return allActiveShareAccounts;
        }

        public async Task<ShareAccountWrapper> GetShareAccount(Expression<Func<ShareAccount, bool>> expression)
        {
           var shareAccount = await _dbContext.ShareAccounts
           .Include(sa=>sa.Client)
           .Where(expression)
           .Select(sa=>new ShareAccountWrapper()
            {
                Id=sa.Id,
                ClientId = (int) sa.ClientId,
                ClientName = $"{sa.Client.ClientFirstName} {sa.Client.ClientLastName}",
                CurrentShareBalance = sa.CurrentShareBalance,
                IsActive = sa.IsActive,
                ShareType = (ShareTypeEnum) sa.Client.ClientShareTypeInfoId
            })
           .AsNoTracking()
           .FirstOrDefaultAsync();
           return shareAccount;
        }

        public async Task<int> CreateShareKitta(ShareKitta shareKitta)
        {
            await _dbContext.ShareKittas.AddAsync(shareKitta);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateShareKitta(ShareKitta shareKitta)
        {
            SemaphoreSlim shareKittaLock = LockManager.Instance.GetShareKittaLock(shareKitta.Id);
            await shareKittaLock.WaitAsync();
            try
            {
                var existingShareKitta = await _dbContext.ShareKittas.FindAsync(shareKitta.Id);
                existingShareKitta.PriceOfOneKitta = shareKitta.PriceOfOneKitta;
                var status =await _dbContext.SaveChangesAsync();
                if(status<0) throw new Exception("Unable to create Share Kitta");
                return shareKitta.Id;
            }
            catch(Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: {ex.Message}. {ex.InnerException}");
                throw new Exception(ex.Message);
            }
            finally
            {
                shareKittaLock.Release();
            }
            
        }

        public async Task<ShareKitta> GetShareKitta()
        {
            var activeShareKitta = await _dbContext.ShareKittas.Where(sk=>sk.IsActive).AsNoTracking().FirstOrDefaultAsync();
            return activeShareKitta;
        }
    }
}