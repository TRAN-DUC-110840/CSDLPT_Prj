using System;
using System.Collections.Generic;

namespace CSDLPT_API.Entities;

public partial class Sinhvien
{
    public string MaSv { get; set; } = null!;

    public string TenSv { get; set; } = null!;

    public string MaClb { get; set; } = null!;

    public Guid Rowguid { get; set; }

    public virtual ICollection<BienLai> BienLais { get; set; } = new List<BienLai>();

    public virtual CauLacBo MaClbNavigation { get; set; } = null!;
}
