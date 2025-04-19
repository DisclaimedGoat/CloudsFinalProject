using System.ComponentModel.DataAnnotations.Schema;
using FinalProject.Services;

namespace FinalProject.Models;

[Table("users")]
public class User()
{
	public User(string email, string username, string password) : this()
	{
		Email = email;
		Username = username;
		HashedPassword = UserService.Hash(password);
	}
	
	[Column("id")]
	public Guid Id { get; init; } = Guid.NewGuid();

	[Column("email")]
	public string Email { get; init; } = string.Empty;
	
	[Column("username")]
	public string Username { get; init; } = "None";

	[Column("password")]
	public string HashedPassword { get; init; } = string.Empty;

	[Column("role")]
	public Roles Role { get; set; } = Roles.None;

	[Column("created")] 
	public DateTime Created { get; init; } = DateTime.Now;
	
	[Column("modified")]
	public DateTime Modified { get; set; } = DateTime.Now;
	
	[Column("logged_in")]
	public DateTime LoggedIn { get; set; } = DateTime.MinValue;
}