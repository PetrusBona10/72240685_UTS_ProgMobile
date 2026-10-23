using AcademicApi.Models;

namespace AcademicApi.Data
{
    public interface IMahasiswaDAL
    {
        Task<IEnumerable<Mahasiswa>> GetAllAsync();

        Task<Mahasiswa?> GetByIdAsync(int id);

        Task<bool> NimExistsAsync(string nim, int? excludeMahasiswaId = null);

        Task<bool> DosenExistsAsync(int dosenId);

        Task<Mahasiswa> AddAsync(Mahasiswa mahasiswa);

        Task UpdateAsync(Mahasiswa mahasiswa);

        Task DeleteAsync(int id);
    }
}
