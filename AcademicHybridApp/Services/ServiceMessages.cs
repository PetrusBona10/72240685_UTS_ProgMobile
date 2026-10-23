namespace AcademicHybridApp.Services
{
    internal static class ServiceMessages
    {
        public const string ConnectionFailed = "Tidak dapat terhubung ke server. Pastikan API sudah berjalan.";

        public const string Timeout = "Permintaan ke server melebihi batas waktu. Silakan coba lagi.";

        public const string InvalidResponse = "Respons dari server tidak dapat dibaca. Silakan coba lagi.";

        public const string Unexpected = "Terjadi kesalahan tak terduga. Silakan coba lagi.";
    }
}
