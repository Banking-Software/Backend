using MicroFinance.DBContext;
using MicroFinance.Models.RecordsWithCode;
using Microsoft.EntityFrameworkCore;

namespace MicroFinance.Repository.RecordsWithCode
{
    public class RecordsWithCodeRepository : IRecordsWithCodeRepository
    {
        private readonly ApplicationDbContext _recordWithCodeDbContext;

        public RecordsWithCodeRepository(ApplicationDbContext recordWithCodeDbContext)
        {
            _recordWithCodeDbContext= recordWithCodeDbContext;
        }
        public async Task<List<Cast>> GetAllCastsDetail()
        {
            return await _recordWithCodeDbContext.Casts.ToListAsync();
        }

        public async Task<List<District>> GetAllDistrictsDetail()
        {
            return await _recordWithCodeDbContext.Districts.ToListAsync();
        }

        public async Task<List<Gender>> GetAllGendersDetail()
        {
            return await _recordWithCodeDbContext.Genders.ToListAsync();
        }

        public async Task<List<MaritalStatus>> GetAllMaritalStatusDetail()
        {
            return await _recordWithCodeDbContext.MaritalStatuses.ToListAsync();
        }

        public async Task<List<State>> GetAllStatesDetail()
        {
            return await _recordWithCodeDbContext.States.ToListAsync();
        }
    }
}