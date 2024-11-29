using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebBanTra.Models
{
    public class TaiKhoan
    {
        [Key]
        public int MaTK { get; set; } // Primary key, auto-incremented (IDENTITY in SQL)

        [Required]
        [StringLength(255)]
      
        public string TenDangNhap { get; set; }
        
        [Required]
        [StringLength(255)]
        public string MatKhau { get; set; }

        [Required]
        [StringLength(50)]
        public string VaiTro { get; set; } // Mặc định là 'Khách hàng'

        [Required]
        [StringLength(50)]
        public string TrangThai { get; set; }  // Mặc định là 'Không Đăng Nhập'

        public TaiKhoan()
        {
            VaiTro = "Khách hàng";
            TrangThai = "Không Đăng Nhập";
        }
    }
}