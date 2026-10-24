using AcademicApi.Data;
using AcademicApi.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AcademicApi.Tests.Helpers;

public class TestDbFactory
{
    public static (AcademicDbContext Context, SqliteConnection Connection) CreateContext()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<AcademicDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new AcademicDbContext(options);
        context.Database.EnsureCreated();
        return (context, connection);
    }
}
