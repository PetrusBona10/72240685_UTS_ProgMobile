using AcademicApi.Controllers;
using AcademicApi.Data;
using AcademicApi.DTOs;
using AcademicApi.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademicApi.Tests.Controllers;

public class DosenControllerTests
{
    [Fact]
    public async Task CreateAsync_ValidDosen_Returns201()
    {
        var (context, connection) = TestDbFactory.CreateContext();
        using var _ = connection;
        var dal = new DosenDAL(context);
        var controller = new DosenController(dal);

        var dto = new DosenCreateDto
        {
            Nidn = "12345678",
            NamaDosen = "Dr. Test",
            Email = "test@univ.ac.id",
            Prodi = "Teknik Informatika"
        };

        var result = await controller.CreateAsync(dto);

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(StatusCodes.Status201Created, created.StatusCode);
    }

    [Fact]
    public async Task CreateAsync_DuplicateNidn_Returns409()
    {
        var (context, connection) = TestDbFactory.CreateContext();
        using var _ = connection;
        var dal = new DosenDAL(context);
        var controller = new DosenController(dal);

        var dto1 = new DosenCreateDto { Nidn = "11111111", NamaDosen = "A", Email = "a@x.com", Prodi = "TI" };
        var dto2 = new DosenCreateDto { Nidn = "11111111", NamaDosen = "B", Email = "b@x.com", Prodi = "SI" };

        await controller.CreateAsync(dto1);
        var result = await controller.CreateAsync(dto2);

        var conflict = Assert.IsType<ConflictObjectResult>(result.Result);
        Assert.Equal(StatusCodes.Status409Conflict, conflict.StatusCode);
    }

    [Fact]
    public async Task DeleteAsync_DosenHasMahasiswa_Returns409AndDataRemains()
    {
        var (context, connection) = TestDbFactory.CreateContext();
        using var _ = connection;
        var dosenDal = new DosenDAL(context);
        var mahasiswaDal = new MahasiswaDAL(context);
        var dosenController = new DosenController(dosenDal);

        var dosen = await dosenDal.AddAsync(new AcademicApi.Models.Dosen
        {
            Nidn = "22222222",
            NamaDosen = "Dr. Wali",
            Email = "wali@x.com",
            Prodi = "TI"
        });

        await mahasiswaDal.AddAsync(new AcademicApi.Models.Mahasiswa
        {
            Nim = "M001",
            NamaMahasiswa = "Mhs 1",
            Email = "m1@x.com",
            Angkatan = 2026,
            DosenId = dosen.DosenId
        });

        var result = await dosenController.DeleteAsync(dosen.DosenId);

        var conflict = Assert.IsType<ConflictObjectResult>(result);
        Assert.Equal(StatusCodes.Status409Conflict, conflict.StatusCode);
        Assert.NotNull(await dosenDal.GetByIdAsync(dosen.DosenId));
    }

    [Fact]
    public async Task DeleteAsync_DosenWithoutMahasiswa_Returns204()
    {
        var (context, connection) = TestDbFactory.CreateContext();
        using var _ = connection;
        var dal = new DosenDAL(context);
        var controller = new DosenController(dal);

        var dosen = await dal.AddAsync(new AcademicApi.Models.Dosen
        {
            Nidn = "33333333",
            NamaDosen = "Dr. Bebas",
            Email = "bebas@x.com",
            Prodi = "TI"
        });

        var result = await controller.DeleteAsync(dosen.DosenId);

        Assert.IsType<NoContentResult>(result);
        Assert.Null(await dal.GetByIdAsync(dosen.DosenId));
    }

    [Fact]
    public async Task GetByIdAsync_NotFound_Returns404()
    {
        var (context, connection) = TestDbFactory.CreateContext();
        using var _ = connection;
        var dal = new DosenDAL(context);
        var controller = new DosenController(dal);

        var result = await controller.GetByIdAsync(999);

        var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(StatusCodes.Status404NotFound, notFound.StatusCode);
    }
}
