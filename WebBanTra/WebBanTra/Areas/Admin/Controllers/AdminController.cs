﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebBanTra.Models;
using System.Data.Entity;
using WebBanTra.OOP;
using System.IO;
using System.Web.Configuration;
using System.Collections;
using static System.Net.WebRequestMethods;

namespace WebBanTra.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {

        private readonly DB_BanTraEntities _dbContext = new DB_BanTraEntities();
        public static List<SanPham> spBanChay = new List<SanPham>();
        public AdminController()
        {
            this._dbContext = new DB_BanTraEntities();
        }

        public ActionResult Admin(string filter = "Hôm nay")
        {
            spBanChay.Clear();
            var list = _dbContext.SanPhams.ToList();
            ViewBag.listAnhSP = _dbContext.Anh_SanPham.ToList();
            DateTime startDate = DateTime.Now, endDate = DateTime.Now;
            DateTime previousStartDate = DateTime.Now, previousEndDate = DateTime.Now;

            // Xử lý các filter theo ngày, tháng, năm
            switch (filter)
            {
                case "Hôm nay":
                    startDate = DateTime.Today;
                    previousStartDate = DateTime.Today.AddDays(-1);
                    previousEndDate = DateTime.Today.AddDays(-1).AddDays(1);
                    break;

                case "Tháng":
                    startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    previousStartDate = startDate.AddMonths(-1);
                    previousEndDate = startDate.AddDays(-1);
                    break;

                case "Năm":
                    startDate = new DateTime(DateTime.Now.Year, 1, 1);
                    previousStartDate = startDate.AddYears(-1);
                    previousEndDate = startDate.AddDays(-1);
                    break;
            }

            // Truyền filter sang View
            ViewBag.Filter = filter;

            // Lấy tổng số đơn hàng và tổng số khách hàng hiện tại
            var totalOrders = _dbContext.DonHangs
                                        .Count(dh => dh.NgayDat >= startDate && dh.NgayDat <= endDate);
            var totalRevenue = _dbContext.HoaDons
                                        .Where(hd => hd.DonHang.NgayDat >= startDate &&
                                                     hd.DonHang.NgayDat <= endDate &&
                                                     hd.DonHang.TongTien.HasValue)
                                        .Sum(hd => hd.DonHang.TongTien) ?? 0;
            var totalCustomers = _dbContext.KhachHangs.Count();

            // Lấy dữ liệu kỳ trước để tính phần trăm thay đổi
            var previousTotalOrders = _dbContext.DonHangs
                                                .Count(dh => dh.NgayDat >= previousStartDate && dh.NgayDat <= previousEndDate);
            var previousTotalRevenue = _dbContext.HoaDons
                                                .Where(hd => hd.DonHang.NgayDat >= previousStartDate &&
                                                             hd.DonHang.NgayDat <= previousEndDate &&
                                                             hd.DonHang.TongTien.HasValue)
                                                .Sum(hd => hd.DonHang.TongTien) ?? 0;

            ViewBag.OrderChange = previousTotalOrders == 0 ? 0 : CalculatePercentageChange(previousTotalOrders, totalOrders);
            ViewBag.RevenueChange = previousTotalRevenue == 0 ? 0 : CalculatePercentageChange(previousTotalRevenue, totalRevenue);
            ViewBag.TotalOrders = totalOrders;
            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.TotalCustomers = totalCustomers;



            var spbc = _dbContext.ChiTietDHs.GroupBy(r => r.MaSP).Select(g => new
            {
                MaSP = g.Key,
                SoLuong = g.Sum(r => r.SoLuongMua),
            }).OrderByDescending(g => g.SoLuong).Take(9).ToList();

            List<SanPham> lstsp = new List<SanPham>();

            foreach (var item in spbc)
            {
                lstsp.Add(_dbContext.SanPhams.Find(item.MaSP));
            }

            bool khac = lstsp.Except(spBanChay).Any() || spBanChay.Except(lstsp).Any();
            if(khac)
            {
                spBanChay = lstsp; 
            }
            return View(list);
        }

        private double CalculatePercentageChange(double previousValue, double currentValue)
        {
            if (previousValue == 0)
                return currentValue > 0 ? 100 : 0; // Tránh chia cho 0
            return ((currentValue - previousValue) / previousValue) * 100;
        }

        public ActionResult CreateSanPham()
        {
            ViewBag.DanhMucs = _dbContext.DanhMucs.ToList();
            return View();
        }

