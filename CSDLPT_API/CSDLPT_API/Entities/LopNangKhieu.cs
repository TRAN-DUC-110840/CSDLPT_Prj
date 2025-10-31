using System;
using System.Collections.Generic;

namespace CSDLPT_API.Entities;

public partial class LopNangKhieu
{
    public string MaLop { get; set; } = null!;

    public DateTime NgayMo { get; set; }

    public string MaGv { get; set; } = null!;

    public long HocPhi { get; set; }

    public Guid Rowguid { get; set; }

    public virtual ICollection<BienLai> BienLais { get; set; } = new List<BienLai>();

    public virtual GiangVien MaGvNavigation { get; set; } = null!;
}
