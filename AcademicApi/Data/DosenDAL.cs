using AcademicApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AcademicApi.Data
{
    public class DosenDAL : IDosenDAL
    {
        private readonly AcademicDbContext _context;

        public DosenDAL(AcademicDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Dosen>> GetAllAsync()
        {
            return await _context.Dosen
                .OrderBy(d => d.NamaDosen)
                .ToListAsync();
        }

        public async Task<Dosen?> GetByIdAsync(int id)
        {
            return await _context.Dosen
                .FirstOrDefaultAsync(d => d.DosenId == id);
        }

        public async Task<bool> NidnExistsAsync(string nidn, int? excludeDosenId = null)
        {
            return await _context.Dosen
                .AnyAsync(d => d.Nidn == nidn && (!excludeDosenId.HasValue || d.DosenId != excludeDosenId.Value));
        }

        public async Task<Dosen> AddAsync(Dosen dosen)
        {
            _context.Dosen.Add(dosen);
            await _context.SaveChangesAsync();
            return dosen;
        }

        public async Task UpdateAsync(Dosen dosen)
        {
            _context.Dosen.Update(dosen);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var dosen = await _context.Dosen.FindAsync(id);
            if (dosen is null)
            {
                return;
            }

            _context.Dosen.Remove(dosen);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasMahasiswaAsync(int dosenId)
        {
            return await _context.Mahasiswa
                .AnyAsync(m => m.DosenId == dosenId);
        }
    }
}
