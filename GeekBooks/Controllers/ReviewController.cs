using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeekBooks.Models;

namespace GeekBooks.Controllers
{
    public class ReviewController : Controller
    {
        // GET: Review
        public ActionResult Index()
        {
            GeektextContext bookContext = new GeektextContext();
            List<Review> reviews = bookContext.Reviews.ToList();
            return View(reviews);
        }

        // POST: CreateReview
        public ActionResult CreateReview()
        {
            return View();
        }
    }
}