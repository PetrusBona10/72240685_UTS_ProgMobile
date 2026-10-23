using System.Net.Http.Json;
using AcademicHybridApp.Models;

namespace AcademicHybridApp.Services
{
    public class DosenService : IDosenService
    {
        private const string BasePath = "api/dosen";

        private readonly HttpClient _httpClient;

        public DosenService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ServiceResult<List<DosenModel>>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(BasePath);
                if (!response.IsSuccessStatusCode)
                {
                    return ServiceResult<List<DosenModel>>.Fail(await ApiErrorReader.ReadMessageAsync(response));
                }

                var data = await response.Content.ReadFromJsonAsync<List<DosenModel>>();
                return ServiceResult<List<DosenModel>>.Ok(data ?? new List<DosenModel>());
            }
            catch (TaskCanceledException)
            {
                return ServiceResult<List<DosenModel>>.Fail(ServiceMessages.Timeout);
            }
            catch (HttpRequestException)
            {
                return ServiceResult<List<DosenModel>>.Fail(ServiceMessages.ConnectionFailed);
            }
            catch (Exception)
            {
                return ServiceResult<List<DosenModel>>.Fail(ServiceMessages.Unexpected);
            }
        }

        public async Task<ServiceResult<DosenModel>> GetByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BasePath}/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    return ServiceResult<DosenModel>.Fail(await ApiErrorReader.ReadMessageAsync(response));
                }

                var data = await response.Content.ReadFromJsonAsync<DosenModel>();
                if (data is null)
                {
                    return ServiceResult<DosenModel>.Fail("Data dosen tidak dapat dibaca.");
                }

                return ServiceResult<DosenModel>.Ok(data);
            }
            catch (TaskCanceledException)
            {
                return ServiceResult<DosenModel>.Fail(ServiceMessages.Timeout);
            }
            catch (HttpRequestException)
            {
                return ServiceResult<DosenModel>.Fail(ServiceMessages.ConnectionFailed);
            }
            catch (Exception)
            {
                return ServiceResult<DosenModel>.Fail(ServiceMessages.Unexpected);
            }
        }

        public async Task<ServiceResult<DosenModel>> CreateAsync(DosenFormModel form)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(BasePath, form);
                if (!response.IsSuccessStatusCode)
                {
                    return ServiceResult<DosenModel>.Fail(await ApiErrorReader.ReadMessageAsync(response));
                }

                var data = await response.Content.ReadFromJsonAsync<DosenModel>();
                if (data is null)
                {
                    return ServiceResult<DosenModel>.Fail("Data dosen yang dibuat tidak dapat dibaca.");
                }

                return ServiceResult<DosenModel>.Ok(data, "Dosen berhasil ditambahkan.");
            }
            catch (TaskCanceledException)
            {
                return ServiceResult<DosenModel>.Fail(ServiceMessages.Timeout);
            }
            catch (HttpRequestException)
            {
                return ServiceResult<DosenModel>.Fail(ServiceMessages.ConnectionFailed);
            }
            catch (Exception)
            {
                return ServiceResult<DosenModel>.Fail(ServiceMessages.Unexpected);
            }
        }

        public async Task<ServiceResult> UpdateAsync(int id, DosenFormModel form)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{BasePath}/{id}", form);
                if (!response.IsSuccessStatusCode)
                {
                    return ServiceResult.Fail(await ApiErrorReader.ReadMessageAsync(response));
                }

                return ServiceResult.Ok("Dosen berhasil diperbarui.");
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

                return ServiceResult.Ok("Dosen berhasil dihapus.");
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
