using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanTra.Models
{
    public class ChiTietDH
    {
        [Key]
        public int MaCTDH { get; set; } // Khóa chính, tự động tăng (IDENTITY trong SQL)

        [ForeignKey("DonHang")]
        public int MaDH { get; set; } // Mã đơn hàng

        [ForeignKey("SanPham")]
        public int MaSP { get; set; } // Mã sản phẩm

        [Range(1, int.MaxValue, ErrorMessage = "Số lượng mua phải lớn hơn 0.")]
        public int SoLuongMua { get; set; } // Số lượng mua

        [StringLength(255)]
        public string YeuCau { get; set; } // Yêu cầu đặc biệt (nếu có)

        // Navigation properties
        public virtual DonHang DonHang { get; set; } // Quan hệ với bảng DonHang
        public virtual SanPham SanPham { get; set; } // Quan hệ với bảng SanPham

        public ChiTietDH()
        {
            SoLuongMua = 1;
        }
    }
}