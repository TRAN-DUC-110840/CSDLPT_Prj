using System.ComponentModel.DataAnnotations;
using CSDLPT_API.Entities;

namespace CSDLPT_API.Dtos;

public class RegisterDto
{
	[Required]
	[MaxLength(100)]
	public string Username { get; set; } = default!;

	[Required]
	[MinLength(6)]
	public string Password { get; set; } = default!;

	[Required]
	public Role Role { get; set; }
}

public class LoginDto
{
	[Required]
	[MaxLength(100)]
	public string Username { get; set; } = default!;

	[Required]
	public string Password { get; set; } = default!;
}


