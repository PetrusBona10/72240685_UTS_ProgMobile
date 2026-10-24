using AcademicHybridApp.Models;

namespace AcademicHybridApp.Services
{
    public interface IMahasiswaService
    {
        Task<ServiceResult<List<MahasiswaModel>>> GetAllAsync();

        Task<ServiceResult<List<MahasiswaModel>>> GetFilteredAsync(
            string? search = null,
            string? sortBy = null,
            string? sortDir = null,
            int? page = null,
            int? pageSize = null);

        Task<ServiceResult<MahasiswaModel>> GetByIdAsync(int id);

        Task<ServiceResult<MahasiswaModel>> CreateAsync(MahasiswaFormModel form);

        Task<ServiceResult> UpdateAsync(int id, MahasiswaFormModel form);

        Task<ServiceResult> DeleteAsync(int id);
    }
}
