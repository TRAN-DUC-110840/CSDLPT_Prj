using CSDLPT_API.Context;
using CSDLPT_API.Entities;
using CSDLPT_API.Dtos;
using CSDLPT_API.Utils;
using Microsoft.AspNetCore.Mvc;

namespace CSDLPT_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SinhvienController : ControllerBase
{
	private readonly MyDbContext _db;

	public SinhvienController(MyDbContext db)
	{
		_db = db;
	}

	[HttpGet]
	public IActionResult GetAll()
	{
		return Ok(_db.Sinhviens.ToList());
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(string id)
	{
		var item = await _db.Sinhviens.FindAsync(id);
		if (item == null) return NotFound();
		return Ok(item);
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] SinhvienCreateDto dto)
	{
		var entity = new Sinhvien
		{
			MaSv = IdHelper.GenerateNextId(_db.Sinhviens.Select(x => x.MaSv), "SV", 3),
			TenSv = dto.TenSv,
			MaClb = dto.MaClb
		};
		_db.Sinhviens.Add(entity);
		await _db.SaveChangesAsync();
		return CreatedAtAction(nameof(GetById), new { id = entity.MaSv }, entity);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> Update(string id, [FromBody] SinhvienUpdateDto dto)
	{
		var item = await _db.Sinhviens.FindAsync(id);
		if (item == null) return NotFound();
		item.TenSv = dto.TenSv;
		item.MaClb = dto.MaClb;
		await _db.SaveChangesAsync();
		return Ok(item);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(string id)
	{
		var item = await _db.Sinhviens.FindAsync(id);
		if (item == null) return NotFound();
		_db.Sinhviens.Remove(item);
		await _db.SaveChangesAsync();
		return NoContent();
	}
}


