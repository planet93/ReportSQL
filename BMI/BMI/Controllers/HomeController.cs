using BMI.Models;
using BMI.Services;
using BMI.Services.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BMI.Controllers
{
    public class HomeController : Controller
    {
        private readonly LoadDataServices mICs;
        private readonly SpendServices mSs;
        public HomeController()
        {
            mICs = new LoadDataServices();
            mSs = new SpendServices();
        }
        public ActionResult Index()
        {
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

        public ActionResult Spend()
        {
            //ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult SpendTwo()
        {
            return View();
        }

        
        public ActionResult LoadData()
        {
            return View();
        }

        public JsonResult GetSpends()
        {
            //var res = mSs.GetSpends();
            //var res = mSs.GetSpendsFast();
            var res = mSs.GetSpendFromDDM();
            return Json(res);
        }

        public JsonResult PostMasterData(HttpPostedFileBase file, string type)
        {
            if (file != null)
            {
                var fileName = file.FileName.ToLower().Trim();
                var savePathBase = Server.MapPath("~/Content/");
                var logFile = Server.MapPath("~/Content/") + "Log.txt";
                var fileExtension = IO.GetExtensionFromString(fileName);
                var newFileName = RandomGenerator.RandomString();
                var savePath = $"{savePathBase}{newFileName}{fileExtension}";
                file.SaveAs(savePath);

                ResultViewModel result = new ResultViewModel();

                //В зависимости от выбраного типа загрузки 
                switch (type)
                {
                    case "ClassifierType":
                        result = mICs.CreateClassifierType(mICs.ParserClassifierType(savePath));
                        break;
                    case "Spends":
                        result = mICs.CreateSpend(mICs.ParserClassifierType(savePath));
                        break;
                    case "Spend4Level":
                        result = mICs.CreateSpend4Level(mICs.ParserClassifierType(savePath));
                        break;
                }
                System.IO.File.Delete(savePath);

                return Json(result);
            }

            return Json("ok");
        }

    }
}