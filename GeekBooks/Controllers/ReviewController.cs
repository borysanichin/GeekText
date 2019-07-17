using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            //Session["Username"] = "guest2";
            ViewBag.UserName = "No data";
            if (review.BoolValue)
                ViewBag.UserName = "Anonymous user";
            else
                ViewBag.UserName = review.Username;
            ViewBag.BoolValue = review.BoolValue;
            ViewBag.Test = "Testing";
            List<Review> reviews = db.Reviews.ToList();              
            //decimal rating = review.Rating;
            //string comment = review.Comment;
          
            return View(reviews);
        }

        // GET: CreateReview
        [HttpGet]
        public ActionResult CreateReview(string id = "1")
        {
            //To Do: Check if user is logged in
            if(Session["Username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var review = new Review();
            var book = new BookeModel();
            review.DatePosted = System.DateTime.Now;
            review.Username = (string)Session["Username"];
            var isbn = db.Books.Where(i => i.ISBN == id).Select(i => i.ISBN).Single();
            review.ISBN = isbn;
            /*var isbn = from b in db.Books
                       join r in db.Reviews
                       on b.ISBN equals r.ISBN
                       where b.ISBN == id
                       select new {
                           _ISBN = b.ISBN
                       };
            foreach(var i in isbn)
            {
                review.ISBN = i._ISBN;
                id = i._ISBN;
            }
            if(book.ISBN != id)
            {
                return HttpNotFound();
            }

            if (id.Equals(book.ISBN))
            {
                review.ISBN = book.ISBN;

            }*/
            ViewBag.ISBN = review.ISBN;
            //ModelState.Clear();
            return View(review);
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
                if (reviewData.BoolValue)
                    ViewBag.UserName = "Anonymous user";
                else
                    ViewBag.Username = reviewData.Username;
                ViewBag.Rating = reviewData.Rating;
                ViewBag.Comment = reviewData.Comment;
                ViewBag.DatePosted = reviewData.DatePosted;
                ViewBag.BoolValue = reviewData.BoolValue;
                ViewBag.Anonymous = reviewData.Anonymous;
                //To Do: Check if user has purchased the book
                var reviewPrimaryKey = db.Reviews.Where(u => u.Username == reviewData.Username && u.ISBN == reviewData.ISBN).Select(u => u.Username).Single();
                if (reviewData.Username == reviewPrimaryKey)
                {
                    TempData["msg"] = "<script>alert('Only one review allowed per book');</script>";
                    return RedirectToAction("Details", "Book", new { id = reviewData.ISBN, username = "guest" });
                }
                db.Reviews.Add(reviewData);
                db.SaveChanges();
                
                ModelState.Clear();
                return RedirectToAction("Index");
                return RedirectToRoute("BookRoute", new { id = reviewData.ISBN });
                return View();
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