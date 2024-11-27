using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebBanTra.Models;

namespace WebBanTra.Controllers
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
            return View(list);
        }

        public ActionResult CreateSanPham()
        {
            ViewBag.DanhMucs = GetDanhMucList();
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

            ViewBag.DanhMucs = GetDanhMucList();
            return View(model);
        }

        public ActionResult UpdateSanPham(int id)
        {
            var model = _dbContext.SanPhams.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            ViewBag.DanhMucs = GetDanhMucList();
            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateSanPham(SanPham model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingProduct = _dbContext.SanPhams.Find(model.MaSP);
                    if (existingProduct == null)
                    {
                        ModelState.AddModelError("", "Sản phẩm không tồn tại.");
                        return View(model);
                    }

                    existingProduct.TenSP = model.TenSP;
                    existingProduct.Gia = model.Gia;
                    existingProduct.MoTa_SanPham = model.MoTa_SanPham;
                    existingProduct.MaDM = model.MaDM;

                    _dbContext.SaveChanges();
                    return RedirectToAction("Admin");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Đã xảy ra lỗi: " + ex.Message);
                }
            }

            ViewBag.DanhMucs = GetDanhMucList();
            return View(model);
        }

        public ActionResult DeleteSanPham(int id)
        {
            try
            {
                var model = _dbContext.SanPhams.Find(id);
                if (model != null)
                {
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

        private ActionResult UserProfile()
        {
            return View();
        }
    }

}