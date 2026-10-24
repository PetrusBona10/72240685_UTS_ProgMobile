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

        public async Task<(IEnumerable<Mahasiswa> Items, int TotalCount)> GetFilteredAsync(
            string? search,
            string? sortBy,
            string? sortDir,
            int? page,
            int? pageSize)
        {
            var query = _context.Mahasiswa
                .Include(m => m.Dosen)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                query = query.Where(m =>
                    m.Nim.ToLower().Contains(term) ||
                    m.NamaMahasiswa.ToLower().Contains(term));
            }

            var isDesc = string.Equals(sortDir, "desc", StringComparison.OrdinalIgnoreCase);
            query = (sortBy ?? string.Empty).ToLower() switch
            {
                "nim" => isDesc ? query.OrderByDescending(m => m.Nim) : query.OrderBy(m => m.Nim),
                "angkatan" => isDesc ? query.OrderByDescending(m => m.Angkatan) : query.OrderBy(m => m.Angkatan),
                "nama" => isDesc ? query.OrderByDescending(m => m.NamaMahasiswa) : query.OrderBy(m => m.NamaMahasiswa),
                _ => query.OrderBy(m => m.NamaMahasiswa)
            };

            var totalCount = await query.CountAsync();

            if (page.HasValue && pageSize.HasValue)
            {
                var p = page.Value;
                var ps = pageSize.Value;
                if (p < 1) p = 1;
                if (ps < 1) ps = 1;
                var items = await query
                    .Skip((p - 1) * ps)
                    .Take(ps)
                    .ToListAsync();
                return (items, totalCount);
            }

            var all = await query.ToListAsync();
            return (all, totalCount);
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
