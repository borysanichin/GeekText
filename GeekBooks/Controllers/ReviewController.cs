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
        BookContext db = new BookContext();
        public ActionResult Index(Review review)
        {

            List<Review> reviews = db.Reviews.ToList();
            decimal rating = review.Rating;
            string comment = review.Comment;
          
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
        public ActionResult CreateReview(Review reviewData)
        {

            var review = reviewData;

            if (ModelState.IsValid) { 

                 db.Reviews.Add(reviewData);
                 db.SaveChanges();

                 return RedirectToAction("Index");
             }
            return View();
        }
    }
}