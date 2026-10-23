using AcademicHybridApp.Models;

namespace AcademicHybridApp.Services
{
    public interface IDosenService
    {
        Task<ServiceResult<List<DosenModel>>> GetAllAsync();

        Task<ServiceResult<DosenModel>> GetByIdAsync(int id);

        Task<ServiceResult<DosenModel>> CreateAsync(DosenFormModel form);

        Task<ServiceResult> UpdateAsync(int id, DosenFormModel form);

        Task<ServiceResult> DeleteAsync(int id);
    }
}
