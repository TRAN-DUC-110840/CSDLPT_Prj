using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDLPT_API.Entities;

[Table("Users")]
public class User
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }

	[Required]
	[MaxLength(100)]
	public string Username { get; set; } = default!;

	[Required]
	[MaxLength(255)]
	public string PasswordHash { get; set; } = default!;

	[Required]
	public Role Role { get; set; }

	public bool IsApproved { get; set; } = true;
}


