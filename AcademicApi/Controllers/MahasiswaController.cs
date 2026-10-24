using AcademicApi.Data;
using AcademicApi.DTOs;
using AcademicApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace AcademicApi.Controllers
{
    [Route("api/mahasiswa")]
    [ApiController]
    public class MahasiswaController : ControllerBase
    {
        private readonly IMahasiswaDAL _mahasiswaDal;

        public MahasiswaController(IMahasiswaDAL mahasiswaDal)
        {
            _mahasiswaDal = mahasiswaDal;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MahasiswaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<MahasiswaDto>>> GetAllAsync(
            [FromQuery] string? search,
            [FromQuery] string? sortBy,
            [FromQuery] string? sortDir,
            [FromQuery] int? page,
            [FromQuery] int? pageSize)
        {
            if (page.HasValue || pageSize.HasValue)
            {
                if (!page.HasValue || !pageSize.HasValue)
                {
                    return BadRequest(new { message = "page dan pageSize harus diisi bersamaan." });
                }

                if (page.Value < 1)
                {
                    return BadRequest(new { message = "page harus bernilai minimal 1." });
                }

                if (pageSize.Value < 1 || pageSize.Value > 100)
                {
                    return BadRequest(new { message = "pageSize harus bernilai antara 1 dan 100." });
                }
            }

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                var sb = sortBy.Trim().ToLower();
                if (sb != "nama" && sb != "nim" && sb != "angkatan")
                {
                    return BadRequest(new { message = "sortBy hanya boleh: nama, nim, atau angkatan." });
                }
            }

            if (!string.IsNullOrWhiteSpace(sortDir))
            {
                var sd = sortDir.Trim().ToLower();
                if (sd != "asc" && sd != "desc")
                {
                    return BadRequest(new { message = "sortDir hanya boleh: asc atau desc." });
                }
            }

            if (string.IsNullOrWhiteSpace(search) && string.IsNullOrWhiteSpace(sortBy) && string.IsNullOrWhiteSpace(sortDir) && !page.HasValue && !pageSize.HasValue)
            {
                var mahasiswa = await _mahasiswaDal.GetAllAsync();
                return Ok(mahasiswa.Select(ToDto).ToList());
            }

            var result = await _mahasiswaDal.GetFilteredAsync(search, sortBy, sortDir, page, pageSize);
            Response.Headers["X-Total-Count"] = result.TotalCount.ToString();
            return Ok(result.Items.Select(ToDto).ToList());
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(MahasiswaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MahasiswaDto>> GetByIdAsync(int id)
        {
            var mahasiswa = await _mahasiswaDal.GetByIdAsync(id);
            if (mahasiswa is null)
            {
                return NotFound(new { message = "Mahasiswa tidak ditemukan." });
            }

            return Ok(ToDto(mahasiswa));
        }

        [HttpPost]
        [ProducesResponseType(typeof(MahasiswaDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<MahasiswaDto>> CreateAsync(MahasiswaCreateDto dto)
        {
            var nim = dto.Nim.Trim();

            if (await _mahasiswaDal.NimExistsAsync(nim))
            {
                return Conflict(new { message = "NIM sudah digunakan oleh mahasiswa lain." });
            }

            if (!await _mahasiswaDal.DosenExistsAsync(dto.DosenId))
            {
                return NotFound(new { message = "Dosen wali tidak ditemukan." });
            }

            var mahasiswa = new Mahasiswa
            {
                Nim = nim,
                NamaMahasiswa = dto.NamaMahasiswa.Trim(),
                Email = dto.Email.Trim(),
                Angkatan = dto.Angkatan,
                DosenId = dto.DosenId,
            };

            var created = await _mahasiswaDal.AddAsync(mahasiswa);
            var createdWithDosen = await _mahasiswaDal.GetByIdAsync(created.MahasiswaId);
            var result = ToDto(createdWithDosen ?? created);

            return CreatedAtAction("GetById", new { id = result.MahasiswaId }, result);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateAsync(int id, MahasiswaUpdateDto dto)
        {
            var mahasiswa = await _mahasiswaDal.GetByIdAsync(id);
            if (mahasiswa is null)
            {
                return NotFound(new { message = "Mahasiswa tidak ditemukan." });
            }

            var nim = dto.Nim.Trim();

            if (await _mahasiswaDal.NimExistsAsync(nim, id))
            {
                return Conflict(new { message = "NIM sudah digunakan oleh mahasiswa lain." });
            }

            if (!await _mahasiswaDal.DosenExistsAsync(dto.DosenId))
            {
                return NotFound(new { message = "Dosen wali tidak ditemukan." });
            }

            mahasiswa.Nim = nim;
            mahasiswa.NamaMahasiswa = dto.NamaMahasiswa.Trim();
            mahasiswa.Email = dto.Email.Trim();
            mahasiswa.Angkatan = dto.Angkatan;
            mahasiswa.DosenId = dto.DosenId;

            await _mahasiswaDal.UpdateAsync(mahasiswa);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var mahasiswa = await _mahasiswaDal.GetByIdAsync(id);
            if (mahasiswa is null)
            {
                return NotFound(new { message = "Mahasiswa tidak ditemukan." });
            }

            await _mahasiswaDal.DeleteAsync(id);

            return NoContent();
        }

        private static MahasiswaDto ToDto(Mahasiswa mahasiswa)
        {
            return new MahasiswaDto
            {
                MahasiswaId = mahasiswa.MahasiswaId,
                Nim = mahasiswa.Nim,
                NamaMahasiswa = mahasiswa.NamaMahasiswa,
                Email = mahasiswa.Email,
                Angkatan = mahasiswa.Angkatan,
                DosenId = mahasiswa.DosenId,
                NamaDosenWali = mahasiswa.Dosen?.NamaDosen ?? string.Empty,
            };
        }
    }
}
