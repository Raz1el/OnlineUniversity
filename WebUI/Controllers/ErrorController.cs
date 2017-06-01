using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index(string error)
        {
            ViewBag.Error = error;
            return View();
        }
    }
}