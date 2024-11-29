using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebBanTra.Models;
using WebBanTra.OOP;


namespace WebBanTra.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        private readonly DB_BanTraEntities _dbBanTra;
        DB_BanTraEntities db = new DB_BanTraEntities();

        public ActionResult Product(int page = 1)
        {
            int pageSize = 9;

            List<WebBanTra.Models.SanPham> listProducts = db.SanPhams.ToList().Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ProductViewModel productViewModels = new ProductViewModel
            {
                listProducts = listProducts,
                listAnhSP = db.Anh_SanPham.ToList(),
                totalPage = (int)Math.Ceiling((double)db.SanPhams.Count() / pageSize),
                currentPage = page
            };

            return View(productViewModels);
        }
        public ActionResult ChiTietSanPham(int id = 0)
        {
            using (db)
            {
                ViewBag.limitQuantity = db.SanPhams.Where(sp => sp.MaSP == id).Select(sp => sp.SoLuongTon).FirstOrDefault();
                var product = db.SanPhams
                    .Where(sp => sp.MaSP == id)
                    .Select(sp => new ProductDetailViewModel
                    {
                        MaSP = id,
                        TenSP = sp.TenSP,
                        Gia = sp.Gia,
                        Images = sp.Anh_SanPham.Select(a => a.LinhAnh).ToList(),
                        Descriptions = sp.MoTa_SanPham.Select(m => m.MoTa).ToList(),
                        DanhMuc = sp.DanhMuc.TenDM
                    }).FirstOrDefault();

                if (product == null)
                {
                    return HttpNotFound();
                }
                return View(product);
            }
        }


        public ActionResult Search(string nameSP, int page = 1)
        {
            ProductViewModel productViewModels;
            if (nameSP == null)
            {
                return RedirectToAction("Product");
            }
            else
            {
                int pageSize = 9;
                List<WebBanTra.Models.SanPham> listProducts = string.IsNullOrEmpty(nameSP)
                ? db.SanPhams.ToList()
                : db.SanPhams.Where(p => p.TenSP.Contains(nameSP)).ToList();
                ViewBag.ListAnhSP = db.Anh_SanPham.ToList().Skip((page - 1) * pageSize).Take(pageSize).ToList();

                productViewModels = new ProductViewModel
                {
                    listProducts = listProducts,
                    listAnhSP = db.Anh_SanPham.ToList(),
                    totalPage = (int)Math.Ceiling((double)db.SanPhams.Where(r => r.TenSP.Contains(nameSP)).Count() / pageSize),
                    currentPage = page
                };

            }
            return View("Product", productViewModels);
        }

        public ActionResult SearchDM(string nameDM, int page = 1)
        {
            ProductViewModel productViewModels;
            if (nameDM == null)
            {
                return RedirectToAction("Product");
            }
            else
            {
                int pageSize = 9;
                int MaDM = Convert.ToInt16(nameDM);
                List<WebBanTra.Models.SanPham> listProducts = string.IsNullOrEmpty(nameDM)
                ? null
                : db.SanPhams.Where(p => p.MaDM == MaDM).ToList();
                ViewBag.ListAnhSP = db.Anh_SanPham.ToList().Skip((page - 1) * pageSize).Take(pageSize).ToList();

                productViewModels = new ProductViewModel
                {
                    listProducts = listProducts,
                    listAnhSP = db.Anh_SanPham.ToList(),
                    totalPage = (int)Math.Ceiling((double)db.SanPhams.Where(r => r.MaDM == MaDM).Count() / pageSize),
                    currentPage = page
                };
            }
            return View("Product", productViewModels);
        }

        public ActionResult SearchGia(string GiaBatDau, string GiaKetThuc, int page = 1)
        {
            ProductViewModel productViewModels;
            if (GiaBatDau == null || GiaKetThuc == null)
            {
                return RedirectToAction("Product");
            }
            else
            {
                int pageSize = 9;
                int giaBD = Convert.ToInt32(GiaBatDau);
                int giaKT = Convert.ToInt32(GiaKetThuc);
                List<WebBanTra.Models.SanPham> listProducts = string.IsNullOrEmpty(GiaBatDau) || string.IsNullOrEmpty(GiaKetThuc)
                ? null
                : db.SanPhams.Where(p => p.Gia >= giaBD && p.Gia <= giaKT).ToList();
                ViewBag.ListAnhSP = db.Anh_SanPham.ToList().Skip((page - 1) * pageSize).Take(pageSize).ToList();

                productViewModels = new ProductViewModel
                {
                    listProducts = listProducts,
                    listAnhSP = db.Anh_SanPham.ToList(),
                    totalPage = (int)Math.Ceiling((double)db.SanPhams.Where(p => p.Gia >= giaBD && p.Gia <= giaKT).Count() / pageSize),
                    currentPage = page
                };
            }
            return View("Product", productViewModels);
        }

        public ActionResult SanPham(int page = 1, int NoId = 0, string TenDM = "", int pageSize = 9)
        {
            List<WebBanTra.Models.SanPham> listProducts = db.SanPhams.Where(r => r.MaSP != NoId && r.DanhMuc.TenDM == TenDM).ToList().Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ProductViewModel productViewModels = new ProductViewModel
            {
                listProducts = listProducts,
                listAnhSP = db.Anh_SanPham.ToList(),
                totalPage = (int)Math.Ceiling((double)db.SanPhams.Count() / pageSize),
                currentPage = page
            };
            return PartialView(productViewModels);
        }

        public ActionResult DanhMuc()
        {
            DB_BanTraEntities db = new DB_BanTraEntities();
            List<DanhMuc> dm = db.DanhMucs.ToList();
            return PartialView(dm);
        }
    }
}