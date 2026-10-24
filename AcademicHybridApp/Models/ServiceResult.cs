namespace AcademicHybridApp.Models
{
    public class ServiceResult
    {
        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;

        public static ServiceResult Ok(string message = "Berhasil.")
        {
            return new ServiceResult
            {
                Success = true,
                Message = message,
            };
        }

        public static ServiceResult Fail(string message)
        {
            return new ServiceResult
            {
                Success = false,
                Message = message,
            };
        }
    }

    public class ServiceResult<T>
    {
        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;

        public T? Data { get; set; }

        public int TotalCount { get; set; }

        public static ServiceResult<T> Ok(T data, string message = "Berhasil.")
        {
            return new ServiceResult<T>
            {
                Success = true,
                Message = message,
                Data = data,
            };
        }

        public static ServiceResult<T> Fail(string message, int? statusCode = null)
        {
            return new ServiceResult<T>
            {
                Success = false,
                Message = message,
            };
        }
    }
}
