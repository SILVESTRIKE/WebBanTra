using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebBanTra.OOP
{
    public class AppUser : WebBanTra.Models.TaiKhoan
    {
        public AppUser() : base() { }

        [Required(ErrorMessage = "Họ tên không được để trống.")]
        public string HoTen { get; set; }
        [Required(ErrorMessage = "Email không được để trống.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        public string SoDienThoai { get; set; }
        [Required(ErrorMessage = "Địa chỉ không được để trống.")]
        public string DiaChi { get; set; }
        public string NgaySinh { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [Compare("MatKhau", ErrorMessage = "Mật khẩu không khớp.")]
        public string confirmMatKhau { get; set; }
    }
}