        //[HttpPost]
        //public ActionResult CreateSanPham(DetailProduct model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _dbContext.SanPhams.Add(model);
        //            _dbContext.SaveChanges();
        //            return RedirectToAction("Admin");
        //        }
        //        catch (Exception ex)
        //        {
        //            ModelState.AddModelError("", "Lỗi khi thêm sản phẩm: " + ex.Message);
        //        }
        //    }

        //    ViewBag.DanhMucs = _dbContext.DanhMucs.ToList();
        //    return View(model);
        //}

        [HttpPost]
        public ActionResult CreateSanPham(DetailProduct model, HttpPostedFileBase hinhanh)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        // 1. Tạo sản phẩm mới
                        var sanPham = new SanPham
                        {
                            TenSP = model.TenSP,
                            Gia = model.Gia,
                            SoLuongTon = model.SoLuongTon,
                            MaDM = model.MaDM
                        };
                        _dbContext.SanPhams.Add(sanPham);
                        _dbContext.SaveChanges(); // Lưu để lấy MaSP

                        int maSP = _dbContext.SanPhams.OrderByDescending(r => r.MaSP).FirstOrDefault().MaSP;

                        // 2. Xử lý hình ảnh nếu có
                        if (hinhanh != null && hinhanh.ContentLength > 0)
                        {
                            var fileName = Path.GetFileNameWithoutExtension(hinhanh.FileName);
                            var extension = Path.GetExtension(hinhanh.FileName);
                            var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";
                            var uploadPath = Server.MapPath("~/images/");

                            if (!Directory.Exists(uploadPath))
                            {
                                Directory.CreateDirectory(uploadPath);
                            }

                            var path = Path.Combine(uploadPath, uniqueFileName);
                            hinhanh.SaveAs(path);

                            model.hinhanh = $"/images/{uniqueFileName}";
                        }

                        // 3. Lưu mô tả sản phẩm
                        var moTaSanPham = new MoTa_SanPham
                        {
                            MaSP = maSP,
                            MoTa = model.mota
                        };
                        _dbContext.MoTa_SanPham.Add(moTaSanPham);
                        if (model.hinhanh != null)
                        {
                            var anhSanPham = new Anh_SanPham
                            {
                                LinhAnh = model.hinhanh,
                                MaSP = maSP
                            };
                            _dbContext.Anh_SanPham.Add(anhSanPham);
                        }
                        else
                        {
                            var anhSanPham = new Anh_SanPham
                            {
                                LinhAnh = "/Images/no-image.jpg",
                                MaSP = maSP
                            };
                            _dbContext.Anh_SanPham.Add(anhSanPham);
                        }
                        _dbContext.SaveChanges();

