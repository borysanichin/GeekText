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
            if (review.Anonymous)
                ViewBag.UserName = "Anonymous user";
            else
                ViewBag.UserName = review.Username;
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
        //public ActionResult CreateReview(FormCollection fc)
        {
            if (string.IsNullOrEmpty(reviewData.Comment))
            {
                ModelState.AddModelError("Comment", "Comment cannot be empty.");
            }

            if (ModelState.IsValid)
            {
                ViewBag.ISBN = reviewData.ISBN;
                if (reviewData.Anonymous)
                    ViewBag.UserName = "Anonymous user";
                else
                    ViewBag.Username = reviewData.Username;
                ViewBag.Rating = reviewData.Rating;
                ViewBag.Comment = reviewData.Comment;
                ViewBag.DatePosted = reviewData.DatePosted;
                ViewBag.Anonymous = reviewData.Anonymous;
                db.Reviews.Add(reviewData);
                db.SaveChanges();
                return RedirectToAction("Index");
                //return View();
            }
            else
            {
                ViewBag.ISBN = "No Data";
                ViewBag.Username = "No Data";
                ViewBag.Rating = "No Data";
                ViewBag.Comment = "No Data";
                ViewBag.DatePosted = "No Data";
                ViewBag.Anynomous = "No Data";
                return View();
            }

            //var review = new Review();

            /*ViewBag.ISBN = fc["ISBN"];
            ViewBag.UserName = fc["UserName"];
            ViewBag.Rating = fc["Rating"];
            ViewBag.Comment = fc["Comment"];
            ViewBag.DatePosted = fc["DatePosted"];  //Derived attribute*/

            /*review.ISBN = fc["ISBN"];
            review.Username = fc["UserName"];
            review.Rating = Convert.ToDecimal(fc["Rating"]);
            review.Comment = fc["Comment"];
            review.DatePosted = Convert.ToDateTime(fc["DatePosted"]);*/

            /*var review = reviewData;

            if (ModelState.IsValid) { 

                 db.Reviews.Add(reviewData);
                 db.SaveChanges();

                 return RedirectToAction("Index");
             }
             */
            //return View("Index");

            //TODO: return Book/Details/{isbn}
            return View();
        }
    }
}