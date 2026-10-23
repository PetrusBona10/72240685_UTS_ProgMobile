using System.ComponentModel.DataAnnotations;

namespace AcademicApi.Models
{
    public class Dosen
    {
        public int DosenId { get; set; }

        [Required, StringLength(20)]
        public string Nidn { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string NamaDosen { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Prodi { get; set; } = string.Empty;

        public ICollection<Mahasiswa> Mahasiswa { get; set; } = new List<Mahasiswa>();
    }
}
