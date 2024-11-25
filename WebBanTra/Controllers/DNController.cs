using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanTra.Models;

namespace WebBanTra.Controllers
{

    
    public class DNController : Controller
    {
         private DB_BanTraEntities2 db = new DB_BanTraEntities2(); // Entity Data Model

        // GET: DN/DangNhap
        public ActionResult DangNhap()
        {
            return View();
        }

        // POST: DN/DangNhap
        [HttpPost]
        public ActionResult DangNhap(string taiKhoan, string matKhau)
        {
            var taiKhoanDangNhap = db.TaiKhoans
                .Where(tk => tk.TenDangNhap == taiKhoan && tk.MatKhau == matKhau)
                .FirstOrDefault();

            if (taiKhoanDangNhap != null)
            {
                // Cập nhật trạng thái đăng nhập
                taiKhoanDangNhap.TrangThai = "Đang Đăng Nhập";
                db.SaveChanges();

                // Lưu thông tin đăng nhập vào Session
                Session["MaNguoiDung"] = taiKhoanDangNhap.MaTK;
                Session["TenNguoiDung"] = taiKhoanDangNhap.TenDangNhap;
                Session["VaiTro"] = taiKhoanDangNhap.VaiTro;

                // Chuyển hướng dựa trên vai trò
                if (taiKhoanDangNhap.VaiTro == "Admin")
                {
                    return RedirectToAction("Admin", "Admin");
                }
                else if (taiKhoanDangNhap.VaiTro == "Nhân viên")
                {
                    return RedirectToAction("Home", "Home");
                }
                else
                {
                    return RedirectToAction("Home", "Home");
                }
            }

            // Nếu không tìm thấy tài khoản
            ViewBag.ErrorMessage = "Tên đăng nhập hoặc mật khẩu không đúng!";
            return View();
        }

        // GET: DN/DangKy
        public ActionResult DangKy()
        {
            return View();
        }
        // POST: DN/DangKy
        [HttpPost]
        public ActionResult DangKy(string taiKhoan, string matKhau, string xacNhanMatKhau, string vaiTro)
        {
            if (matKhau != xacNhanMatKhau)
            {
                ViewBag.ErrorMessage = "Mật khẩu và xác nhận mật khẩu không khớp!";
                return View();
            }

            // Kiểm tra tài khoản đã tồn tại
            var taiKhoanTonTai = db.TaiKhoans.Any(tk => tk.TenDangNhap == taiKhoan);
            if (taiKhoanTonTai)
            {
                ViewBag.ErrorMessage = "Tên đăng nhập đã tồn tại!";
                return View();
            }

            // Tạo tài khoản mới
            var taiKhoanMoi = new TaiKhoan
            {
                TenDangNhap = taiKhoan,
                MatKhau = matKhau,
                VaiTro = string.IsNullOrEmpty(vaiTro) ? "Khách hàng" : vaiTro,  // Default to Khách hàng if empty
                TrangThai = "Không Đăng Nhập"
            };

            db.TaiKhoans.Add(taiKhoanMoi);
            db.SaveChanges();

            ViewBag.SuccessMessage = "Đăng ký thành công! Vui lòng đăng nhập.";
            return RedirectToAction("DangNhap");
        }
        // Đăng xuất
        public ActionResult DangXuat()
        {
            // Cập nhật trạng thái đăng xuất
            int maNguoiDung = (int)Session["MaNguoiDung"];
            var taiKhoanDangXuat = db.TaiKhoans.Find(maNguoiDung);
            if (taiKhoanDangXuat != null)
            {
                taiKhoanDangXuat.TrangThai = "Không Đăng Nhập";
                db.SaveChanges();
            }

            // Xóa thông tin đăng nhập trong Session
            Session.Clear();
            return RedirectToAction("DangNhap", "DN");
        }
    }
    }
