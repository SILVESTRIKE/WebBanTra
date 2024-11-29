using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebBanTra.Models
{
    public class NhanVien
    {
        [Key]
        public int MaNV { get; set; } // Primary key, auto-incremented (IDENTITY in SQL)

        public int MaTK { get; set; } // Foreign key to TaiKhoan

        [Required]
        [StringLength(255)]
        public string TenNV { get; set; } // Tên nhân viên

        [StringLength(10)]
        public string SDT { get; set; } // Số điện thoại của nhân viên

        [Required]
        [StringLength(255)]
        [EmailAddress]
        public string Email { get; set; } // Email của nhân viên, phải là duy nhất (Unique)

        [StringLength(255)]
        public string DiaChi { get; set; } // Địa chỉ của nhân viên

        [StringLength(50)]
        public string ChucVu { get; set; } // Chức vụ của nhân viên

        public int? MaQuanLy { get; set; } // Mã quản lý (có thể là null nếu không có quản lý)

        // Navigation property - Quan hệ với bảng NhanVien (Quản lý)
        [ForeignKey("MaQuanLy")]
        public NhanVien QuanLy { get; set; } // Quản lý của nhân viên

        // Navigation property - Quan hệ với bảng TaiKhoan
        [ForeignKey("MaTK")]
        public TaiKhoan TaiKhoan { get; set; } // Tài khoản của nhân viên
    }
}