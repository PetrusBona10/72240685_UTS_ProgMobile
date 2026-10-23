using AcademicApi.Data;
using AcademicApi.DTOs;
using AcademicApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace AcademicApi.Controllers
{
    [Route("api/dosen")]
    [ApiController]
    public class DosenController : ControllerBase
    {
        private readonly IDosenDAL _dosenDal;

        public DosenController(IDosenDAL dosenDal)
        {
            _dosenDal = dosenDal;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DosenDto>>> GetAllAsync()
        {
            var dosen = await _dosenDal.GetAllAsync();
            return Ok(dosen.Select(ToDto).ToList());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<DosenDto>> GetByIdAsync(int id)
        {
            var dosen = await _dosenDal.GetByIdAsync(id);
            if (dosen is null)
            {
                return NotFound(new { message = "Dosen tidak ditemukan." });
            }

            return Ok(ToDto(dosen));
        }

        [HttpPost]
        public async Task<ActionResult<DosenDto>> CreateAsync(DosenCreateDto dto)
        {
            var nidn = dto.Nidn.Trim();

            if (await _dosenDal.NidnExistsAsync(nidn))
            {
                return Conflict(new { message = "NIDN sudah digunakan oleh dosen lain." });
            }

            var dosen = new Dosen
            {
                Nidn = nidn,
                NamaDosen = dto.NamaDosen.Trim(),
                Email = dto.Email.Trim(),
                Prodi = dto.Prodi.Trim(),
            };

            var created = await _dosenDal.AddAsync(dosen);
            var result = ToDto(created);

            return CreatedAtAction("GetById", new { id = result.DosenId }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync(int id, DosenUpdateDto dto)
        {
            var dosen = await _dosenDal.GetByIdAsync(id);
            if (dosen is null)
            {
                return NotFound(new { message = "Dosen tidak ditemukan." });
            }

            var nidn = dto.Nidn.Trim();

            if (await _dosenDal.NidnExistsAsync(nidn, id))
            {
                return Conflict(new { message = "NIDN sudah digunakan oleh dosen lain." });
            }

            dosen.Nidn = nidn;
            dosen.NamaDosen = dto.NamaDosen.Trim();
            dosen.Email = dto.Email.Trim();
            dosen.Prodi = dto.Prodi.Trim();

            await _dosenDal.UpdateAsync(dosen);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var dosen = await _dosenDal.GetByIdAsync(id);
            if (dosen is null)
            {
                return NotFound(new { message = "Dosen tidak ditemukan." });
            }

            if (await _dosenDal.HasMahasiswaAsync(id))
            {
                return Conflict(new { message = "Dosen tidak dapat dihapus karena masih membimbing mahasiswa." });
            }

            await _dosenDal.DeleteAsync(id);

            return NoContent();
        }

        private static DosenDto ToDto(Dosen dosen)
        {
            return new DosenDto
            {
                DosenId = dosen.DosenId,
                Nidn = dosen.Nidn,
                NamaDosen = dosen.NamaDosen,
                Email = dosen.Email,
                Prodi = dosen.Prodi,
            };
        }
    }
}
