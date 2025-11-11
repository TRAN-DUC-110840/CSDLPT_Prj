using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using CSDLPT_API.Context;
using CSDLPT_API.Dtos;
using CSDLPT_API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace CSDLPT_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
	private readonly MyDbContext _db;
	private readonly IConfiguration _config;

	public AuthController(MyDbContext db, IConfiguration config)
	{
		_db = db;
		_config = config;
	}

	[HttpPost("register")]
	public async Task<IActionResult> Register([FromBody] RegisterDto dto)
	{
		var existed = await _db.Users.AnyAsync(u => u.Username == dto.Username);
		if (existed) return Conflict(new { message = "Username đã tồn tại" });

		var user = new User
		{
			Username = dto.Username,
			PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
			Role = dto.Role,
			IsApproved = dto.Role == Role.Giaovien ? false : true
		};
		_db.Users.Add(user);
		await _db.SaveChangesAsync();
		if (user.Role == Role.Giaovien)
		{
			return Ok(new { message = "Đăng ký thành công. Tài khoản giáo viên đang chờ Admin duyệt." });
		}
		return Ok(new { message = "Đăng ký thành công." });
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] LoginDto dto)
	{
		var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
		if (user == null) return Unauthorized(new { message = "Sai thông tin đăng nhập" });
		var valid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
		if (!valid) return Unauthorized(new { message = "Sai thông tin đăng nhập" });

		if (!user.IsApproved)
		{
			return Unauthorized(new { message = "Tài khoản chưa được duyệt. Vui lòng chờ Admin phê duyệt." });
		}

		var token = GenerateJwtToken(user);
		return Ok(new { token });
	}

	private string GenerateJwtToken(User user)
	{
		var secret = _config["Jwt:Secret"];
		var issuer = _config["Jwt:Issuer"];
		var audience = _config["Jwt:Audience"];
		var expiresMinutes = int.TryParse(_config["Jwt:ExpiresMinutes"], out var m) ? m : 60;

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret!));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var claims = new List<Claim>
		{
			new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
			new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
			new Claim(ClaimTypes.Role, user.Role.ToString())
		};

		var token = new JwtSecurityToken(
			issuer: issuer,
			audience: audience,
			claims: claims,
			expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
			signingCredentials: creds
		);
		return new JwtSecurityTokenHandler().WriteToken(token);
	}

	// Admin: danh sách giáo viên chờ duyệt
	[HttpGet("pending-giaovien")]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> GetPendingGiaovien()
	{
		var list = await _db.Users
			.Where(u => u.Role == Role.Giaovien && !u.IsApproved)
			.Select(u => new { u.Id, u.Username, u.Role, u.IsApproved })
			.ToListAsync();
		return Ok(list);
	}

	// Admin: duyệt tài khoản theo Id
	[HttpPost("approve/{id:int}")]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> ApproveGiaovien(int id)
	{
		var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id && u.Role == Role.Giaovien);
		if (user == null) return NotFound(new { message = "Không tìm thấy tài khoản giáo viên" });
		if (user.IsApproved) return BadRequest(new { message = "Tài khoản đã được duyệt trước đó" });
		user.IsApproved = true;
		await _db.SaveChangesAsync();
		return Ok(new { message = "Duyệt tài khoản giáo viên thành công" });
	}
}


