﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanTra.Models;
using WebBanTra.OOP;    

namespace WebBanTra.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Cart()
        {
            List<CartDetail> listCart = GetCart();
            return View(listCart);
        }

        public List<CartDetail> GetCart()
        {
            List<CartDetail> carts;
            if (Session["Cart"] == null)
            {
                carts = new List<CartDetail>();
                Session["Cart"] = carts;
                return Session["Cart"] as List<CartDetail>;
            }
            carts = Session["Cart"] as List<CartDetail>;
            return carts;
        }

        [HttpPost]
        public ActionResult UpdateCart(int id, int quantity)
        {
            List<CartDetail> listCart = GetCart();
            CartDetail cart = listCart.Find(p => p.MaSP == id);
            listCart.Find(p => p.MaSP == id).SoLuong = quantity;
            Session["Cart"] = listCart;
            return Json(new
            {
                success = true,
                data = new
                {
                    itemPrice = cart.TongTien, // Tổng tiền của toàn giỏ hàng
                    totalPrice = listCart.Sum(r=>r.TongTien) // Tổng thành tiền của 1 sản phẩm
                }
            });

        }

        public ActionResult AddProduct(int maSP, int soLuong = 1)
        {
            DB_BanTraEntities db = new DB_BanTraEntities();
            try
            {
                List<CartDetail> listCart = GetCart();
                if (db.SanPhams.Where(p => p.MaSP == maSP).Count() > 0 && listCart.All(r => r.MaSP != maSP))
                {
                    SanPham sp = db.SanPhams.Find(maSP);
                    CartDetail cart = new CartDetail();

                    cart.MaSP = maSP;
                    cart.TenSP = sp.TenSP;
                    cart.Gia = sp.Gia;
                    cart.SoLuong = soLuong;
                    cart.LinkAnh = db.Anh_SanPham.FirstOrDefault(p => p.MaSP == maSP).LinhAnh;
                    listCart.Add(cart);
                    Session["Cart"] = listCart;

                    return RedirectToAction("Product", "Product");
                }
                else if (listCart.Where(r => r.MaSP == maSP).Count() > 0)
                {
                    listCart.Where(r => r.MaSP == maSP).ToList().ForEach(p => p.SoLuong += soLuong);
                    return RedirectToAction("Product", "Product");
                }
                else
                {
                    return RedirectToAction("Product", "Product");
                }

            }
            catch
            {
                return RedirectToAction("Product", "Product");
            }
        }

        [HttpPost]
        public ActionResult AddCart(int maSP, int soLuong = 1)
        {
            DB_BanTraEntities db = new DB_BanTraEntities();
            try
            {
                List<CartDetail> listCart = GetCart();
                if (db.SanPhams.Any(p => p.MaSP == maSP) && listCart.All(r => r.MaSP != maSP))
                {
                    SanPham sp = db.SanPhams.Find(maSP);
                    CartDetail cart = new CartDetail
                    {
                        MaSP = maSP,
                        TenSP = sp.TenSP,
                        Gia = sp.Gia,
                        SoLuong = soLuong,
                        LinkAnh = db.Anh_SanPham.FirstOrDefault(p => p.MaSP == maSP)?.LinhAnh
                    };
                    listCart.Add(cart);
                    Session["Cart"] = listCart;

                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else if (listCart.Any(r => r.MaSP == maSP))
                {
                    CartDetail cart = listCart.Find(p => p.MaSP == maSP);
                    listCart.First(r => r.MaSP == maSP).SoLuong += soLuong;
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }




        void removeProduct(int maSP)
        {
            List<CartDetail> listCart = GetCart();
            listCart.RemoveAll(p => p.MaSP == maSP);
            Session["Cart"] = listCart;
        }

        public ActionResult RemoveProduct(int maSP)
        {
            removeProduct(maSP);
            return View("Cart", Session["Cart"] as List<CartDetail>);
        }

        public ActionResult DataCart()
        {
            List<CartDetail> listCart = GetCart();
            return PartialView(listCart);
        }

        public ActionResult Order()
        {
            DB_BanTraEntities db = new DB_BanTraEntities();
            int user = Convert.ToInt32(Session["MaTK"]);
            OrderDetail listOrderDetail = new OrderDetail();
            listOrderDetail.listCart = GetCart();
            KhachHang kh =  new KhachHang();
            kh = db.KhachHangs.Where(p => p.MaTK == user).FirstOrDefault();
            if(kh == null)
            {
                NhanVien nv = new NhanVien();
                nv = db.NhanViens.Where(p => p.MaTK == user).FirstOrDefault();
                kh = new KhachHang()
                {
                    MaKH = nv.MaNV,
                    TenKH = nv.TenNV,
                    DiaChi = nv.DiaChi,
                    Email = nv.Email,
                    SDT = nv.SDT,
                    GioiTinh = "Không có"
                };
            }
            listOrderDetail.khachHang = kh;
            return View(listOrderDetail);
        }

        public ActionResult DataOrder()
        {
            List<CartDetail> listCart = GetCart();
            return PartialView(listCart);
        }
    }
}