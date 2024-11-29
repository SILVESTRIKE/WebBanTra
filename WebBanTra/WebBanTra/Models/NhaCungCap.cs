using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebBanTra.Models
{
    public class NhaCungCap
    {
        [Key]
        public int MaNCC { get; set; } // Khóa chính, tự động tăng (IDENTITY trong SQL)

        [Required]
        [StringLength(255)]
        public string TenNCC { get; set; } // Tên nhà cung cấp

        [StringLength(255)]
        public string DiaChi { get; set; } // Địa chỉ nhà cung cấp

        [StringLength(10)]
        public string SDT { get; set; } // Số điện thoại nhà cung cấp

        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } // Email nhà cung cấp
    }
}