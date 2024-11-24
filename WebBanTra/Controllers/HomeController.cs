using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanTra.Models;
namespace WebBanTra.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            DB_BanTraEntities db = new DB_BanTraEntities();
            ViewBag.DanhMuc = db.DanhMucs.ToList();
            ViewBag.DanhMucCount = db.DanhMucs.Count();
            base.OnActionExecuting(filterContext);
        }
        public ActionResult Home()
        {
            return View();
        }
    }
}