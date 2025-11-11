using CSDLPT_API.Context;
using CSDLPT_API.Entities;
using CSDLPT_API.Dtos;
using CSDLPT_API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSDLPT_API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]

public class CauLacBoController : ControllerBase
{
	private readonly MyDbContext _db;

	public CauLacBoController(MyDbContext db)
	{
		_db = db;
	}

	[HttpGet]
	public IActionResult GetAll()
	{
		return Ok(_db.CauLacBos.ToList());
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(string id)
	{
		var item = await _db.CauLacBos.FindAsync(id);
		if (item == null) return NotFound();
		return Ok(item);
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CauLacBoCreateDto dto)
	{
		if (string.IsNullOrWhiteSpace(dto.TenKhoa) || !dto.TenKhoa.StartsWith("K"))
			return BadRequest(new { message = "TenKhoa phải bắt đầu bằng 'K'" });

		var entity = new CauLacBo
		{
			MaClb = IdHelper.GenerateNextId(_db.CauLacBos.Select(x => x.MaClb), "CLB", 2),
			TenClb = dto.TenClb,
			TenKhoa = dto.TenKhoa
		};
		_db.CauLacBos.Add(entity);
		await _db.SaveChangesAsync();
		return CreatedAtAction(nameof(GetById), new { id = entity.MaClb }, entity);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> Update(string id, [FromBody] CauLacBoUpdateDto dto)
	{
		var item = await _db.CauLacBos.FindAsync(id);
		if (item == null) return NotFound();
		if (string.IsNullOrWhiteSpace(dto.TenKhoa) || !dto.TenKhoa.StartsWith("K"))
			return BadRequest(new { message = "TenKhoa phải bắt đầu bằng 'K'" });
		item.TenClb = dto.TenClb;
		item.TenKhoa = dto.TenKhoa;
		await _db.SaveChangesAsync();
		return Ok(item);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(string id)
	{
		var item = await _db.CauLacBos.FindAsync(id);
		if (item == null) return NotFound();
		_db.CauLacBos.Remove(item);
		await _db.SaveChangesAsync();
		return NoContent();
	}
}


