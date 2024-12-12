using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using WebBanTra.Models;
using WebBanTra.OOP;

namespace WebBanTra.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly DB_BanTraEntities _dbBanTra;
        DB_BanTraEntities db = new DB_BanTraEntities();
        // GET: KhachHang
        public ActionResult KhachHangProfile()
        {
            if (Session["TenDangNhap"] == null)
            {
                return RedirectToAction("DangNhap", "DN");
            }

            DB_BanTraEntities db = new DB_BanTraEntities();
            UserKH userKH = new UserKH();

            int maTK = Convert.ToInt32(Session["MaTK"]);
            int maKH = db.KhachHangs.Where(r => r.MaTK == maTK).FirstOrDefault().MaKH;

            if (Session["VaiTro"].ToString() == "Khách hàng")
            {
                KhachHang kh = db.KhachHangs.Where(r => r.MaKH == maKH).FirstOrDefault();
                List<ChiTietDH> ctdh = new List<ChiTietDH>();
                List<DonHang> dh = db.DonHangs.Where(r => r.MaKH == maKH).ToList();

                foreach (DonHang d in dh)
                {
                    // Lấy chi tiết đơn hàng theo từng trạng thái
                    List<ChiTietDH> ct = db.ChiTietDHs.Where(r => r.MaDH == d.MaDH && r.TrangThaiDanhGia != "Đã đánh giá").ToList();
                    ctdh.AddRange(ct);
                }

                foreach (ChiTietDH i in ctdh)
                {
                    Anh_SanPham a = db.Anh_SanPham.Where(r => r.SanPham.MaSP == i.SanPham.MaSP).FirstOrDefault();
                    userKH.listAnhSP.Add(a);
                }

                userKH.KhachHang = kh;
                userKH.ListChiTietDonHang = ctdh;
                userKH.listDonHang = dh;
                return View(userKH);
            }

            return RedirectToAction("DangNhap", "DN");
        }

        private void XoaSanPhamDaDanhGia(int MaCTDH)
        {
            var chiTietDH = db.ChiTietDHs.FirstOrDefault(ctdh => ctdh.MaCTDH == MaCTDH);
            if (chiTietDH != null && chiTietDH.TrangThaiDanhGia == "Chưa đánh giá")
            {
                chiTietDH.TrangThaiDanhGia = "Đã đánh giá";
                db.SaveChanges();
            }
        }

        public ActionResult DanhGia(int maCTDH)
        {
            var chiTietDH = db.ChiTietDHs.Find(maCTDH);
            if (chiTietDH != null && chiTietDH.DonHang.TrangThai == "Đã giao")
            {
                ViewBag.TenSP = chiTietDH.SanPham.TenSP;
                ViewBag.MaCTDH = maCTDH;
                return View();
            }

            TempData["ErrorMessage"] = "Sản phẩm này không thể đánh giá.";
            return RedirectToAction("KhachHangProfile", "KhachHang");
        }

        [HttpPost]
        public ActionResult DanhGia(int MaCTDH, int DiemDG, string BinhLuan)
        {
            if (ModelState.IsValid)
            {
                int MaKH = 0;

                if (Session["MaTK"] != null)
                {
                    int MaTK = (int)Session["MaTK"];
                    var khachHang = db.KhachHangs.FirstOrDefault(kh => kh.MaTK == MaTK);

                    if (khachHang != null)
                    {
                        MaKH = khachHang.MaKH;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Không tìm thấy khách hàng nào với mã tài khoản này.";
                        return RedirectToAction("Home", "Home");
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Bạn cần đăng nhập để thực hiện thao tác này.";
                    return RedirectToAction("DangNhap", "DN");
                }

                var danhGia = new DanhGia
                {
                    MaKH = MaKH,
                    MaCTDH = MaCTDH,
                    DiemDG = DiemDG,
                    BinhLuan = BinhLuan,
                    NgayDG = DateTime.Now
                };
                db.ChiTietDHs.Where(r=>r.MaCTDH == MaCTDH).FirstOrDefault().TrangThaiDanhGia = "Đã đánh giá";

                db.DanhGias.Add(danhGia);
                db.SaveChanges();

                XoaSanPhamDaDanhGia(MaCTDH);

                TempData["SuccessMessage"] = "Đánh giá của bạn đã được gửi thành công!";
                return RedirectToAction("KhachHangProfile", "KhachHang");
            }

            TempData["ErrorMessage"] = "Có lỗi xảy ra khi gửi đánh giá.";
            return View();
        }

        [HttpPost]
        public ActionResult ChangeProfile(KhachHang kh)
        {
            if (Session["TenDangNhap"] == null)
            {
                return RedirectToAction("DangNhap", "DN");
            }
            else
            {
                DB_BanTraEntities db = new DB_BanTraEntities();
                UserKH userKH = new UserKH();

                int maTK = Convert.ToInt32(Session["MaTK"]);
                int maKH = db.KhachHangs.Where(r => r.MaTK == maTK).FirstOrDefault().MaKH;

                string user = Session["TenDangNhap"].ToString();

                if (user == null)
                {
                    return RedirectToAction("DangNhap", "DN");
                }
                else if (user != null)
                {
                    if (Session["VaiTro"].ToString() == "Khách hàng")
                    {
                        KhachHang khachHang = db.KhachHangs.Where(r => r.MaKH == maKH).FirstOrDefault();
                        khachHang.TenKH = kh.TenKH;
                        khachHang.GioiTinh = kh.GioiTinh;
                        khachHang.DiaChi = kh.DiaChi;
                        khachHang.NgaySinh = kh.NgaySinh;
                        db.SaveChanges();
                        List<ChiTietDH> ctdh = new List<ChiTietDH>();
                        List<DonHang> dh = db.DonHangs.Where(r => r.MaKH == maKH && r.TrangThai == "Chưa giao" || r.TrangThai == "Chờ xác nhận").ToList();
                        foreach (DonHang d in dh)
                        {
                            List<ChiTietDH> ct = db.ChiTietDHs.Where(r => r.MaDH == d.MaDH).ToList();
                            foreach (ChiTietDH i in ct)
                            {
                                ctdh.Add(i);
                            }
                        }
                        foreach (ChiTietDH i in ctdh)
                        {
                            Anh_SanPham a = db.Anh_SanPham.Where(r => r.SanPham.MaSP == i.SanPham.MaSP).FirstOrDefault();
                            userKH.listAnhSP.Add(a);
                        }
                        ctdh.OrderBy(r => r.SanPham.MaDM).GroupBy(r => r.SanPham.MaDM);
                        userKH.KhachHang = kh;
                        userKH.ListChiTietDonHang = ctdh;
                        userKH.listDonHang = dh;
                        return View("KhachHangProfile", userKH);
                    }
                }
                return View("KhachHangProfile",userKH);
            }
        }

    }
}