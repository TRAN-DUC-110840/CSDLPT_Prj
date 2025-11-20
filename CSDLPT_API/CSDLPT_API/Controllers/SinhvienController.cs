using CSDLPT_API.Context;
using CSDLPT_API.Entities;
using CSDLPT_API.Dtos;
using CSDLPT_API.Enums;
using CSDLPT_API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSDLPT_API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SinhvienController : ControllerBase
{
	
	private readonly serverSwitcherHelper _serverSwitcherHelper;

	public SinhvienController(serverSwitcherHelper serverSwitcherHelper)
	{
		this._serverSwitcherHelper = serverSwitcherHelper;
	}

	[HttpGet]
	public IActionResult GetAll(serverEnum serverEnum)
	{
		var _db = _serverSwitcherHelper.serverChangerHelper(serverEnum);
		
		return Ok(_db.Sinhviens.ToList());
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(string id , serverEnum serverEnum)
	{
		var _db = _serverSwitcherHelper.serverChangerHelper(serverEnum);

		var item = await _db.Sinhviens.FindAsync(id);
		if (item == null) return NotFound();
		return Ok(item);
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] SinhvienCreateDto dto , serverEnum serverEnum)
	{
		var _db = _serverSwitcherHelper.serverChangerHelper(serverEnum);

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
	public async Task<IActionResult> Update(string id, [FromBody] SinhvienUpdateDto dto , serverEnum serverEnum)
	{
		var _db = _serverSwitcherHelper.serverChangerHelper(serverEnum);

		var item = await _db.Sinhviens.FindAsync(id);
		if (item == null) return NotFound();
		item.TenSv = dto.TenSv;
		item.MaClb = dto.MaClb;
		await _db.SaveChangesAsync();
		return Ok(item);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(string id , serverEnum serverEnum)
	{
		var _db = _serverSwitcherHelper.serverChangerHelper(serverEnum);

		var item = await _db.Sinhviens.FindAsync(id);
		if (item == null) return NotFound();
		_db.Sinhviens.Remove(item);
		await _db.SaveChangesAsync();
		return NoContent();
	}
}


