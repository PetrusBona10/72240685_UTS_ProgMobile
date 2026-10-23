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
        public async Task<ActionResult<IEnumerable<MahasiswaDto>>> GetAllAsync()
        {
            var mahasiswa = await _mahasiswaDal.GetAllAsync();
            return Ok(mahasiswa.Select(ToDto).ToList());
        }

        [HttpGet("{id:int}")]
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
