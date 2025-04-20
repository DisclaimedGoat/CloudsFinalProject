using System.Security.Cryptography;
using System.Security.Policy;
using FinalProject.Models;

namespace FinalProject.Services;

/*
 * Hashing algorithm and methods were gathered from here:
 * https://stackoverflow.com/a/73125177
 */

public class UserService(AppContext appContext)
{
	private const int SaltSize = 16; // 128 bits
	private const int KeySize = 32; // 256 bits
	private const int Iterations = 50000;
	private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;

	private const char SegmentDelimiter = ':';

	
	public async Task InitializeAsync()
	{
		User admin = new("admin@admin.com", "admin", "password")
		{
			Role = Roles.Admin
		};
		
		await SignupAsync(admin);
	}
	
	public async Task<User?> AuthAsync(string username, string password)
	{
		User? find = appContext.Users.FirstOrDefault(user => user.Username == username);
		if (find is null) return null;

		if (!Verify(password, find.HashedPassword)) return null;
		
		find.LoggedIn = DateTime.Now;
		find.Modified = DateTime.Now;
		
		appContext.Update(find);
		await appContext.SaveChangesAsync();
		return find;
	}

	public List<string> GetUsersAsync()
	{
		return appContext.Users.Select(u => u.Username).ToList();
	}

	public async Task<bool> SignupAsync(User user)
	{
		user.LoggedIn = DateTime.Now;
		user.Modified = DateTime.Now;
		
		await appContext.AddAsync(user);
		await appContext.SaveChangesAsync();
		return true;
	}

	public static string Hash(string input)
	{
		byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
		byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
			input,
			salt,
			Iterations,
			Algorithm,
			KeySize
		);
		return string.Join(
			SegmentDelimiter,
			Convert.ToHexString(hash),
			Convert.ToHexString(salt),
			Iterations,
			Algorithm
		);
	}

	public static bool Verify(string input, string hashString)
	{
		string[] segments = hashString.Split(SegmentDelimiter);
		byte[] hash = Convert.FromHexString(segments[0]);
		byte[] salt = Convert.FromHexString(segments[1]);
		int iterations = int.Parse(segments[2]);
		HashAlgorithmName algorithm = new HashAlgorithmName(segments[3]);
		byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(
			input,
			salt,
			iterations,
			algorithm,
			hash.Length
		);
		return CryptographicOperations.FixedTimeEquals(inputHash, hash);
	}
	
}