using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoWeb.Models;

namespace DemoWeb.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        #region Variable
        WebDBContext db = new WebDBContext();
        #endregion
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(tbl_TaiKhoan model, FormCollection collection)
        {
            var tentaikhoan = model.TEN_TAIKHOAN;
            var matkhau = model.MAT_KHAU;         
            var item = db.tbl_TaiKhoan.Where(x => x.TEN_TAIKHOAN == tentaikhoan && x.MAT_KHAU == matkhau && x.TRANG_THAI == 1).FirstOrDefault();

            if (item != null)
            {
                Session["UserLogin"] = item;
                if (item.NGAY_HIEULUC < DateTime.Now)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Hết hiệu lực");
                    return View();
                }

            }
            else
            {
                if ((tentaikhoan == null || matkhau == null) || (tentaikhoan == null && matkhau == null))
                {
                    ModelState.AddModelError("", "Tên tài khoản hoặc mật khẩu không được để trống. Vui lòng nhập lại!");
                }
                else
                {
                    ModelState.AddModelError("", "Thông tin tài khoản hoặc mật khẩu không đúng!!!");                  
                }
                return View();
            }
        }
        [HttpPost]
        public JsonResult ReturnURL(string Email, string FirstName, string LastName, string GoogleID, string ProfileURL)
        {         
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}