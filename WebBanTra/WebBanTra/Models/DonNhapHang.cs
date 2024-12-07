using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebBanTra.Models
{
    public class DonNhapHang
    {
        [Key]
        public int MaDNH { get; set; } // Khóa chính, tự động tăng (IDENTITY trong SQL)

        [ForeignKey("NhaCungCap")]
        public int MaNCC { get; set; } // Mã nhà cung cấp

        [Required]
        public DateTime NgayDat { get; set; } // Ngày đặt

        [Range(0.01, double.MaxValue, ErrorMessage = "Tổng tiền phải lớn hơn 0.")]
        public decimal TongTien { get; set; } // Tổng tiền

        // Thuộc tính mới
        public bool TrangThai { get; set; } // Trạng thái (Boolean)

        [ForeignKey("NhanVien")]
        public int MaNV { get; set; } // Mã nhân viên

        // Navigation properties
        public virtual NhaCungCap NhaCungCap { get; set; } // Quan hệ với bảng NhaCungCap
        public virtual NhanVien NhanVien { get; set; } // Quan hệ với bảng NhanVien
    }
}