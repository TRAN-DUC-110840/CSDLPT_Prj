using CSDLPT_API.Context;
using CSDLPT_API.Entities;
using CSDLPT_API.Dtos;
using CSDLPT_API.Utils;
using Microsoft.AspNetCore.Mvc;

namespace CSDLPT_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class giangVienController : ControllerBase
{
    private readonly MyDbContext myDbContext;

    public giangVienController(MyDbContext myDbContext)
    {
        this.myDbContext = myDbContext;
    }


	[HttpGet]
	public async Task<IActionResult> getAll()
	{
		var list = await Task.FromResult(myDbContext.GiangViens.ToList());
		return Ok(list);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> getById(string id)
	{
		var gv = await myDbContext.GiangViens.FindAsync(id);
		if (gv == null) return NotFound();
		return Ok(gv);
	}

	[HttpPost]
	public async Task<IActionResult> postGvData([FromBody] GiangVienCreateDto gvDto)
    {
        var transaction = await myDbContext.Database.BeginTransactionAsync();
        try
		{
			var nextId = IdHelper.GenerateNextId(myDbContext.GiangViens.Select(x => x.MaGv), "GV", 3);
			var gvObject = new GiangVien()
            {
				HoTenGv = gvDto.HoTenGv,
				MaClb = gvDto.MaClb,
				MaGv = nextId,
            };

            await myDbContext.GiangViens.AddAsync(gvObject);
            await myDbContext.SaveChangesAsync();
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
	public async Task<IActionResult> putGvData(string id, [FromBody] GiangVienUpdateDto gvDto)
	{
		var gv = await myDbContext.GiangViens.FindAsync(id);
		if (gv == null) return NotFound();
		gv.HoTenGv = gvDto.HoTenGv;
		gv.MaClb = gvDto.MaClb;
		await myDbContext.SaveChangesAsync();
		return Ok(gv);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> deleteGv(string id)
	{
		var gv = await myDbContext.GiangViens.FindAsync(id);
		if (gv == null) return NotFound();
		myDbContext.GiangViens.Remove(gv);
		await myDbContext.SaveChangesAsync();
		return NoContent();
	}
}