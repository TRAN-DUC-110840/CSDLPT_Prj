using System;
using System.Collections.Generic;

namespace CSDLPT_API.Entities;

public partial class BienLai
{
    public long? SoBienLai { get; set; }

    public long Thang { get; set; }

    public long Nam { get; set; }

    public string MaLop { get; set; } = null!;

    public string MaSv { get; set; } = null!;

    public decimal SoTien { get; set; }

    public Guid Rowguid { get; set; }

    public virtual LopNangKhieu MaLopNavigation { get; set; } = null!;

    public virtual Sinhvien MaSvNavigation { get; set; } = null!;
}
