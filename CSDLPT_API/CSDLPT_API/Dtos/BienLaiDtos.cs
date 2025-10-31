namespace CSDLPT_API.Dtos;

public class BienLaiCreateDto
{
	public long? SoBienLai { get; set; }
	public long Thang { get; set; }
	public long Nam { get; set; }
	public string MaLop { get; set; }
	public string MaSv { get; set; }
	public decimal SoTien { get; set; }
}

public class BienLaiUpdateDto
{
	public long? SoBienLai { get; set; }
	public long Thang { get; set; }
	public long Nam { get; set; }
	public decimal SoTien { get; set; }
}


