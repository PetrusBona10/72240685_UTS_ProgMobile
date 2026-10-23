using System.ComponentModel.DataAnnotations;

namespace AcademicHybridApp.Models
{
    public class MahasiswaFormModel
    {
        [Required(ErrorMessage = "NIM wajib diisi.")]
        [StringLength(20, ErrorMessage = "NIM tidak boleh lebih dari 20 karakter.")]
        public string Nim { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nama mahasiswa wajib diisi.")]
        [StringLength(100, ErrorMessage = "Nama mahasiswa tidak boleh lebih dari 100 karakter.")]
        public string NamaMahasiswa { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email wajib diisi.")]
        [EmailAddress(ErrorMessage = "Format email tidak valid.")]
        [StringLength(100, ErrorMessage = "Email tidak boleh lebih dari 100 karakter.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Angkatan wajib diisi.")]
        [Range(2020, 2026, ErrorMessage = "Angkatan harus berada di antara 2020 dan 2026.")]
        public int Angkatan { get; set; }

        [Required(ErrorMessage = "Dosen wali wajib dipilih.")]
        [Range(1, int.MaxValue, ErrorMessage = "Dosen wali wajib dipilih.")]
        public int DosenId { get; set; }
    }
}
