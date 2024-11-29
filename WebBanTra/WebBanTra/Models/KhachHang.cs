using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebBanTra.Models
{
    public class KhachHang
    {
        [Key]
        public int MaKH { get; set; } // Primary key, auto-incremented (IDENTITY in SQL)

        public int MaTK { get; set; } // Foreign key to TaiKhoan

        [Required]
        [StringLength(255)]
        public string TenKH { get; set; } // Tên khách hàng

        [StringLength(10)]
        public string GioiTinh { get; set; } // Giới tính (Nam, Nữ, v.v.)

        [Required]
        [StringLength(255)]
        [EmailAddress]
        public string Email { get; set; } // Địa chỉ email, phải là duy nhất (Unique)

        [StringLength(10)]
        public string SDT { get; set; } // Số điện thoại

        [StringLength(255)]
        public string DiaChi { get; set; } // Địa chỉ của khách hàng

        public DateTime? NgaySinh { get; set; } // Ngày sinh của khách hàng

        // Navigation property
        [ForeignKey("MaTK")]
        public virtual TaiKhoan TaiKhoan { get; set; } // Mối quan hệ với bảng TaiKhoan
    }
}