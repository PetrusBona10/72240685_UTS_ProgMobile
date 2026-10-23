using AcademicHybridApp.Models;

namespace AcademicHybridApp.Services
{
    public interface IMahasiswaService
    {
        Task<ServiceResult<List<MahasiswaModel>>> GetAllAsync();

        Task<ServiceResult<MahasiswaModel>> GetByIdAsync(int id);

        Task<ServiceResult<MahasiswaModel>> CreateAsync(MahasiswaFormModel form);

        Task<ServiceResult> UpdateAsync(int id, MahasiswaFormModel form);

        Task<ServiceResult> DeleteAsync(int id);
    }
}
