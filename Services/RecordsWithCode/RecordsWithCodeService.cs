using AutoMapper;
using MicroFinance.Dtos.RecordsWithCode;
using MicroFinance.Repository.RecordsWithCode;

namespace MicroFinance.Services.RecordsWithCode
{
    public class RecordsWithCodeService : IRecordsWithCodeService
    {
        private readonly IRecordsWithCodeRepository _recordsWithCodeRepository;
        private readonly IMapper _mapper;

        public RecordsWithCodeService(IRecordsWithCodeRepository recordsWithCodeRepository, IMapper mapper)
        {
            _recordsWithCodeRepository=recordsWithCodeRepository;
            _mapper = mapper;
        }
        public async Task<List<CastDto>> GetAllCastsDetailService()
        {
            var allCast = await _recordsWithCodeRepository.GetAllCastsDetail();
            if(allCast.Any())
            {
                return _mapper.Map<List<CastDto>>(allCast);
            }
            throw new NotImplementedException("No Cast Details Found");
        }

        public async Task<List<DistrictDto>> GetAllDistrictsDetailService()
        {
            var allDistrict = await _recordsWithCodeRepository.GetAllDistrictsDetail();
            if(allDistrict.Any())
                return _mapper.Map<List<DistrictDto>>(allDistrict);
            throw new NotImplementedException("No District Details Found");
        }

        public async Task<List<GenderDto>> GetAllGendersDetailService()
        {
            var allGenders = await _recordsWithCodeRepository.GetAllGendersDetail();
            if(allGenders.Any())
                return _mapper.Map<List<GenderDto>>(allGenders);
            throw new NotImplementedException();
        }

        public async Task<List<MaritalStatusDto>> GetAllMartialStatusDetailService()
        {
            var allMartialStatus = await _recordsWithCodeRepository.GetAllMaritalStatusDetail();
            if(allMartialStatus.Any())
                return _mapper.Map<List<MaritalStatusDto>>(allMartialStatus);
            throw new NotImplementedException("No Marital Status Found");
        }

        public async Task<List<StateDto>> GetAllStatesDetailsService()
        {
            var allStates = await _recordsWithCodeRepository.GetAllStatesDetail();
            if(allStates.Any())
                return _mapper.Map<List<StateDto>>(allStates);
            throw new NotImplementedException("No States Data Found");
        }
        public async Task<RecordsWithCodeDto> GetAllRecordWithCodeService()
        {
            var allCast = await GetAllCastsDetailService();
            var allDistrict = await GetAllDistrictsDetailService();
            var allGenders = await GetAllGendersDetailService();
            var allMaritalStatus = await GetAllMartialStatusDetailService();
            var allStates = await GetAllStatesDetailsService();
            RecordsWithCodeDto allRecordsWithCode = new RecordsWithCodeDto()
            {
                Cast = allCast,
                District = allDistrict,
                Gender = allGenders,
                MaritalStatus=allMaritalStatus,
                State = allStates
            };
            return allRecordsWithCode;
        }

    }
}