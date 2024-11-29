using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebBanTra.Models
{

    public class DonHang
    {
        [Key]
        public int MaDH { get; set; } // Khóa chính, tự động tăng (IDENTITY trong SQL)

        [ForeignKey("KhachHang")]
        public int MaKH { get; set; } // Mã khách hàng

        [ForeignKey("NhanVien")]
        public int? MaNV { get; set; } // Mã nhân viên xử lý đơn hàng

        [Required]
        public DateTime NgayDat { get; set; } // Ngày đặt

        [Range(0, int.MaxValue)]
        public int TongTien { get; set; } // Tổng tiền

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^(Chờ xác nhận|Đã giao|Chưa giao)$", ErrorMessage = "Trạng thái không hợp lệ.")]
        public string TrangThai { get; set; }  // Trạng thái đơn hàng

        // Navigation properties
        public virtual KhachHang KhachHang { get; set; } // Quan hệ với bảng KhachHang
        public virtual NhanVien NhanVien { get; set; } // Quan hệ với bảng NhanVien
        public DonHang()
        {
            TrangThai = "Chờ xác nhận";
        }
    }
}