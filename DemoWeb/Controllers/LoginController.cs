using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
            var matkhau = GetMD5(model.MAT_KHAU);         
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
        //Get: Dang ky
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]

        public ActionResult Register(tbl_TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                var check = db.tbl_TaiKhoan.FirstOrDefault(s => s.TEN_TAIKHOAN == taiKhoan.TEN_TAIKHOAN);
                if (check == null)
                {
                    taiKhoan.MAT_KHAU = GetMD5(taiKhoan.MAT_KHAU);
                    taiKhoan.NGAY_HIEULUC = DateTime.Now;
                    taiKhoan.TRANG_THAI = 1;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.tbl_TaiKhoan.Add(taiKhoan);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    ModelState.AddModelError("", "Tên tài khoản đã tồn tại!");
                    return View();
                }
            }
            return View();
        }
        //Đăng xuất
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index","Login");
        }
        //Mã hóa mật khẩu bằng MD5
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;
            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");
            }
            return byte2String;
        }
    }
}