using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DemoWeb.Models;
using Facebook;
using System.Web.Security;

namespace DemoWeb.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        #region Variable
        WebDBContext db = new WebDBContext();
        #endregion    
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(tbl_TaiKhoan model, FormCollection collection)
        {
            var tentaikhoan = model.TEN_TAIKHOAN;
            var matkhau =GetMD5(model.MAT_KHAU);         
            var item = db.tbl_TaiKhoan.Where(x => x.TEN_TAIKHOAN == tentaikhoan && x.MAT_KHAU == matkhau && x.TRANG_THAI == 1).FirstOrDefault();
            if (item != null)
            {
                Session["UserLogin"] = item;
                if (item.NGAY_HIEULUC < DateTime.Now)
                {
                    FormsAuthentication.SetAuthCookie(model.TEN_TAIKHOAN, false);
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
            FormsAuthentication.SignOut();
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
        //login facebook
        private Uri RediredtUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }
        [AllowAnonymous]
        public ActionResult Facebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = "1885657841637327",
                client_secret = "a6af87a2d66a02c302879136245618ad",
                redirect_uri = RediredtUri.AbsoluteUri,
                response_type = "code",
                scope = "email"
            });
            return Redirect(loginUrl.AbsoluteUri);
        }
        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = "1885657841637327",
                client_secret = "a6af87a2d66a02c302879136245618ad",
                redirect_uri = RediredtUri.AbsoluteUri,
                code = code
            });

            var accessToken = result.access_token;
            Session["AccessToken"] = accessToken;
            fb.AccessToken = accessToken;
            dynamic me = fb.Get("me?fields=link,first_name,currency,last_name,email,gender,locale,timezone,verified,picture,age_range");
            string email = me.email;

            TempData["email"] = me.email;
            TempData["first_name"] = me.first_name;
            TempData["last_name"] = me.last_name;
            TempData["picture"] = me.picture;
            FormsAuthentication.SetAuthCookie(email, false);
            return RedirectToAction("Index", "Home");
        }
    }
}