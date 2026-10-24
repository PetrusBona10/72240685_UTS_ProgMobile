using AcademicApi.Controllers;
using AcademicApi.Data;
using AcademicApi.DTOs;
using AcademicApi.Models;
using AcademicApi.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademicApi.Tests.Controllers;

public class MahasiswaControllerTests
{
    private async Task<Dosen> SeedDosenAsync(AcademicDbContext context)
    {
        var dal = new DosenDAL(context);
        return await dal.AddAsync(new Dosen
        {
            Nidn = "44444444",
            NamaDosen = "Dr. Seed",
            Email = "seed@x.com",
            Prodi = "TI"
        });
    }

    [Fact]
    public async Task GetAllAsync_LoadsNamaDosenWali()
    {
        var (context, connection) = TestDbFactory.CreateContext();
        using var _ = connection;
        var dosen = await SeedDosenAsync(context);
        var mDal = new MahasiswaDAL(context);
        await mDal.AddAsync(new Mahasiswa
        {
            Nim = "M100",
            NamaMahasiswa = "Andi",
            Email = "andi@x.com",
            Angkatan = 2025,
            DosenId = dosen.DosenId
        });

        var controller = new MahasiswaController(mDal);
        var result = await controller.GetAllAsync(null, null, null, null, null);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var list = Assert.IsAssignableFrom<IEnumerable<MahasiswaDto>>(ok.Value);
        var item = list.First();
        Assert.Equal("Dr. Seed", item.NamaDosenWali);
    }

    [Fact]
    public async Task CreateAsync_DosenNotExists_Returns404()
    {
        var (context, connection) = TestDbFactory.CreateContext();
        using var _ = connection;
        var mDal = new MahasiswaDAL(context);
        var controller = new MahasiswaController(mDal);

        var dto = new MahasiswaCreateDto
        {
            Nim = "M200",
            NamaMahasiswa = "Budi",
            Email = "budi@x.com",
            Angkatan = 2026,
            DosenId = 999
        };

        var result = await controller.CreateAsync(dto);

        var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(StatusCodes.Status404NotFound, notFound.StatusCode);
    }

    [Fact]
    public async Task CreateAsync_DuplicateNim_Returns409()
    {
        var (context, connection) = TestDbFactory.CreateContext();
        using var _ = connection;
        var dosen = await SeedDosenAsync(context);
        var mDal = new MahasiswaDAL(context);
        var controller = new MahasiswaController(mDal);

        var dto1 = new MahasiswaCreateDto { Nim = "M300", NamaMahasiswa = "A", Email = "a@x.com", Angkatan = 2026, DosenId = dosen.DosenId };
        var dto2 = new MahasiswaCreateDto { Nim = "M300", NamaMahasiswa = "B", Email = "b@x.com", Angkatan = 2026, DosenId = dosen.DosenId };

        await controller.CreateAsync(dto1);
        var result = await controller.CreateAsync(dto2);

        var conflict = Assert.IsType<ConflictObjectResult>(result.Result);
        Assert.Equal(StatusCodes.Status409Conflict, conflict.StatusCode);
    }

    [Fact]
    public async Task UpdateAsync_ChangeDosenWali_Saved()
    {
        var (context, connection) = TestDbFactory.CreateContext();
        using var _ = connection;
        var dosen1 = await SeedDosenAsync(context);
        var dosenDal = new DosenDAL(context);
        var dosen2 = await dosenDal.AddAsync(new Dosen { Nidn = "55555555", NamaDosen = "Dr. Baru", Email = "baru@x.com", Prodi = "SI" });

        var mDal = new MahasiswaDAL(context);
        var controller = new MahasiswaController(mDal);
        var created = (CreatedAtActionResult)(await controller.CreateAsync(new MahasiswaCreateDto
        {
            Nim = "M400",
            NamaMahasiswa = "Citra",
            Email = "citra@x.com",
            Angkatan = 2026,
            DosenId = dosen1.DosenId
        })).Result!;
        var dto = (MahasiswaDto)created.Value!;
        var update = new MahasiswaUpdateDto { Nim = dto.Nim, NamaMahasiswa = dto.NamaMahasiswa, Email = dto.Email, Angkatan = dto.Angkatan, DosenId = dosen2.DosenId };

        var result = await controller.UpdateAsync(dto.MahasiswaId, update);

        Assert.IsType<NoContentResult>(result);
        var updated = await mDal.GetByIdAsync(dto.MahasiswaId);
        Assert.Equal(dosen2.DosenId, updated!.DosenId);
    }

    [Fact]
    public async Task GetByIdAsync_NotFound_Returns404()
    {
        var (context, connection) = TestDbFactory.CreateContext();
        using var _ = connection;
        var mDal = new MahasiswaDAL(context);
        var controller = new MahasiswaController(mDal);

        var result = await controller.GetByIdAsync(12345);

        var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(StatusCodes.Status404NotFound, notFound.StatusCode);
    }

    [Fact]
    public async Task GetFilteredAsync_SearchAndSort_Works()
    {
        var (context, connection) = TestDbFactory.CreateContext();
        using var _ = connection;
        var dosen = await SeedDosenAsync(context);
        var mDal = new MahasiswaDAL(context);

        await mDal.AddAsync(new Mahasiswa { Nim = "20260001", NamaMahasiswa = "Budi", Email = "b@x.com", Angkatan = 2026, DosenId = dosen.DosenId });
        await mDal.AddAsync(new Mahasiswa { Nim = "20250002", NamaMahasiswa = "Andi", Email = "a@x.com", Angkatan = 2025, DosenId = dosen.DosenId });

        var filtered = await mDal.GetFilteredAsync("bud", "nama", "asc", 1, 10);
        Assert.Single(filtered.Items);
        var allAsc = await mDal.GetFilteredAsync(null, "nim", "asc", null, null);
        Assert.Equal("20250002", allAsc.Items.First().Nim);
    }
}