                        transaction.Commit();
                        return RedirectToAction("Admin");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        ModelState.AddModelError("", "Lỗi khi thêm sản phẩm: " + ex.Message);
                    }
                }
            }

            // Nếu thất bại, hiển thị lại form
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

                    List<Anh_SanPham> lstASp = _dbContext.Anh_SanPham.Where(r => r.MaSP == id).ToList();
                    List<MoTa_SanPham> lstMoTa = _dbContext.MoTa_SanPham.Where(r => r.MaSP == id).ToList();
                    foreach (Anh_SanPham item in lstASp)
                    {
                        if (item != null)
                        {
                            _dbContext.Anh_SanPham.Remove(item);
                        }
                    }
                    foreach (MoTa_SanPham item in lstMoTa)
                    {
                        if (item != null)
                        {
                            _dbContext.MoTa_SanPham.Remove(item);
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
                if (donHang.TrangThai == "Chờ xác nhận")
                {
                    donHang.TrangThai = "Chưa giao";
                    donHang.MaNV = nhanVien.MaNV;
                    _dbContext.SaveChanges();
                }
                else
                {
                    donHang.TrangThai = "Đã giao";
                    _dbContext.SaveChanges();
                }

                return RedirectToAction("QuanLyDonHang");
            }

        }

        int GetPriority(string status)
        {
            switch(status)
            {
                case "Chờ xác nhận": return 0;
                case "Chưa giao":  return 1;
                case "Đã giao": return 2;
                default: return 3;
            };
        }

        public ActionResult QuanLyDonHang()
        {
            using (_dbContext)
            {
                var danhSachDonHang = _dbContext.DonHangs
                                                         .Include(dh => dh.NhanVien)
                                                         .Include(dh => dh.KhachHang)
                                                         .ToList().OrderBy(r => GetPriority(r.TrangThai));
                return View(danhSachDonHang);
            }
        }

        public ActionResult QuanLyHoaDon()
        {
            var hoaDons = _dbContext.HoaDons
                               .Include(hd => hd.DonHang) // Explicit loading
                               .ToList();
            ViewBag.TotalIncome = hoaDons.Sum(hd => hd.DonHang.TongTien ?? 0);
            return View(hoaDons);
        }

        public ActionResult QuanLyNhanVien()
        {
            using (_dbContext)
            {
                List<NhanVien> listNhanVien = _dbContext.NhanViens.ToList();
                return View(listNhanVien);
            }
        }

        public ActionResult CreateNhanVien()
        {
            ViewBag.Nhanviens = _dbContext.NhanViens.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult CreateNhanVien(NhanVien model)
        {
            try
            {
                ViewBag.NhanViens = _dbContext.NhanViens.ToList();
                if (model.TenNV == null || model.SDT == null || model.Email == null || model.ChucVu == null)
                {
                    ModelState.AddModelError("TenNV", "Hãy nhập đầy đủ trường thông tin");
                    return View(model);
                }

                if (_dbContext.TaiKhoans.Where(r => r.TenDangNhap == model.TaiKhoan.TenDangNhap).FirstOrDefault() != null)
                {
                    ModelState.AddModelError("TenDangNhap", "Tài khoản đã tồn tại.");
                    return View(model);
                }
                model.TaiKhoan.VaiTro = model.ChucVu == "Admin" ? "Admin" : "Nhân viên";
                model.TaiKhoan.TrangThai = "Không đăng nhập";

                if (_dbContext.NhanViens.Where(r=> r.SDT == model.SDT).FirstOrDefault() != null)
                {
                    ModelState.AddModelError("", "Số điện thoại của nhân viên đã tồn tại.");
                    return View(model);
                }
                if (_dbContext.NhanViens.Where(r => r.SDT == model.Email).FirstOrDefault() != null)
                {
                    ModelState.AddModelError("", "Email của nhân viên đã tồn tại.");
                    return View(model);
                }

                _dbContext.NhanViens.Add(model);
                _dbContext.SaveChanges();
                return RedirectToAction("QuanLyNhanVien");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi khi thêm Nhân viên: " + ex.Message);
            }
            
            return View(model);
        }

        public ActionResult UpdateNhanVien(int id)
        {
            NhanVien model = _dbContext.NhanViens.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            ViewBag.lstNhanVien = _dbContext.NhanViens.Where(r => r.MaNV != id).ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateNhanVien(NhanVien model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    NhanVien existingStaff = _dbContext.NhanViens.Where(r => r.MaNV == model.MaNV).FirstOrDefault();
                    if (existingStaff == null)
                    {
                        ModelState.AddModelError("", "Sản phẩm không tồn tại.");
                        return View(model);
                    }

                    existingStaff.TenNV = model.TenNV;
                    existingStaff.ChucVu = model.ChucVu;
                    existingStaff.DiaChi = model.DiaChi;
                    existingStaff.Email = model.Email;
                    existingStaff.SDT = model.SDT;
                    existingStaff.MaQuanLy = model.MaQuanLy;

                    _dbContext.SaveChanges();

                    return RedirectToAction("QuanLyNhanVien");
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

        public ActionResult DeleteStaff(int id)
        {
            try
            {
                NhanVien model = _dbContext.NhanViens.Find(id);
                if (model != null)
                {
                    List<DonHang> lstDH = _dbContext.DonHangs.Where(r => r.MaNV == id).ToList();
                    List<TaiKhoan> lstCTDNH = _dbContext.TaiKhoans.Where(r => r.MaTK == model.MaTK).ToList();
                    List<NhanVien> lstNhanVien = _dbContext.NhanViens.Where(r => r.MaQuanLy == model.MaNV).ToList();
                    foreach (DonHang item in lstDH)
                    {
                        if (item != null)
                        {
                            _dbContext.DonHangs.Where(r => r.MaNV == model.MaNV).FirstOrDefault().MaNV = 1;
                        }
                    }
                    foreach (TaiKhoan item in lstCTDNH)
                    {
                        if (item != null)
                        {
                            _dbContext.TaiKhoans.Remove(item);
                        }
                    }
                    foreach (NhanVien item in lstNhanVien)
                    {
                        if (item != null)
                        {
                            _dbContext.NhanViens.Where(r => r.MaQuanLy == model.MaNV).FirstOrDefault().MaQuanLy = null;
                        }
                    }

                    _dbContext.NhanViens.Remove(model);
                    _dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi xóa sản phẩm: " + ex.Message;
            }

            return RedirectToAction("QuanLyNhanVien");
        }

        public ActionResult CreateDonNhapHang()
        {
            ViewBag.NhaCungCaps = _dbContext.NhaCungCaps.ToList();
            ViewBag.SanPhams = _dbContext.SanPhams.ToList();

            int maTK = (int)Session["MaTK"];
            var nhanVien = _dbContext.NhanViens.FirstOrDefault(nv => nv.MaTK == maTK);

            if (nhanVien == null)
            {
                // Xử lý trường hợp không tìm thấy nhân viên
                ModelState.AddModelError("", "Không tìm thấy nhân viên.");
                return View();
            }

            ViewBag.MaNV = nhanVien.MaNV; // Lấy giá trị Mã Nhân Viên từ session
            ViewBag.TenNV = nhanVien.TenNV;
            return View();
        }


        [HttpPost]
        public ActionResult CreateDonNhapHang(DonNhapHang model, List<ChiTietDNH> chiTietDonNhap)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        // Lấy Mã Nhân Viên từ session và gán vào model
                        int maTK = (int)Session["MaTK"];
                        var nhanVien = _dbContext.NhanViens.FirstOrDefault(nv => nv.MaTK == maTK);

                        if (nhanVien == null)
                        {
                            // Xử lý trường hợp không tìm thấy nhân viên
                            ModelState.AddModelError("", "Không tìm thấy nhân viên.");
                            return View(model);
                        }

                        model.MaNV = nhanVien.MaNV;
                        model.NhanVien = nhanVien; // Gán đối tượng nhanVien vào model.NhanVien

                        // Tạo đơn nhập hàng mới
                        model.NgayDat = DateTime.Now;
                        _dbContext.DonNhapHangs.Add(model);
                        _dbContext.SaveChanges(); // Lưu để lấy MaDNH

                        // Lấy mã đơn nhập hàng vừa tạo
                        int maDNH = model.MaDNH;

                        // Kiểm tra và khởi tạo chiTietDonNhap nếu nó là null
                        if (chiTietDonNhap == null)
                        {
                            chiTietDonNhap = new List<ChiTietDNH>();
                        }

                        // Thêm chi tiết đơn nhập hàng
                        foreach (var item in chiTietDonNhap)
                        {
                            item.MaDNH = maDNH;
                            _dbContext.ChiTietDNHs.Add(item);
                        }

                        _dbContext.SaveChanges();
                        transaction.Commit();
                        return RedirectToAction("Admin");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        // Ghi log lỗi hoặc hiển thị chi tiết inner exception
                        ModelState.AddModelError("", "Lỗi khi tạo đơn nhập hàng: " + ex.InnerException?.Message ?? ex.Message);
                    }
                }
            }

            // Nếu thất bại, hiển thị lại form
            ViewBag.NhaCungCaps = _dbContext.NhaCungCaps.ToList();
            ViewBag.SanPhams = _dbContext.SanPhams.ToList();
            return View(model);
        }


        private List<CartForNCC> GetCart()
        {
            // Lấy giỏ hàng từ session
            List<CartForNCC> cart = Session["Cart"] as List<CartForNCC>;

            // Kiểm tra nếu giỏ hàng rỗng hoặc không tồn tại
            if (cart == null)
            {
                cart = new List<CartForNCC>();
            }

            // Lấy các sản phẩm có số lượng lớn hơn 0
            return cart.Where(c => c.SoLuong > 0).ToList();
        }

        [HttpPost]
        public ActionResult CheckOutDNH(DonNhapHang model, FormCollection form)
        {
            DB_BanTraEntities db = new DB_BanTraEntities();
            List<ChiTietDNH> chiTietDonNhap = new List<ChiTietDNH>();

            var sanPhams = db.SanPhams.ToList();
            foreach (var sanPham in sanPhams)
            {
                int soLuong;
                if (int.TryParse(form["chiTietDonNhap[" + sanPham.MaSP + "].SoLuongNhap"], out soLuong) && soLuong > 0)
                {
                    chiTietDonNhap.Add(new ChiTietDNH
                    {
                        MaSP = sanPham.MaSP,
                        SoLuongNhap = soLuong,
                        GiaNhap = Math.Round(sanPham.Gia * 0.7, 2)

                    });
                }
            }

            if (!chiTietDonNhap.Any())
            {
                ModelState.AddModelError("", "Giỏ hàng của bạn trống. Vui lòng thêm sản phẩm vào giỏ hàng trước khi thanh toán.");
                ViewBag.NhaCungCaps = db.NhaCungCaps.ToList();
                ViewBag.SanPhams = db.SanPhams.ToList();
                return View("CreateDonNhapHang", model);
            }

            try
            {
                int maTK = Convert.ToInt32(Session["MaTK"]);
                int maNV = db.NhanViens.FirstOrDefault(p => p.MaTK == maTK).MaNV;

                model.MaNV = maNV;
                model.NgayDat = DateTime.Now;
                model.TongTien = chiTietDonNhap.Sum(r => r.SoLuongNhap * r.GiaNhap);
                model.TrangThai = false;

                db.DonNhapHangs.Add(model);
                db.SaveChanges();

                int maDNH = model.MaDNH;

                foreach (var item in chiTietDonNhap)
                {
                    item.MaDNH = maDNH;
                    db.ChiTietDNHs.Add(item);
                }
                db.SaveChanges();

                Session["Cart"] = null;
                return RedirectToAction("QuanLyDonNhapHang");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi khi tạo đơn nhập hàng: " + ex.Message);
                ViewBag.NhaCungCaps = db.NhaCungCaps.ToList();
                ViewBag.SanPhams = db.SanPhams.ToList();
                return View("CreateDonNhapHang", model);
            }
        }



        public ActionResult QuanLyDonNhapHang()
        {
            using (_dbContext)
            {
                var danhSachNhapDonHang = _dbContext.DonNhapHangs
                                                         .Include(dnh => dnh.NhanVien)
                                                         .Include(dnh => dnh.NhaCungCap)
                                                         .ToList();
                return View(danhSachNhapDonHang);
            }
        }
        [HttpPost]
        public ActionResult XacNhanDonNhapHang(int madonNhapHang)
        {
            if (Session["MaTK"] == null)
            {
                return RedirectToAction("DangNhap", "DN"); // Chuyển hướng nếu chưa đăng nhập
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var donNhapHang = _dbContext.DonNhapHangs
                                                .Include(dnh => dnh.ChiTietDNHs)
                                                .FirstOrDefault(dnh => dnh.MaDNH == madonNhapHang);
                    if (donNhapHang == null)
                    {
                        return HttpNotFound();
                    }

                    int maTK = (int)Session["MaTK"];
                    var nhanVien = _dbContext.NhanViens.FirstOrDefault(nv => nv.MaTK == maTK);
                    if (nhanVien == null)
                    {
                        return RedirectToAction("DangNhap", "DN"); // Kiểm tra nhân viên có tồn tại
                    }

                    if (donNhapHang.TrangThai == false)
                    {
                        foreach (var chiTiet in donNhapHang.ChiTietDNHs)
                        {
                            var sanPham = _dbContext.SanPhams.FirstOrDefault(s => s.MaSP == chiTiet.MaSP);
                            if (sanPham != null)
                            {
                                sanPham.SoLuongTon += chiTiet.SoLuongNhap ?? 0; // Chuyển đổi tường minh từ int? sang int
                            }
                        }

                        donNhapHang.TrangThai = true; // Cập nhật trạng thái đơn nhập hàng thành đã xử lý
                        donNhapHang.MaNV = nhanVien.MaNV;
                        _dbContext.SaveChanges();
                        transaction.Commit();
                    }

                    return RedirectToAction("QuanLyDonNhapHang");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ModelState.AddModelError("", "Lỗi khi xác nhận đơn nhập hàng: " + ex.Message);
                    return RedirectToAction("QuanLyDonNhapHang");
                }
            }
        }
        public ActionResult GetTotalIncome()
        {
            using (_dbContext)
            {
                var totalIncome = _dbContext.HoaDons.Sum(hd => hd.DonHang.TongTien ?? 0); 
                return PartialView("_TotalIncome", totalIncome);
            }
        }
        public ActionResult QuanLyKhachHang()
        {
            using (_dbContext)
            {
                List<KhachHang> listKhachHang = _dbContext.KhachHangs.ToList();
                return View(listKhachHang);
            }
        }
        public ActionResult CreateKhachHang()
        {
            // Nếu cần truyền danh sách khách hàng vào ViewBag
            ViewBag.KhachHang = _dbContext.KhachHangs.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult CreateKhachHang(KhachHang model)
        {
            try
            {
                // Kiểm tra các trường bắt buộc
                if (string.IsNullOrWhiteSpace(model.TenKH) || string.IsNullOrWhiteSpace(model.SDT) || string.IsNullOrWhiteSpace(model.Email))
                {
                    ModelState.AddModelError("TenKH", "Hãy nhập đầy đủ trường thông tin");
                    return View(model);
                }

                // Kiểm tra số điện thoại, email và tài khoản đã tồn tại
                if (_dbContext.KhachHangs.Any(kh => kh.SDT == model.SDT))
                {
                    ModelState.AddModelError("", "Số điện thoại của khách hàng đã tồn tại.");
                    return View(model);
                }
                if (_dbContext.KhachHangs.Any(kh => kh.Email == model.Email))
                {
                    ModelState.AddModelError("", "Email của khách hàng đã tồn tại.");
                    return View(model);
                }
                if (_dbContext.KhachHangs.Any(kh => kh.TaiKhoan == model.TaiKhoan))
                {
                    ModelState.AddModelError("", "Tài khoản của khách hàng đã tồn tại.");
                    return View(model);
                }

                // Thêm khách hàng mới vào cơ sở dữ liệu
                _dbContext.KhachHangs.Add(model);
                _dbContext.SaveChanges();

                // Chuyển hướng đến trang quản lý khách hàng sau khi lưu thành công
                return RedirectToAction("QuanLyKhachHang");
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và thêm thông báo lỗi vào ModelState
                ModelState.AddModelError("", "Lỗi khi thêm Khách hàng: " + ex.Message);
            }

            // Trả về view cùng với ViewBag để hiển thị danh sách khách hàng
            ViewBag.KhachHang = _dbContext.KhachHangs.ToList();
            return View(model);
        }


        public ActionResult UpdateKhachHang(int id)
        {
            KhachHang model = _dbContext.KhachHangs.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            ViewBag.lstKhachHang = _dbContext.KhachHangs.Where(r => r.MaKH != id).ToList();
            return View(model);
        }


        [HttpPost]
        public ActionResult UpdateKhachHang(KhachHang model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    KhachHang existingCustomer = _dbContext.KhachHangs.Where(r => r.MaKH == model.MaKH).FirstOrDefault();
                    if (existingCustomer == null)
                    {
                        ModelState.AddModelError("", "Khách hàng không tồn tại.");
                        return View(model);
                    }

                    existingCustomer.TenKH = model.TenKH;
                    existingCustomer.NgaySinh = model.NgaySinh;
                    existingCustomer.DiaChi = model.DiaChi;
                    existingCustomer.Email = model.Email;
                    existingCustomer.SDT = model.SDT;
                    existingCustomer.GioiTinh = model.GioiTinh;

                    _dbContext.SaveChanges();

                    return RedirectToAction("QuanLyKhachHang");
                }
                catch (Exception ex)
                {
                    ViewBag.lstKhachHang = _dbContext.KhachHangs.ToList();
                    ModelState.AddModelError("", "Đã xảy ra lỗi: " + ex.Message);
                }
            }

            ViewBag.lstKhachHang = _dbContext.KhachHangs.ToList();
            return View(model);
        }


        public ActionResult DeleteKhachHang(int id)
        {
            try
            {
                KhachHang model = _dbContext.KhachHangs.Find(id);
                if (model != null)
                {
                    // Xóa các đơn hàng của khách hàng
                    List<DonHang> lstDH = _dbContext.DonHangs.Where(r => r.MaKH == id).ToList();
                    foreach (DonHang item in lstDH)
                    {
                        if (item != null)
                        {
                            // Nếu cần thiết, có thể cập nhật thông tin trước khi xóa
                            _dbContext.DonHangs.Remove(item);
                        }
                    }

                    // Xóa tài khoản của khách hàng
                    TaiKhoan tk = _dbContext.TaiKhoans.FirstOrDefault(r => r.MaTK == model.MaTK);
                    if (tk != null)
                    {
                        _dbContext.TaiKhoans.Remove(tk);
                    }

                    // Xóa khách hàng
                    _dbContext.KhachHangs.Remove(model);

                    _dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi xóa khách hàng: " + ex.Message;
            }

            return RedirectToAction("QuanLyKhachHang");
        }





    }
}