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
            BookContext bookContext = new BookContext();
            List<Review> reviews = bookContext.Reviews.ToList();
            return View(reviews);
        }

        // GET: CreateReview
        [HttpGet]
        public ActionResult CreateReview()
        {
            return View();
        }

        // POST: CreateReview
        [HttpPost]
        public ActionResult CreateReview(Models.Review reviewData)
        {
            if(ModelState.IsValid)
                return RedirectToAction("Index", "Book");
            return View();
        }
    }
}