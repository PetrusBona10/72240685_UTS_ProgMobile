using AcademicApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AcademicApi.Data
{
    public class MahasiswaDAL : IMahasiswaDAL
    {
        private readonly AcademicDbContext _context;

        public MahasiswaDAL(AcademicDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Mahasiswa>> GetAllAsync()
        {
            return await _context.Mahasiswa
                .Include(m => m.Dosen)
                .OrderBy(m => m.NamaMahasiswa)
                .ToListAsync();
        }

        public async Task<Mahasiswa?> GetByIdAsync(int id)
        {
            return await _context.Mahasiswa
                .Include(m => m.Dosen)
                .FirstOrDefaultAsync(m => m.MahasiswaId == id);
        }

        public async Task<bool> NimExistsAsync(string nim, int? excludeMahasiswaId = null)
        {
            return await _context.Mahasiswa
                .AnyAsync(m => m.Nim == nim && (!excludeMahasiswaId.HasValue || m.MahasiswaId != excludeMahasiswaId.Value));
        }

        public async Task<bool> DosenExistsAsync(int dosenId)
        {
            return await _context.Dosen
                .AnyAsync(d => d.DosenId == dosenId);
        }

        public async Task<Mahasiswa> AddAsync(Mahasiswa mahasiswa)
        {
            _context.Mahasiswa.Add(mahasiswa);
            await _context.SaveChangesAsync();
            return mahasiswa;
        }

        public async Task UpdateAsync(Mahasiswa mahasiswa)
        {
            _context.Mahasiswa.Update(mahasiswa);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var mahasiswa = await _context.Mahasiswa.FindAsync(id);
            if (mahasiswa is null)
            {
                return;
            }

            _context.Mahasiswa.Remove(mahasiswa);
            await _context.SaveChangesAsync();
        }
    }
}
