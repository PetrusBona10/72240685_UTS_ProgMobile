using Microsoft.Maui.Devices;

namespace AcademicHybridApp.Services
{
    public static class ApiConfig
    {
        public static string BaseAddress => DeviceInfo.Platform == DevicePlatform.Android
            ? "http://10.0.2.2:5080/"
            : "http://localhost:5080/";
    }
}
