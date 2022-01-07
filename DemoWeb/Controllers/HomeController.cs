using DemoWeb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoWeb.Controllers
{
	public class HomeController : Controller
    {
		#region Variable
		WebDBContext db = new WebDBContext();
        #endregion
        // GET: Home
        [Authorize]
		public ActionResult Index()
        {
			List<DataPoint> dataPoints1 = new List<DataPoint>();
			List<DataPoint> dataPoints2 = new List<DataPoint>();
			List<DataPoint> dataPoints3 = new List<DataPoint>();
			List<DataPoint> dataPoints4 = new List<DataPoint>();
			var model = db.tbl_NangLuong.Where(x => x.TEN_TIEUDE == "Mặt Trời").ToList();
            foreach (var item in model)
            {
				dataPoints1.Add(new DataPoint(item.TGian.Value, Convert.ToDouble(item.CS_HIENTAI)));
			}
			var model2 = db.tbl_NangLuong.Where(x => x.TEN_TIEUDE == "Gió").ToList();
			foreach (var item in model2)
			{
				dataPoints2.Add(new DataPoint(item.TGian.Value, Convert.ToDouble(item.CS_HIENTAI)));
			}
			var model3 = db.tbl_NangLuong.Where(x => x.TEN_TIEUDE == "Sinh Khối").ToList();
			foreach (var item in model3)
			{
				dataPoints3.Add(new DataPoint(item.TGian.Value, Convert.ToDouble(item.CS_HIENTAI)));
			}
			var model4 = db.tbl_NangLuong.Where(x => x.TEN_TIEUDE == "Tổng").ToList();
			foreach (var item in model4)
			{
				dataPoints4.Add(new DataPoint(item.TGian.Value, Convert.ToDouble(item.CS_HIENTAI)));
			}
			ViewBag.DataPoints1 = JsonConvert.SerializeObject(dataPoints1);
			ViewBag.DataPoints2 = JsonConvert.SerializeObject(dataPoints2);
			ViewBag.DataPoints3 = JsonConvert.SerializeObject(dataPoints3);
			ViewBag.DataPoints4 = JsonConvert.SerializeObject(dataPoints4);

			return View();
		}

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}