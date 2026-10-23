using AcademicHybridApp.Models;

namespace AcademicHybridApp.Services
{
    public class MahasiswaService : ApiServiceBase, IMahasiswaService
    {
        private const string BasePath = "api/mahasiswa";

        public MahasiswaService(HttpClient httpClient)
            : base(httpClient)
        {
        }

        public Task<ServiceResult<List<MahasiswaModel>>> GetAllAsync()
        {
            return GetListAsync<MahasiswaModel>(BasePath);
        }

        public Task<ServiceResult<MahasiswaModel>> GetByIdAsync(int id)
        {
            return GetItemAsync<MahasiswaModel>($"{BasePath}/{id}");
        }

        public Task<ServiceResult<MahasiswaModel>> CreateAsync(MahasiswaFormModel form)
        {
            return CreateItemAsync<MahasiswaFormModel, MahasiswaModel>(
                BasePath,
                form,
                "Mahasiswa berhasil ditambahkan.");
        }

        public Task<ServiceResult> UpdateAsync(int id, MahasiswaFormModel form)
        {
            return UpdateItemAsync($"{BasePath}/{id}", form, "Mahasiswa berhasil diperbarui.");
        }

        public Task<ServiceResult> DeleteAsync(int id)
        {
            return DeleteItemAsync($"{BasePath}/{id}", "Mahasiswa berhasil dihapus.");
        }
    }
}
