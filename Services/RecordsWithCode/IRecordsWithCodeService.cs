using MicroFinance.Dtos.RecordsWithCode;

namespace MicroFinance.Services.RecordsWithCode
{
    public interface IRecordsWithCodeService
    {
        Task<List<CastDto>> GetAllCastsDetailService();
        Task<List<DistrictDto>> GetAllDistrictsDetailService();
        Task<List<MaritalStatusDto>> GetAllMartialStatusDetailService();
        Task<List<GenderDto>> GetAllGendersDetailService();
        Task<List<StateDto>> GetAllStatesDetailsService();
        Task<RecordsWithCodeDto> GetAllRecordWithCodeService();
        
    }
}