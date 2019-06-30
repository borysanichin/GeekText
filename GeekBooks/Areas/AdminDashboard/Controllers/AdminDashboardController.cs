using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeekBooks.Areas.AdminDashboard.Controllers
{
    public class AdminDashboardController : Controller
    {
        // GET: AdminDashboard/AdminDashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}