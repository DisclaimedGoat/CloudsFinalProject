using FinalProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FinalProject.Services;

public class AppContext(IOptions<DatabaseConfig> dbConfigOptions) : DbContext
{
	private DatabaseConfig DbConfig { get; } = dbConfigOptions.Value;

	public DbSet<User> Users { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		switch (DbConfig.Type)
		{
			case DatabaseConfig.DbType.Memory:
				optionsBuilder.UseInMemoryDatabase(DbConfig.ConnectionString);
				break;
			case DatabaseConfig.DbType.Postgres:
				optionsBuilder.UseNpgsql(DbConfig.ConnectionString);
				break;
			case DatabaseConfig.DbType.None:
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}
}