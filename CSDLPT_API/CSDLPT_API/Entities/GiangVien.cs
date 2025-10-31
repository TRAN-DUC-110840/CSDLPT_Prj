using System;
using System.Collections.Generic;

namespace CSDLPT_API.Entities;

public partial class GiangVien
{
    public string MaGv { get; set; } = null!;

    public string HoTenGv { get; set; } = null!;

    public string MaClb { get; set; } = null!;

    public Guid Rowguid { get; set; }

    public virtual ICollection<LopNangKhieu> LopNangKhieus { get; set; } = new List<LopNangKhieu>();

    public virtual CauLacBo MaClbNavigation { get; set; } = null!;
}
