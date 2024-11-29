using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebBanTra.Models
{
    public class HoaDon
    {
         [Key]
        public int MaHD { get; set; } // Khóa chính, tự động tăng (IDENTITY trong SQL)

        [ForeignKey("DonHang")]
        public int MaDH { get; set; } // Mã đơn hàng

        [Required]
        [StringLength(255)]
        [RegularExpression(@"^(Đã thanh toán|Chưa thanh toán)$", ErrorMessage = "Trạng thái thanh toán không hợp lệ.")]
        public string TrangThaiThanhToan { get; set; }  // Trạng thái thanh toán (mặc định là 'Chưa thanh toán')

        [Required]
        public DateTime NgayLap { get; set; } // Ngày lập hóa đơn

        // Navigation property
        public virtual DonHang DonHang { get; set; } // Quan hệ với bảng DonHang
        public HoaDon ()
        {
            TrangThaiThanhToan = "Chưa thanh toán";
        }
    }
}