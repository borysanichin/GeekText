using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeekBooks.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            //var url = Url.Action("Buy", "Products", new { id = 17 });
            return View();
        }

        public ActionResult Books()
        {
            return View();
        }

        public ActionResult ShoppingCart()
        {
            return View();
        }
    }
}