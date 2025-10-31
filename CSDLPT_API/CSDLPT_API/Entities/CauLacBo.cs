using System;
using System.Collections.Generic;

namespace CSDLPT_API.Entities;

public partial class CauLacBo
{
    public string MaClb { get; set; } = null!;

    public string TenClb { get; set; } = null!;

    public string TenKhoa { get; set; } = null!;

    public Guid Rowguid { get; set; }

    public virtual ICollection<GiangVien> GiangViens { get; set; } = new List<GiangVien>();

    public virtual ICollection<Sinhvien> Sinhviens { get; set; } = new List<Sinhvien>();
}
