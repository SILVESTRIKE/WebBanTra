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
        // GET: KhachHang
        public ActionResult KhachHangProfile()
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
                        KhachHang kh = db.KhachHangs.Where(r => r.MaKH == maKH).FirstOrDefault();
                        List<ChiTietDH> ctdh = new List<ChiTietDH>();
                        List<DonHang> dh = db.DonHangs.Where(r => r.MaKH == maKH && r.TrangThai != "Đã giao").ToList();
                        if(dh.Count > 0)
                        {
                            foreach (DonHang d in dh)
                            {
                                List<ChiTietDH> ct = db.ChiTietDHs.Where(r => r.MaDH == d.MaDH).ToList();
                                foreach (ChiTietDH i in ct)
                                {
                                    ctdh.Add(i);
                                }
                            }
                        }
                        else
                        {
                            ctdh = db.ChiTietDHs.Where(r => r.TrangThaiDanhGia == "Chưa đánh giá").ToList();
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
                        return View(userKH);
                    }
                }
                return View(userKH);
            }

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