using MicroFinance.Models.Share;

namespace MicroFinance.Repository.Share
{
    public interface IShareRepository
    {
        Task<int> CreateShareAccount(int clientId);
        Task<int> UpdateShareAccount(ShareAccount shareAccount);
        Task<List<ShareAccount>> GetAllShareAccountByAccountNumber(int accountNumber);
        Task<List<ShareAccount>> GetAllShareAccount();
        Task<ShareAccount> GetActiveShareAccountByAccountNuber(int accountNumber);
        Task<List<ShareAccount>> GetAllActiveShareAccount();
    }
}