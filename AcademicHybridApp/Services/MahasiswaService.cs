using AcademicHybridApp.Models;
using System.Net.Http.Json;

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

        public async Task<ServiceResult<List<MahasiswaModel>>> GetFilteredAsync(
            string? search = null,
            string? sortBy = null,
            string? sortDir = null,
            int? page = null,
            int? pageSize = null)
        {
            var query = new List<string>();
            if (!string.IsNullOrWhiteSpace(search))
            {
                query.Add($"search={Uri.EscapeDataString(search.Trim())}");
            }
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                query.Add($"sortBy={Uri.EscapeDataString(sortBy.Trim())}");
            }
            if (!string.IsNullOrWhiteSpace(sortDir))
            {
                query.Add($"sortDir={Uri.EscapeDataString(sortDir.Trim())}");
            }
            if (page.HasValue)
            {
                query.Add($"page={page.Value}");
            }
            if (pageSize.HasValue)
            {
                query.Add($"pageSize={pageSize.Value}");
            }

            var url = query.Count == 0 ? BasePath : $"{BasePath}?{string.Join("&", query)}";

            try
            {
                var response = await HttpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                var errorMessage = await ApiErrorReader.ReadMessageAsync(response);
                return ServiceResult<List<MahasiswaModel>>.Fail(errorMessage);
                }

                var data = await response.Content.ReadFromJsonAsync<List<MahasiswaModel>>() ?? new List<MahasiswaModel>();
                var totalCount = 0;
                if (response.Headers.TryGetValues("X-Total-Count", out var values))
                {
                    var header = values.FirstOrDefault();
                    int.TryParse(header, out totalCount);
                }
                else if (totalCount == 0 && !page.HasValue)
                {
                    totalCount = data.Count;
                }

                var result = ServiceResult<List<MahasiswaModel>>.Ok(data);
                result.TotalCount = totalCount;
                return result;
            }
            catch (Exception ex)
            {
                return ServiceResult<List<MahasiswaModel>>.Fail(ex.Message, null);
            }
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
