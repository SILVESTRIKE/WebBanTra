using System;
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
                if (db.SanPhams.Where(p => p.MaSP == maSP).Count() > 0 && listCart.Where(r =>r.MaSP == maSP).Count() == 0)
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
    }
}