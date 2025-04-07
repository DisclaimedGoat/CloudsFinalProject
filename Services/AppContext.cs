using FinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Services;

public class AppContext : DbContext
{
	public DbSet<User> Users { get; set; }
}