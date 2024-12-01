﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebBanTra.Models;
using System.Data.Entity;

namespace WebBanTra.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {

        private readonly DB_BanTraEntities _dbContext;

        public AdminController()
        {
            this._dbContext = new DB_BanTraEntities();
        }

        public ActionResult Admin()
        {
            var list = _dbContext.SanPhams.ToList();
            ViewBag.listAnhSP = _dbContext.AnhSanPhams.ToList();
            return View(list);
        }

        public ActionResult CreateSanPham()
        {
            ViewBag.DanhMucs = _dbContext.DanhMucs.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult CreateSanPham(SanPham model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.SanPhams.Add(model);
                    _dbContext.SaveChanges();
                    return RedirectToAction("Admin");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi thêm sản phẩm: " + ex.Message);
                }
            }

            ViewBag.DanhMucs = _dbContext.DanhMucs.ToList();
            return View(model);
        }

        public ActionResult UpdateSanPham(int id)
        {
            SanPham model = _dbContext.SanPhams.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            ViewBag.DanhMucs = _dbContext.DanhMucs.ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateSanPham(SanPham model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    SanPham existingProduct = _dbContext.SanPhams.Find(model.MaSP);
                    if (existingProduct == null)
                    {
                        ModelState.AddModelError("", "Sản phẩm không tồn tại.");
                        return View(model);
                    }

                    existingProduct.TenSP = model.TenSP;
                    existingProduct.Gia = model.Gia;
                    existingProduct.SoLuongTon = model.SoLuongTon;
                    existingProduct.MaDM = model.MaDM;

                    _dbContext.SaveChanges();
                    ViewBag.DanhMucs = _dbContext.DanhMucs.ToList();
                    return RedirectToAction("Admin");
                }
                catch (Exception ex)
                {
                    ViewBag.DanhMucs = _dbContext.DanhMucs.ToList();
                    ModelState.AddModelError("", "Đã xảy ra lỗi: " + ex.Message);
                }
            }

            ViewBag.DanhMucs = _dbContext.DanhMucs.ToList();
            return View(model);
        }

        public ActionResult DeleteSanPham(int id)
        {
            try
            {
                var model = _dbContext.SanPhams.Find(id);
                if (model != null)
                {
                    List<ChiTietDH> lstCTDH = _dbContext.ChiTietDHs.Where(r => r.MaSP == id).ToList();
                    List<ChiTietDNH> lstCTDNH = _dbContext.ChiTietDNHs.Where(r => r.MaSP == id).ToList();

                    List<Anh_SanPham> lstASp = _dbContext.AnhSanPhams.Where(r => r.MaSP == id).ToList();
                    List<MoTa_SanPham> lstMoTa = _dbContext.MoTaSanPhams.Where(r => r.MaSP == id).ToList();
                    foreach (Anh_SanPham item in lstASp)
                    {
                        if (item != null)
                        {
                            _dbContext.AnhSanPhams.Remove(item);
                        }
                    }
                    foreach (MoTa_SanPham item in lstMoTa)
                    {
                        if (item != null)
                        {
                            _dbContext.MoTaSanPhams.Remove(item);
                        }
                    }
                    foreach (ChiTietDNH ctDN in lstCTDNH)
                    {
                        if (ctDN != null)
                        {
                            _dbContext.ChiTietDNHs.Remove(ctDN);
                        }
                    }
                    foreach (ChiTietDH ctdh in lstCTDH)
                    {
                        DanhGia dg = _dbContext.DanhGias.Where(r => r.MaCTDH == ctdh.MaCTDH).FirstOrDefault();

                        if (dg != null)
                        {
                            _dbContext.DanhGias.Remove(dg);
                        }
                        _dbContext.ChiTietDHs.Remove(ctdh);

                    }
                    _dbContext.SanPhams.Remove(model);
                    _dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi xóa sản phẩm: " + ex.Message;
            }

            return RedirectToAction("Admin");
        }

        private SelectList GetDanhMucList()
        {


            return new SelectList(_dbContext.DanhMucs, "MaDM", "TenDM");
        }

        public ActionResult UserProfile()
        {
            int maTK = Convert.ToInt32(Session["MaTK"]);
            NhanVien nv = _dbContext.NhanViens.FirstOrDefault(n => n.MaTK == maTK);
            return View(nv);
        }

        [HttpPost]
        public ActionResult XacNhanDonHang(int maDonHang)
        {
            if (Session["MaTK"] == null)
            {
                return RedirectToAction("DangNhap", "DN"); // Chuyển hướng nếu chưa đăng nhập
            }
            using (_dbContext)
            {
                var donHang = _dbContext.DonHangs.FirstOrDefault(dh => dh.MaDH == maDonHang);
                if (donHang == null)
                {
                    return HttpNotFound();
                }

                int maTK = (int)Session["MaTK"];
                var nhanVien = _dbContext.NhanViens.FirstOrDefault(nv => nv.MaTK == maTK);
                if (nhanVien == null)
                {
                    return RedirectToAction("DangNhap", "DN"); // Kiểm tra nhân viên có tồn tại
                }
                donHang.TrangThai = "Chưa giao";
                donHang.MaNV = nhanVien.MaNV; // Lưu mã nhân viên xử lý đơn hàng
                _dbContext.SaveChanges();

                return RedirectToAction("QuanLyDonHang");
            }

        }
        public ActionResult QuanLyDonHang()
        {
            using (_dbContext)
            {
                var danhSachDonHang = _dbContext.DonHangs
                                                         .Include(dh => dh.NhanVien)
                                                         .Include(dh => dh.KhachHang)
                                                         .ToList();
                return View(danhSachDonHang);
            }
        }

        public ActionResult QuanLyHoaDon()
        {
            using (_dbContext)
            {
                List<HoaDon> danhSachHoaDon = _dbContext.HoaDons.ToList();
                return View(danhSachHoaDon);
            }
        }

    }

}