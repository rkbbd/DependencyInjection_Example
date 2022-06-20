using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Model;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAuditsBL _audits;

        public HomeController(IAuditsBL audits)
        {
            _audits = audits;
        }
        public ActionResult Index()
        {
            ViewBag.title = _audits.SetTitle();
            return View();
        }
    }
}
