using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebBanTra.Models
{
    public class ChiTietDNH
    {
        // Khóa chính kết hợp (MaDNH + MaSP)
        [Key, Column(Order = 0)]
        public int MaDNH { get; set; } // Mã đơn nhập hàng

        [Key, Column(Order = 1)]
        public int MaSP { get; set; } // Mã sản phẩm

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng nhập phải lớn hơn 0.")]
        public int SoLuongNhap { get; set; } // Số lượng nhập, kiểm tra lớn hơn 0

        [Required]
        public int GiaNhap { get; set; } // Giá nhập

        // Navigation properties
        [ForeignKey("MaDNH")]
        public virtual DonNhapHang DonNhapHang { get; set; }

        [ForeignKey("MaSP")]
        public virtual SanPham SanPham { get; set; }

        public ChiTietDNH()
        {
            SoLuongNhap = 0;
            GiaNhap = 0;
        }
    }
}