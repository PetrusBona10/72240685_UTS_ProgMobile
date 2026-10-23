using System.Net.Http.Json;
using System.Text.Json;
using AcademicHybridApp.Models;

namespace AcademicHybridApp.Services
{
    public abstract class ApiServiceBase
    {
        private readonly HttpClient _httpClient;

        protected ApiServiceBase(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        protected Task<ServiceResult<List<T>>> GetListAsync<T>(string path)
        {
            return ExecuteAsync(
                () => _httpClient.GetAsync(path),
                async response =>
                {
                    var data = await response.Content.ReadFromJsonAsync<List<T>>();
                    return ServiceResult<List<T>>.Ok(data ?? new List<T>());
                });
        }

        protected Task<ServiceResult<T>> GetItemAsync<T>(string path)
        {
            return ExecuteAsync(
                () => _httpClient.GetAsync(path),
                async response =>
                {
                    var data = await response.Content.ReadFromJsonAsync<T>();
                    return data is null
                        ? ServiceResult<T>.Fail(ServiceMessages.InvalidResponse)
                        : ServiceResult<T>.Ok(data);
                });
        }

        protected Task<ServiceResult<TResponse>> CreateItemAsync<TRequest, TResponse>(
            string path,
            TRequest body,
            string successMessage)
        {
            return ExecuteAsync(
                () => _httpClient.PostAsJsonAsync(path, body),
                async response =>
                {
                    var data = await response.Content.ReadFromJsonAsync<TResponse>();
                    return data is null
                        ? ServiceResult<TResponse>.Fail(ServiceMessages.InvalidResponse)
                        : ServiceResult<TResponse>.Ok(data, successMessage);
                });
        }

        protected Task<ServiceResult> UpdateItemAsync<TRequest>(
            string path,
            TRequest body,
            string successMessage)
        {
            return ExecuteAsync(
                () => _httpClient.PutAsJsonAsync(path, body),
                _ => Task.FromResult(ServiceResult.Ok(successMessage)));
        }

        protected Task<ServiceResult> DeleteItemAsync(string path, string successMessage)
        {
            return ExecuteAsync(
                () => _httpClient.DeleteAsync(path),
                _ => Task.FromResult(ServiceResult.Ok(successMessage)));
        }

        private async Task<ServiceResult<T>> ExecuteAsync<T>(
            Func<Task<HttpResponseMessage>> send,
            Func<HttpResponseMessage, Task<ServiceResult<T>>> readSuccess)
        {
            try
            {
                using var response = await send();

                if (!response.IsSuccessStatusCode)
                {
                    return ServiceResult<T>.Fail(await ApiErrorReader.ReadMessageAsync(response));
                }

                return await readSuccess(response);
            }
            catch (OperationCanceledException)
            {
                return ServiceResult<T>.Fail(ServiceMessages.Timeout);
            }
            catch (HttpRequestException)
            {
                return ServiceResult<T>.Fail(ServiceMessages.ConnectionFailed);
            }
            catch (JsonException)
            {
                return ServiceResult<T>.Fail(ServiceMessages.InvalidResponse);
            }
            catch (NotSupportedException)
            {
                return ServiceResult<T>.Fail(ServiceMessages.InvalidResponse);
            }
            catch (Exception)
            {
                return ServiceResult<T>.Fail(ServiceMessages.Unexpected);
            }
        }

        private async Task<ServiceResult> ExecuteAsync(
            Func<Task<HttpResponseMessage>> send,
            Func<HttpResponseMessage, Task<ServiceResult>> readSuccess)
        {
            try
            {
                using var response = await send();

                if (!response.IsSuccessStatusCode)
                {
                    return ServiceResult.Fail(await ApiErrorReader.ReadMessageAsync(response));
                }

                return await readSuccess(response);
            }
            catch (OperationCanceledException)
            {
                return ServiceResult.Fail(ServiceMessages.Timeout);
            }
            catch (HttpRequestException)
            {
                return ServiceResult.Fail(ServiceMessages.ConnectionFailed);
            }
            catch (JsonException)
            {
                return ServiceResult.Fail(ServiceMessages.InvalidResponse);
            }
            catch (NotSupportedException)
            {
                return ServiceResult.Fail(ServiceMessages.InvalidResponse);
            }
            catch (Exception)
            {
                return ServiceResult.Fail(ServiceMessages.Unexpected);
            }
        }
    }
}
