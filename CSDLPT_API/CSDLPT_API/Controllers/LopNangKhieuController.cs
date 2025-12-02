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
public class LopNangKhieuController : ControllerBase
{
	private readonly serverSwitcherHelper _serverSwitcherHelper;

	public LopNangKhieuController(serverSwitcherHelper serverSwitcherHelper)
	{
		this._serverSwitcherHelper = serverSwitcherHelper;
	}

	[HttpGet]
	public IActionResult GetAll(serverEnum serverEnum)
	{
		var _db = _serverSwitcherHelper.serverChangerHelper(serverEnum);

		return Ok(_db.LopNangKhieus.ToList());
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(string id , serverEnum serverEnum)
	{
		var _db = _serverSwitcherHelper.serverChangerHelper(serverEnum);

		var item = await _db.LopNangKhieus.FindAsync(id);
		if (item == null) return NotFound();
		return Ok(item);
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] LopNangKhieuCreateDto dto , serverEnum serverEnum)
	{
		var _db = _serverSwitcherHelper.serverChangerHelper(serverEnum);

		var entity = new LopNangKhieu
		{
			MaLop = IdHelper.GenerateRandomId(_db.LopNangKhieus.Select(x => x.MaLop)),
			NgayMo = dto.NgayMo,
			MaGv = dto.MaGv,
			HocPhi = dto.HocPhi
		};
		_db.LopNangKhieus.Add(entity);
		await _db.SaveChangesAsync();
		return CreatedAtAction(nameof(GetById), new { id = entity.MaLop }, entity);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> Update(string id, [FromBody] LopNangKhieuUpdateDto dto , serverEnum serverEnum)
	{
		var _db = _serverSwitcherHelper.serverChangerHelper(serverEnum);

		var item = await _db.LopNangKhieus.FindAsync(id);
		if (item == null) return NotFound();
		item.NgayMo = dto.NgayMo;
		item.MaGv = dto.MaGv;
		item.HocPhi = dto.HocPhi;
		await _db.SaveChangesAsync();
		return Ok(item);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(string id , serverEnum serverEnum)
	{
		var _db = _serverSwitcherHelper.serverChangerHelper(serverEnum);

		var item = await _db.LopNangKhieus.FindAsync(id);
		if (item == null) return NotFound();
		_db.LopNangKhieus.Remove(item);
		await _db.SaveChangesAsync();
		return NoContent();
	}
}


