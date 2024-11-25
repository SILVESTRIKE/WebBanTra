using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
                var product = db.SanPhams
                    .Where(sp => sp.MaSP == id)
                    .Select(sp => new ProductDetailViewModel
                    {
                        TenSP = sp.TenSP,
                        Gia = sp.Gia,
                        Images = sp.Anh_SanPham.Select(a => a.LinhAnh).ToList(),
                        Descriptions = sp.MoTa_SanPham.Select(m => m.MoTa).ToList(),
                        DanhMuc = sp.DanhMuc.TenDM
                    })
                    .FirstOrDefault();

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
    }
}