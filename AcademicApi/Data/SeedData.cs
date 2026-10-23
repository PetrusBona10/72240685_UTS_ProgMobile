using AcademicApi.Models;

namespace AcademicApi.Data
{
    public static class SeedData
    {
        public static void Initialize(AcademicDbContext db)
        {
            if (db.Dosen.Any() || db.Mahasiswa.Any())
            {
                return;
            }

            var daftarDosen = new List<Dosen>
            {
                new Dosen
                {
                    Nidn = "0512345678",
                    NamaDosen = "Dr. Budi Santoso, M.Kom.",
                    Email = "budi.santoso@ukdw.ac.id",
                    Prodi = "Informatika",
                },
                new Dosen
                {
                    Nidn = "0623456789",
                    NamaDosen = "Rina Wijaya, S.Kom., M.Cs.",
                    Email = "rina.wijaya@ukdw.ac.id",
                    Prodi = "Sistem Informasi",
                },
                new Dosen
                {
                    Nidn = "0734567890",
                    NamaDosen = "Agus Prasetyo, S.T., M.T.",
                    Email = "agus.prasetyo@ukdw.ac.id",
                    Prodi = "Teknik Informatika",
                },
            };

            db.Dosen.AddRange(daftarDosen);
            db.SaveChanges();

            var daftarMahasiswa = new List<Mahasiswa>
            {
                new Mahasiswa
                {
                    Nim = "71210001",
                    NamaMahasiswa = "Andi Saputra",
                    Email = "andi.saputra@student.ukdw.ac.id",
                    Angkatan = 2021,
                    DosenId = daftarDosen[0].DosenId,
                },
                new Mahasiswa
                {
                    Nim = "71210002",
                    NamaMahasiswa = "Siti Nurhaliza",
                    Email = "siti.nurhaliza@student.ukdw.ac.id",
                    Angkatan = 2021,
                    DosenId = daftarDosen[0].DosenId,
                },
                new Mahasiswa
                {
                    Nim = "71220015",
                    NamaMahasiswa = "Rizky Pratama",
                    Email = "rizky.pratama@student.ukdw.ac.id",
                    Angkatan = 2022,
                    DosenId = daftarDosen[1].DosenId,
                },
                new Mahasiswa
                {
                    Nim = "71230022",
                    NamaMahasiswa = "Maria Natalia",
                    Email = "maria.natalia@student.ukdw.ac.id",
                    Angkatan = 2023,
                    DosenId = daftarDosen[2].DosenId,
                },
                new Mahasiswa
                {
                    Nim = "71240008",
                    NamaMahasiswa = "Fajar Nugroho",
                    Email = "fajar.nugroho@student.ukdw.ac.id",
                    Angkatan = 2024,
                    DosenId = daftarDosen[1].DosenId,
                },
            };

            db.Mahasiswa.AddRange(daftarMahasiswa);
            db.SaveChanges();
        }
    }
}
