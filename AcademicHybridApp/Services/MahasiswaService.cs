using System.Net.Http.Json;
using AcademicHybridApp.Models;

namespace AcademicHybridApp.Services
{
    public class MahasiswaService : IMahasiswaService
    {
        private const string BasePath = "api/mahasiswa";

        private readonly HttpClient _httpClient;

        public MahasiswaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ServiceResult<List<MahasiswaModel>>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(BasePath);
                if (!response.IsSuccessStatusCode)
                {
                    return ServiceResult<List<MahasiswaModel>>.Fail(await ApiErrorReader.ReadMessageAsync(response));
                }

                var data = await response.Content.ReadFromJsonAsync<List<MahasiswaModel>>();
                return ServiceResult<List<MahasiswaModel>>.Ok(data ?? new List<MahasiswaModel>());
            }
            catch (TaskCanceledException)
            {
                return ServiceResult<List<MahasiswaModel>>.Fail(ServiceMessages.Timeout);
            }
            catch (HttpRequestException)
            {
                return ServiceResult<List<MahasiswaModel>>.Fail(ServiceMessages.ConnectionFailed);
            }
            catch (Exception)
            {
                return ServiceResult<List<MahasiswaModel>>.Fail(ServiceMessages.Unexpected);
            }
        }

        public async Task<ServiceResult<MahasiswaModel>> GetByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BasePath}/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    return ServiceResult<MahasiswaModel>.Fail(await ApiErrorReader.ReadMessageAsync(response));
                }

                var data = await response.Content.ReadFromJsonAsync<MahasiswaModel>();
                if (data is null)
                {
                    return ServiceResult<MahasiswaModel>.Fail("Data mahasiswa tidak dapat dibaca.");
                }

                return ServiceResult<MahasiswaModel>.Ok(data);
            }
            catch (TaskCanceledException)
            {
                return ServiceResult<MahasiswaModel>.Fail(ServiceMessages.Timeout);
            }
            catch (HttpRequestException)
            {
                return ServiceResult<MahasiswaModel>.Fail(ServiceMessages.ConnectionFailed);
            }
            catch (Exception)
            {
                return ServiceResult<MahasiswaModel>.Fail(ServiceMessages.Unexpected);
            }
        }

        public async Task<ServiceResult<MahasiswaModel>> CreateAsync(MahasiswaFormModel form)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(BasePath, form);
                if (!response.IsSuccessStatusCode)
                {
                    return ServiceResult<MahasiswaModel>.Fail(await ApiErrorReader.ReadMessageAsync(response));
                }

                var data = await response.Content.ReadFromJsonAsync<MahasiswaModel>();
                if (data is null)
                {
                    return ServiceResult<MahasiswaModel>.Fail("Data mahasiswa yang dibuat tidak dapat dibaca.");
                }

                return ServiceResult<MahasiswaModel>.Ok(data, "Mahasiswa berhasil ditambahkan.");
            }
            catch (TaskCanceledException)
            {
                return ServiceResult<MahasiswaModel>.Fail(ServiceMessages.Timeout);
            }
            catch (HttpRequestException)
            {
                return ServiceResult<MahasiswaModel>.Fail(ServiceMessages.ConnectionFailed);
            }
            catch (Exception)
            {
                return ServiceResult<MahasiswaModel>.Fail(ServiceMessages.Unexpected);
            }
        }

        public async Task<ServiceResult> UpdateAsync(int id, MahasiswaFormModel form)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{BasePath}/{id}", form);
                if (!response.IsSuccessStatusCode)
                {
                    return ServiceResult.Fail(await ApiErrorReader.ReadMessageAsync(response));
                }

                return ServiceResult.Ok("Mahasiswa berhasil diperbarui.");
            }
            catch (TaskCanceledException)
            {
                return ServiceResult.Fail(ServiceMessages.Timeout);
            }
            catch (HttpRequestException)
            {
                return ServiceResult.Fail(ServiceMessages.ConnectionFailed);
            }
            catch (Exception)
            {
                return ServiceResult.Fail(ServiceMessages.Unexpected);
            }
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{BasePath}/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    return ServiceResult.Fail(await ApiErrorReader.ReadMessageAsync(response));
                }

                return ServiceResult.Ok("Mahasiswa berhasil dihapus.");
            }
            catch (TaskCanceledException)
            {
                return ServiceResult.Fail(ServiceMessages.Timeout);
            }
            catch (HttpRequestException)
            {
                return ServiceResult.Fail(ServiceMessages.ConnectionFailed);
            }
            catch (Exception)
            {
                return ServiceResult.Fail(ServiceMessages.Unexpected);
            }
        }
    }
}
