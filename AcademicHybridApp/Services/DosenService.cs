using AcademicHybridApp.Models;

namespace AcademicHybridApp.Services
{
    public class DosenService : ApiServiceBase, IDosenService
    {
        private const string BasePath = "api/dosen";

        public DosenService(HttpClient httpClient)
            : base(httpClient)
        {
        }

        public Task<ServiceResult<List<DosenModel>>> GetAllAsync()
        {
            return GetListAsync<DosenModel>(BasePath);
        }

        public Task<ServiceResult<DosenModel>> GetByIdAsync(int id)
        {
            return GetItemAsync<DosenModel>($"{BasePath}/{id}");
        }

        public Task<ServiceResult<DosenModel>> CreateAsync(DosenFormModel form)
        {
            return CreateItemAsync<DosenFormModel, DosenModel>(
                BasePath,
                form,
                "Dosen berhasil ditambahkan.");
        }

        public Task<ServiceResult> UpdateAsync(int id, DosenFormModel form)
        {
            return UpdateItemAsync($"{BasePath}/{id}", form, "Dosen berhasil diperbarui.");
        }

        public Task<ServiceResult> DeleteAsync(int id)
        {
            return DeleteItemAsync($"{BasePath}/{id}", "Dosen berhasil dihapus.");
        }
    }
}
