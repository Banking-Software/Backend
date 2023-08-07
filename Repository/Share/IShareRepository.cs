using System.Linq.Expressions;
using MicroFinance.Models.ClientSetup;
using MicroFinance.Models.Share;
using MicroFinance.Models.Wrapper;

namespace MicroFinance.Repository.Share
{
    public interface IShareRepository
    {
        Task<int> CreateShareAccount(Client client);
        Task<int> UpdateShareAccount(ShareAccount shareAccount);
        Task<ShareAccountWrapper> GetShareAccount(Expression<Func<ShareAccount, bool>> expression);
        Task<List<ShareAccountWrapper>> GetAllActiveShareAccount();

        Task<int> CreateShareKitta(ShareKitta shareKitta);
        Task<int> UpdateShareKitta(ShareKitta shareKitta);
        Task<ShareKitta> GetShareKitta();
    }
}