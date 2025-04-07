using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models;

[Table("users")]
public class User
{
	[Column("id")]
	public Guid Id { get; set; } = Guid.NewGuid();

	[Column("username")]
	public string Username { get; set; } = string.Empty;

	[Column("salt")]
	public string Salt { get; set; } = string.Empty;

	[Column("password")]
	public string HashedPassword { get; set; } = string.Empty;

	[Column("role")]
	public Roles Role { get; set; } = Roles.None;
}