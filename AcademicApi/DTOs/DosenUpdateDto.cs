using System.ComponentModel.DataAnnotations;

namespace AcademicApi.DTOs
{
    public class DosenUpdateDto
    {
        [Required(ErrorMessage = "NIDN wajib diisi.")]
        [StringLength(20, ErrorMessage = "NIDN tidak boleh lebih dari 20 karakter.")]
        public string Nidn { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nama dosen wajib diisi.")]
        [StringLength(100, ErrorMessage = "Nama dosen tidak boleh lebih dari 100 karakter.")]
        public string NamaDosen { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email wajib diisi.")]
        [EmailAddress(ErrorMessage = "Format email tidak valid.")]
        [StringLength(100, ErrorMessage = "Email tidak boleh lebih dari 100 karakter.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Program studi wajib diisi.")]
        [StringLength(100, ErrorMessage = "Program studi tidak boleh lebih dari 100 karakter.")]
        public string Prodi { get; set; } = string.Empty;
    }
}
