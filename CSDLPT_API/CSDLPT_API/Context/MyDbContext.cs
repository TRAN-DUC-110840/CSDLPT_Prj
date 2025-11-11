using System;
using System.Collections.Generic;
using CSDLPT_API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CSDLPT_API.Context;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BienLai> BienLais { get; set; }

    public virtual DbSet<CauLacBo> CauLacBos { get; set; }

    public virtual DbSet<GiangVien> GiangViens { get; set; }

    public virtual DbSet<LopNangKhieu> LopNangKhieus { get; set; }

    public virtual DbSet<Sinhvien> Sinhviens { get; set; }
	public virtual DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BienLai>(entity =>
        {
            entity.HasKey(e => new { e.MaLop, e.MaSv }).HasName("bienlai_malop_sv_primary");

            entity.ToTable("BienLai", tb =>
                {
                    tb.HasTrigger("MSmerge_del_2B4D61663ED5437485DA13BF01E2A094");
                    tb.HasTrigger("MSmerge_ins_2B4D61663ED5437485DA13BF01E2A094");
                    tb.HasTrigger("MSmerge_upd_2B4D61663ED5437485DA13BF01E2A094");
                });

            entity.HasIndex(e => e.Rowguid, "MSmerge_index_1029578706").IsUnique();

            entity.HasIndex(e => e.SoBienLai, "bienlai_sobienlai_unique").IsUnique();

            entity.Property(e => e.MaLop)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("maLop");
            entity.Property(e => e.MaSv)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("maSV");
            entity.Property(e => e.Nam).HasColumnName("nam");
            entity.Property(e => e.Rowguid)
                .HasDefaultValueSql("(newsequentialid())")
                .HasColumnName("rowguid");
            entity.Property(e => e.SoTien)
                .HasColumnType("decimal(18, 5)")
                .HasColumnName("soTien");
            entity.Property(e => e.Thang).HasColumnName("thang");

            entity.HasOne(d => d.MaLopNavigation).WithMany(p => p.BienLais)
                .HasForeignKey(d => d.MaLop)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("bienlai_malop_foreign");

            entity.HasOne(d => d.MaSvNavigation).WithMany(p => p.BienLais)
                .HasForeignKey(d => d.MaSv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("bienlai_masv_foreign");
        });

        modelBuilder.Entity<CauLacBo>(entity =>
        {
            entity.HasKey(e => e.MaClb).HasName("cau_lac_bo_maclb_primary");

            entity.ToTable("Cau_Lac_Bo", tb =>
                {
                    tb.HasTrigger("MSmerge_del_8090A7E0E09A4934B1C4A47D77456749");
                    tb.HasTrigger("MSmerge_ins_8090A7E0E09A4934B1C4A47D77456749");
                    tb.HasTrigger("MSmerge_upd_8090A7E0E09A4934B1C4A47D77456749");
                });

            entity.HasIndex(e => e.Rowguid, "MSmerge_index_901578250").IsUnique();

            entity.Property(e => e.MaClb)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("maCLB");
            entity.Property(e => e.Rowguid)
                .HasDefaultValueSql("(newsequentialid())")
                .HasColumnName("rowguid");
            entity.Property(e => e.TenClb)
                .HasMaxLength(255)
                .HasColumnName("tenCLB");
            entity.Property(e => e.TenKhoa)
                .HasMaxLength(255)
                .HasColumnName("tenKhoa");
        });

        modelBuilder.Entity<GiangVien>(entity =>
        {
            entity.HasKey(e => e.MaGv).HasName("giang_vien_magv_primary");

            entity.ToTable("Giang_Vien", tb =>
                {
                    tb.HasTrigger("MSmerge_del_BBE491BE857B46C1A1B3972E16EFA82F");
                    tb.HasTrigger("MSmerge_ins_BBE491BE857B46C1A1B3972E16EFA82F");
                    tb.HasTrigger("MSmerge_upd_BBE491BE857B46C1A1B3972E16EFA82F");
                });

            entity.HasIndex(e => e.Rowguid, "MSmerge_index_933578364").IsUnique();

            entity.Property(e => e.MaGv)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("maGV");
            entity.Property(e => e.HoTenGv)
                .HasMaxLength(255)
                .HasColumnName("hoTenGV");
            entity.Property(e => e.MaClb)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("maCLB");
            entity.Property(e => e.Rowguid)
                .HasDefaultValueSql("(newsequentialid())")
                .HasColumnName("rowguid");

            entity.HasOne(d => d.MaClbNavigation).WithMany(p => p.GiangViens)
                .HasForeignKey(d => d.MaClb)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("giang_vien_maclb_foreign");
        });

        modelBuilder.Entity<LopNangKhieu>(entity =>
        {
            entity.HasKey(e => e.MaLop).HasName("lopnangkhieu_malop_primary");

            entity.ToTable("LopNangKhieu", tb =>
                {
                    tb.HasTrigger("MSmerge_del_C310E0F273B1411B9D3E7C509BC7FC97");
                    tb.HasTrigger("MSmerge_ins_C310E0F273B1411B9D3E7C509BC7FC97");
                    tb.HasTrigger("MSmerge_upd_C310E0F273B1411B9D3E7C509BC7FC97");
                });

            entity.HasIndex(e => e.Rowguid, "MSmerge_index_997578592").IsUnique();

            entity.Property(e => e.MaLop)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("maLop");
            entity.Property(e => e.HocPhi).HasColumnName("hocPhi");
            entity.Property(e => e.MaGv)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("maGV");
            entity.Property(e => e.NgayMo)
                .HasColumnType("datetime")
                .HasColumnName("ngayMo");
            entity.Property(e => e.Rowguid)
                .HasDefaultValueSql("(newsequentialid())")
                .HasColumnName("rowguid");

            entity.HasOne(d => d.MaGvNavigation).WithMany(p => p.LopNangKhieus)
                .HasForeignKey(d => d.MaGv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("lopnangkhieu_magv_foreign");
        });

        modelBuilder.Entity<Sinhvien>(entity =>
        {
            entity.HasKey(e => e.MaSv).HasName("sinhvien_masv_primary");

            entity.ToTable("Sinhvien", tb =>
                {
                    tb.HasTrigger("MSmerge_del_A5F875CD00564922B0F0411FB1DFD078");
                    tb.HasTrigger("MSmerge_ins_A5F875CD00564922B0F0411FB1DFD078");
                    tb.HasTrigger("MSmerge_upd_A5F875CD00564922B0F0411FB1DFD078");
                });

            entity.HasIndex(e => e.Rowguid, "MSmerge_index_965578478").IsUnique();

            entity.Property(e => e.MaSv)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("maSV");
            entity.Property(e => e.MaClb)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("maCLB");
            entity.Property(e => e.Rowguid)
                .HasDefaultValueSql("(newsequentialid())")
                .HasColumnName("rowguid");
            entity.Property(e => e.TenSv)
                .HasMaxLength(255)
                .HasColumnName("tenSV");

            entity.HasOne(d => d.MaClbNavigation).WithMany(p => p.Sinhviens)
                .HasForeignKey(d => d.MaClb)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sinhvien_maclb_foreign");
        });

		modelBuilder.Entity<User>(entity =>
		{
			entity.HasIndex(e => e.Username).IsUnique();
		});

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
