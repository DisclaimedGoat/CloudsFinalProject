namespace FinalProject.Models;

public class DatabaseConfig
{
    public enum DbType
    {
        Memory, Postgres, None
    }

    public DbType Type { get; set; } = DbType.None;
    
    public string ConnectionString { get; set; } = string.Empty;
}