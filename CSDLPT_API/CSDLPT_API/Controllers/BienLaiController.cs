using CSDLPT_API.Context;
using CSDLPT_API.Entities;
using CSDLPT_API.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CSDLPT_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BienLaiController : ControllerBase
{
	private readonly MyDbContext _db;

	public BienLaiController(MyDbContext db)
	{
		_db = db;
	}

	[HttpGet]
	public IActionResult GetAll()
	{
		return Ok(_db.BienLais.ToList());
	}

	[HttpGet("{maLop}/{maSv}")]
	public async Task<IActionResult> GetByKey(string maLop, string maSv)
	{
		var item = await _db.BienLais.FindAsync(maLop, maSv);
		if (item == null) return NotFound();
		return Ok(item);
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] BienLaiCreateDto dto)
	{
		var entity = new BienLai
		{
			SoBienLai = dto.SoBienLai,
			Thang = dto.Thang,
			Nam = dto.Nam,
			MaLop = dto.MaLop,
			MaSv = dto.MaSv,
			SoTien = dto.SoTien
		};
		_db.BienLais.Add(entity);
		await _db.SaveChangesAsync();
		return CreatedAtAction(nameof(GetByKey), new { maLop = entity.MaLop, maSv = entity.MaSv }, entity);
	}

	[HttpPut("{maLop}/{maSv}")]
	public async Task<IActionResult> Update(string maLop, string maSv, [FromBody] BienLaiUpdateDto dto)
	{
		var item = await _db.BienLais.FindAsync(maLop, maSv);
		if (item == null) return NotFound();
		item.SoBienLai = dto.SoBienLai;
		item.Thang = dto.Thang;
		item.Nam = dto.Nam;
		item.SoTien = dto.SoTien;
		await _db.SaveChangesAsync();
		return Ok(item);
	}

	[HttpDelete("{maLop}/{maSv}")]
	public async Task<IActionResult> Delete(string maLop, string maSv)
	{
		var item = await _db.BienLais.FindAsync(maLop, maSv);
		if (item == null) return NotFound();
		_db.BienLais.Remove(item);
		await _db.SaveChangesAsync();
		return NoContent();
	}
}


