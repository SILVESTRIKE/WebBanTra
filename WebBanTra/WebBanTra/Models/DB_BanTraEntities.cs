using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace WebBanTra.Models
{
    public class DB_BanTraEntities : DbContext
    {
        public DB_BanTraEntities() : base("DB_BanTraEntities") { }

        // Các DbSet cho mỗi bảng trong cơ sở dữ liệu
        public DbSet<TaiKhoan> TaiKhoans { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<NhanVien> NhanViens { get; set; }
        public DbSet<DanhMuc> DanhMucs { get; set; }
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<Anh_SanPham> AnhSanPhams { get; set; }
        public DbSet<MoTa_SanPham> MoTaSanPhams { get; set; }
        public DbSet<NhaCungCap> NhaCungCaps { get; set; }
        public DbSet<DonHang> DonHangs { get; set; }
        public DbSet<ChiTietDH> ChiTietDHs { get; set; }
        public DbSet<HoaDon> HoaDons { get; set; }
        public DbSet<DonNhapHang> DonNhapHangs { get; set; }
        public DbSet<ChiTietDNH> ChiTietDNHs { get; set; }
        public DbSet<DanhGia> DanhGias { get; set; }
    }
}