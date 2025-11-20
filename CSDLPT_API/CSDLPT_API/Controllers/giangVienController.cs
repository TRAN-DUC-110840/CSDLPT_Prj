using CSDLPT_API.Context;
using CSDLPT_API.Entities;
using CSDLPT_API.Dtos;
using CSDLPT_API.Enums;
using CSDLPT_API.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CSDLPT_API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class giangVienController : ControllerBase
{
    private readonly serverSwitcherHelper _serverSwitcherHelper;

    public giangVienController(serverSwitcherHelper serverSwitcherHelper)
    {
        this._serverSwitcherHelper = serverSwitcherHelper;
    }


	[HttpGet]
	public async Task<IActionResult> getAll(serverEnum serverEnum)
	{
		var _db = _serverSwitcherHelper.serverChangerHelper(serverEnum);

		var list = await Task.FromResult(_db.GiangViens.ToList());
		return Ok(list);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> getById(string id , serverEnum serverEnum)
	{
		var _db = _serverSwitcherHelper.serverChangerHelper(serverEnum);

		var gv = await _db.GiangViens.FindAsync(id);
		if (gv == null) return NotFound();
		return Ok(gv);
	}

	[HttpPost]
	public async Task<IActionResult> postGvData([FromBody] GiangVienCreateDto gvDto , serverEnum serverEnum)
    {
	    var _db = _serverSwitcherHelper.serverChangerHelper(serverEnum);

        var transaction = await _db.BeginTransactionAsync();
        
        try
		{
			var nextId = IdHelper.GenerateNextId(_db.GiangViens.Select(x => x.MaGv), "GV", 3);
			var gvObject = new GiangVien()
            {
				HoTenGv = gvDto.HoTenGv,
				MaClb = gvDto.MaClb,
				MaGv = nextId,
            };

            await _db.GiangViens.AddAsync(gvObject);
            await _db.SaveChangesAsync();
            await transaction.CommitAsync();
			return CreatedAtAction(nameof(getById), new { id = gvObject.MaGv }, gvObject);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
			return BadRequest(new { message = $"Them Giang Vien That Bai {ex.Message}" });
        }
    }

	[HttpPut("{id}")]
	public async Task<IActionResult> putGvData(string id, [FromBody] GiangVienUpdateDto gvDto , serverEnum serverEnum)
	{
		var _db = _serverSwitcherHelper.serverChangerHelper(serverEnum);

		var gv = await _db.GiangViens.FindAsync(id);
		if (gv == null) return NotFound();
		gv.HoTenGv = gvDto.HoTenGv;
		gv.MaClb = gvDto.MaClb;
		await _db.SaveChangesAsync();
		return Ok(gv);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> deleteGv(string id , serverEnum serverEnum)
	{
		var _db = _serverSwitcherHelper.serverChangerHelper(serverEnum);

		var gv = await _db.GiangViens.FindAsync(id);
		if (gv == null) return NotFound();
		_db.GiangViens.Remove(gv);
		await _db.SaveChangesAsync();
		return NoContent();
	}
}