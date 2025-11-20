using CSDLPT_API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CSDLPT_API.Interfaces;

public interface IDbContext
{
    DbSet<BienLai> BienLais { get; set; }

    DbSet<CauLacBo> CauLacBos { get; set; }

    DbSet<GiangVien> GiangViens { get; set; }

    DbSet<LopNangKhieu> LopNangKhieus { get; set; }

    DbSet<Sinhvien> Sinhviens { get; set; }

    Task SaveChangesAsync();
    
    Task<IDbContextTransaction> BeginTransactionAsync();

    Task CommitTransactionAsync(IDbContextTransaction  transaction);

    Task RollbackTransactionAsync(IDbContextTransaction  transaction);
}