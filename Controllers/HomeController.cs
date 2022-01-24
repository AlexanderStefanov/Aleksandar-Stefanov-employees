using LongestCoWorkingPeriod.Models;
using LongestCoWorkingPeriod.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LongestCoWorkingPeriod.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            PairDataModel result = null;
            //TODO: Inject LongestCoWorkingPairFinder 
            var finder = new LongestCoWorkingPairFinder();
            if (file != null && file.ContentLength > 0 && Path.GetExtension(file.FileName) == ".csv")
            {
                try
                {
                    result = finder.FindLongestCoworkingPairFromCSVFile(file);
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorInfo = ex.Message;
                }
            }
            return View(result);
        }
    }
}