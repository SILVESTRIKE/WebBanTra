namespace WebBanTra.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialDataBase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Anh_SanPham",
                c => new
                {
                    MaAnh = c.Int(nullable: false, identity: true),
                    MaSP = c.Int(nullable: false),
                    LinhAnh = c.String(maxLength: 255),
                })
                .PrimaryKey(t => new { t.MaAnh, t.MaSP })
                .ForeignKey("dbo.SanPhams", t => t.MaSP, cascadeDelete: true)
                .Index(t => t.MaSP);

            CreateTable(
                "dbo.SanPhams",
                c => new
                {
                    MaSP = c.Int(nullable: false, identity: true),
                    TenSP = c.String(nullable: false, maxLength: 255),
                    Gia = c.Int(nullable: false),
                    SoLuongTon = c.Int(nullable: false),
                    MaDM = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.MaSP)
                .ForeignKey("dbo.DanhMucs", t => t.MaDM, cascadeDelete: true)
                .Index(t => t.MaDM);

            CreateTable(
                "dbo.DanhMucs",
                c => new
                {
                    MaDM = c.Int(nullable: false, identity: true),
                    TenDM = c.String(nullable: false, maxLength: 50),
                })
                .PrimaryKey(t => t.MaDM);

            CreateTable(
                "dbo.ChiTietDHs",
                c => new
                {
                    MaCTDH = c.Int(nullable: false, identity: true),
                    MaDH = c.Int(nullable: false),
                    MaSP = c.Int(nullable: false),
                    SoLuongMua = c.Int(nullable: false),
                    YeuCau = c.String(maxLength: 255),
                })
                .PrimaryKey(t => t.MaCTDH)
                .ForeignKey("dbo.DonHangs", t => t.MaDH, cascadeDelete: true)
                .ForeignKey("dbo.SanPhams", t => t.MaSP, cascadeDelete: true)
                .Index(t => t.MaDH)
                .Index(t => t.MaSP);

            CreateTable(
                "dbo.DonHangs",
                c => new
                {
                    MaDH = c.Int(nullable: false, identity: true),
                    MaKH = c.Int(nullable: false),
                    MaNV = c.Int(),
                    NgayDat = c.DateTime(nullable: false),
                    TongTien = c.Int(nullable: false),
                    TrangThai = c.String(nullable: false, maxLength: 50),
                })
                .PrimaryKey(t => t.MaDH)
                .ForeignKey("dbo.KhachHangs", t => t.MaKH, cascadeDelete: true)
                .ForeignKey("dbo.NhanViens", t => t.MaNV)
                .Index(t => t.MaKH)
                .Index(t => t.MaNV);

            CreateTable(
                "dbo.KhachHangs",
                c => new
                {
                    MaKH = c.Int(nullable: false, identity: true),
                    MaTK = c.Int(nullable: false),
                    TenKH = c.String(nullable: false, maxLength: 255),
                    GioiTinh = c.String(maxLength: 10),
                    Email = c.String(nullable: false, maxLength: 255),
                    SDT = c.String(maxLength: 10),
                    DiaChi = c.String(maxLength: 255),
                    NgaySinh = c.DateTime(),
                })
                .PrimaryKey(t => t.MaKH)
                .ForeignKey("dbo.TaiKhoans", t => t.MaTK, cascadeDelete: true)
                .Index(t => t.MaTK);

            CreateTable(
                "dbo.TaiKhoans",
                c => new
                {
                    MaTK = c.Int(nullable: false, identity: true),
                    TenDangNhap = c.String(nullable: false, maxLength: 255),
                    MatKhau = c.String(nullable: false, maxLength: 255),
                    VaiTro = c.String(nullable: false, maxLength: 50),
                    TrangThai = c.String(nullable: false, maxLength: 50),
                })
                .PrimaryKey(t => t.MaTK);

            CreateTable(
                "dbo.NhanViens",
                c => new
                {
                    MaNV = c.Int(nullable: false, identity: true),
                    MaTK = c.Int(nullable: false),
                    TenNV = c.String(nullable: false, maxLength: 255),
                    SDT = c.String(maxLength: 10),
                    Email = c.String(nullable: false, maxLength: 255),
                    DiaChi = c.String(maxLength: 255),
                    ChucVu = c.String(maxLength: 50),
                    MaQuanLy = c.Int(),
                })
                .PrimaryKey(t => t.MaNV)
                .ForeignKey("dbo.NhanViens", t => t.MaQuanLy)
                .ForeignKey("dbo.TaiKhoans", t => t.MaTK, cascadeDelete: true)
                .Index(t => t.MaTK)
                .Index(t => t.MaQuanLy);

            CreateTable(
                "dbo.ChiTietDNHs",
                c => new
                {
                    MaDNH = c.Int(nullable: false),
                    MaSP = c.Int(nullable: false),
                    SoLuongNhap = c.Int(nullable: false),
                    GiaNhap = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.MaDNH, t.MaSP })
                .ForeignKey("dbo.DonNhapHangs", t => t.MaDNH, cascadeDelete: true)
                .ForeignKey("dbo.SanPhams", t => t.MaSP, cascadeDelete: true)
                .Index(t => t.MaDNH)
                .Index(t => t.MaSP);

            CreateTable(
                "dbo.DonNhapHangs",
                c => new
                {
                    MaDNH = c.Int(nullable: false, identity: true),
                    MaNCC = c.Int(nullable: false),
                    NgayDat = c.DateTime(nullable: false),
                    TongTien = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.MaDNH)
                .ForeignKey("dbo.NhaCungCaps", t => t.MaNCC, cascadeDelete: true)
                .Index(t => t.MaNCC);

            CreateTable(
                "dbo.NhaCungCaps",
                c => new
                {
                    MaNCC = c.Int(nullable: false, identity: true),
                    TenNCC = c.String(nullable: false, maxLength: 255),
                    DiaChi = c.String(maxLength: 255),
                    SDT = c.String(maxLength: 10),
                    Email = c.String(maxLength: 100),
                })
                .PrimaryKey(t => t.MaNCC);

            CreateTable(
                "dbo.DanhGias",
                c => new
                {
                    MaDG = c.Int(nullable: false, identity: true),
                    MaKH = c.Int(),
                    MaCTDH = c.Int(nullable: false),
                    DiemDG = c.Int(nullable: false),
                    BinhLuan = c.String(),
                    NgayDG = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.MaDG)
                .ForeignKey("dbo.ChiTietDHs", t => t.MaCTDH, cascadeDelete: true)
                .ForeignKey("dbo.KhachHangs", t => t.MaKH)
                .Index(t => t.MaKH)
                .Index(t => t.MaCTDH);

            CreateTable(
                "dbo.HoaDons",
                c => new
                {
                    MaHD = c.Int(nullable: false, identity: true),
                    MaDH = c.Int(nullable: false),
                    TrangThaiThanhToan = c.String(nullable: false, maxLength: 255),
                    NgayLap = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.MaHD)
                .ForeignKey("dbo.DonHangs", t => t.MaDH, cascadeDelete: true)
                .Index(t => t.MaDH);

            CreateTable(
                "dbo.MoTa_SanPham",
                c => new
                {
                    MaMoTa = c.Int(nullable: false, identity: true),
                    MaSP = c.Int(nullable: false),
                    MoTa = c.String(maxLength: 255),
                })
                .PrimaryKey(t => new { t.MaMoTa, t.MaSP })
                .ForeignKey("dbo.SanPhams", t => t.MaSP, cascadeDelete: true)
                .Index(t => t.MaSP);

        }

        public override void Down()
        {
            DropForeignKey("dbo.MoTa_SanPham", "MaSP", "dbo.SanPhams");
            DropForeignKey("dbo.HoaDons", "MaDH", "dbo.DonHangs");
            DropForeignKey("dbo.DanhGias", "MaKH", "dbo.KhachHangs");
            DropForeignKey("dbo.DanhGias", "MaCTDH", "dbo.ChiTietDHs");
            DropForeignKey("dbo.ChiTietDNHs", "MaSP", "dbo.SanPhams");
            DropForeignKey("dbo.ChiTietDNHs", "MaDNH", "dbo.DonNhapHangs");
            DropForeignKey("dbo.DonNhapHangs", "MaNCC", "dbo.NhaCungCaps");
            DropForeignKey("dbo.ChiTietDHs", "MaSP", "dbo.SanPhams");
            DropForeignKey("dbo.ChiTietDHs", "MaDH", "dbo.DonHangs");
            DropForeignKey("dbo.DonHangs", "MaNV", "dbo.NhanViens");
            DropForeignKey("dbo.NhanViens", "MaTK", "dbo.TaiKhoans");
            DropForeignKey("dbo.NhanViens", "MaQuanLy", "dbo.NhanViens");
            DropForeignKey("dbo.DonHangs", "MaKH", "dbo.KhachHangs");
            DropForeignKey("dbo.KhachHangs", "MaTK", "dbo.TaiKhoans");
            DropForeignKey("dbo.Anh_SanPham", "MaSP", "dbo.SanPhams");
            DropForeignKey("dbo.SanPhams", "MaDM", "dbo.DanhMucs");
            DropIndex("dbo.MoTa_SanPham", new[] { "MaSP" });
            DropIndex("dbo.HoaDons", new[] { "MaDH" });
            DropIndex("dbo.DanhGias", new[] { "MaCTDH" });
            DropIndex("dbo.DanhGias", new[] { "MaKH" });
            DropIndex("dbo.DonNhapHangs", new[] { "MaNCC" });
            DropIndex("dbo.ChiTietDNHs", new[] { "MaSP" });
            DropIndex("dbo.ChiTietDNHs", new[] { "MaDNH" });
            DropIndex("dbo.NhanViens", new[] { "MaQuanLy" });
            DropIndex("dbo.NhanViens", new[] { "MaTK" });
            DropIndex("dbo.KhachHangs", new[] { "MaTK" });
            DropIndex("dbo.DonHangs", new[] { "MaNV" });
            DropIndex("dbo.DonHangs", new[] { "MaKH" });
            DropIndex("dbo.ChiTietDHs", new[] { "MaSP" });
            DropIndex("dbo.ChiTietDHs", new[] { "MaDH" });
            DropIndex("dbo.SanPhams", new[] { "MaDM" });
            DropIndex("dbo.Anh_SanPham", new[] { "MaSP" });
            DropTable("dbo.MoTa_SanPham");
            DropTable("dbo.HoaDons");
            DropTable("dbo.DanhGias");
            DropTable("dbo.NhaCungCaps");
            DropTable("dbo.DonNhapHangs");
            DropTable("dbo.ChiTietDNHs");
            DropTable("dbo.NhanViens");
            DropTable("dbo.TaiKhoans");
            DropTable("dbo.KhachHangs");
            DropTable("dbo.DonHangs");
            DropTable("dbo.ChiTietDHs");
            DropTable("dbo.DanhMucs");
            DropTable("dbo.SanPhams");
            DropTable("dbo.Anh_SanPham");
        }
    }
}
