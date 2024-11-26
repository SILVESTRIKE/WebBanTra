using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanTra.Models;
using WebBanTra.OOP;
using System.Data.Entity;


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
            var product = db.SanPhams
                .Include(sp => sp.Anh_SanPham) 
                .Include(sp => sp.MoTa_SanPham)
                .Include(sp => sp.DanhMuc)
                .FirstOrDefault(sp => sp.MaSP == id);

            if (product == null)
            {
                return HttpNotFound();
            }
            var relatedProducts = db.SanPhams
                            .Where(p => p.DanhMuc.MaDM == product.DanhMuc.MaDM && p.MaSP != id)
                            .Take(4)
                            .ToList();
            ViewBag.RelatedProducts = relatedProducts;
            return View(product);
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

        public ActionResult DanhMuc()
        {
            DB_BanTraEntities db = new DB_BanTraEntities();
            List<DanhMuc> dm = db.DanhMucs.ToList();
            return PartialView(dm);
        }

    }
}