using AcademicApi.Models;

namespace AcademicApi.Data
{
    public interface IDosenDAL
    {
        Task<IEnumerable<Dosen>> GetAllAsync();

        Task<Dosen?> GetByIdAsync(int id);

        Task<bool> NidnExistsAsync(string nidn, int? excludeDosenId = null);

        Task<Dosen> AddAsync(Dosen dosen);

        Task UpdateAsync(Dosen dosen);

        Task DeleteAsync(int id);

        Task<bool> HasMahasiswaAsync(int dosenId);
    }
}
