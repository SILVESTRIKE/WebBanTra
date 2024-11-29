using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace WebBanTra.Models
{
    public class DanhMuc
    {
        [Key]
        public int MaDM { get; set; } // Khóa chính, tự động tăng (IDENTITY trong SQL)

        [Required] // Yêu cầu trường TenDM không được null
        [StringLength(50)] // Giới hạn chiều dài tên danh mục
        public string TenDM { get; set; } // Tên danh mục
    }
}