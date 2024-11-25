using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebBanTra.Models;
using WebBanTra.OOP;
using System.Web.Helpers;

namespace WebBanTra.Controllers
{

    
    public class DNController : Controller
    {
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangNhap(TaiKhoan model)
        {
            if (ModelState.IsValid)
            {
                DB_BanTraEntities db = new DB_BanTraEntities();
                TaiKhoan user = db.TaiKhoans.FirstOrDefault(r=>r.TenDangNhap == model.TenDangNhap && r.MatKhau == model.MatKhau);

                if (user != null)
                {
                    // Lưu thông tin đăng nhập vào Session
                    Session["TenDangNhap"] = user.TenDangNhap;
                    Session["VaiTro"] = user.VaiTro;

                    // Điều hướng theo vai trò
                    switch (user.VaiTro)
                    {
                        case "Admin":
                            return RedirectToAction("Home", "Home");
                        case "Nhân viên":
                            return RedirectToAction("Home", "Home");
                        case "Khách hàng":
                            return RedirectToAction("Home", "Home");
                        default:
                            ModelState.AddModelError("", "Không thể xác định vai trò.");
                            break;
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không chính xác.");
                }
            }

            return View(model);
        }

        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(AppUser user)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    DB_BanTraEntities db = new DB_BanTraEntities();
                    if(user.SoDienThoai.Length > 10 || user.SoDienThoai.Length < 10)
                        throw new Exception("SoDienThoai");
                    if (db.KhachHangs.Where(r => r.SDT == user.SoDienThoai).Count() > 0 || db.NhanViens.Where(r => r.SDT == user.SoDienThoai).Count() > 0)
                        throw new Exception("Số điện thoại đã tồn tại");
                    TaiKhoan taiKhoan = new TaiKhoan();
                    KhachHang khachHang = new KhachHang();
                    if (db.TaiKhoans.Where(r => r.TenDangNhap == user.TenDangNhap).Count() > 0)
                        throw new Exception("Tên đăng nhập đã tồn tại");
                    taiKhoan.TenDangNhap = user.TenDangNhap;
                    taiKhoan.MatKhau = user.MatKhau;
                    taiKhoan.VaiTro = "Khách hàng";
                    db.TaiKhoans.Add(taiKhoan);
                    db.SaveChanges();

                    khachHang.TenKH = user.HoTen;
                    khachHang.Email = user.Email;
                    khachHang.SDT = user.SoDienThoai;
                    khachHang.DiaChi = user.DiaChi;
                    khachHang.MaTK = db.TaiKhoans.Where(r => r.TenDangNhap == user.TenDangNhap).FirstOrDefault().MaTK;
                    db.KhachHangs.Add(khachHang);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(ex.Message, "Lỗi dữ liệu");
                    return View();
                }
                return RedirectToAction("DN", "DangNhap");
            }
            else
            {
                ModelState.AddModelError("Lỗi", "Đăng ký không thành công.");
                return View();
            }
            
        }

        public ActionResult DangXuat()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}