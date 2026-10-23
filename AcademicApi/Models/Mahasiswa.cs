using System.ComponentModel.DataAnnotations;

namespace AcademicApi.Models
{
    public class Mahasiswa
    {
        public int MahasiswaId { get; set; }

        [Required, StringLength(20)]
        public string Nim { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string NamaMahasiswa { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required, Range(2020, 2026)]
        public int Angkatan { get; set; }

        public int DosenId { get; set; }

        public Dosen Dosen { get; set; } = null!;
    }
}
