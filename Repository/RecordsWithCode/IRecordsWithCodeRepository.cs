using MicroFinance.Models.RecordsWithCode;

namespace MicroFinance.Repository.RecordsWithCode
{
    public interface IRecordsWithCodeRepository
    {
        Task<List<Cast>> GetAllCastsDetail();
        Task<List<District>> GetAllDistrictsDetail();
        Task<List<State>> GetAllStatesDetail();
        Task<List<Gender>> GetAllGendersDetail();
        Task<List<MaritalStatus>> GetAllMaritalStatusDetail();
        
    }
}