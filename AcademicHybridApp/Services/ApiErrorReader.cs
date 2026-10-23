using System.Net.Http.Json;
using System.Text.Json;

namespace AcademicHybridApp.Services
{
    internal static class ApiErrorReader
    {
        public static async Task<string> ReadMessageAsync(HttpResponseMessage response)
        {
            try
            {
                var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
                if (!string.IsNullOrWhiteSpace(error?.Message))
                {
                    return error.Message;
                }
            }
            catch (JsonException)
            {
            }
            catch (NotSupportedException)
            {
            }

            return $"Terjadi kesalahan pada server ({(int)response.StatusCode}).";
        }

        private sealed class ApiErrorResponse
        {
            public string? Message { get; set; }
        }
    }
}
