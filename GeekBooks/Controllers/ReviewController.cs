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
        //initialize BookContext db
        BookContext db = new BookContext();

        //Temporarily list reviews in Review/Index
        // GET: Review
        public ActionResult Index(Review review)
        {
            ViewBag.UserName = "No data";
            if (review.BoolValue)
                ViewBag.UserName = "Anonymous user";
            else
                ViewBag.UserName = review.Username;
            ViewBag.BoolValue = review.BoolValue;
            List<Review> reviews = db.Reviews.ToList();              
          
            return View(reviews);
        }

        // GET: CreateReview/{id}
        [HttpGet]
        public ActionResult CreateReview(string id = "1")
        {
            //Check if user is logged in
            if(Session["Username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            //To Do: Check if user has purchased the book

            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var review = new Review();
            var book = new BookeModel();
            review.DatePosted = System.DateTime.Now;
            review.Username = (string)Session["Username"];

            //Check if book isbn is valid
            try
            {
                var isbn = db.Books.Where(i => i.ISBN == id).Select(i => i.ISBN).Single();
                review.ISBN = isbn;
            }
            catch(ArgumentNullException e)
            {
                Console.WriteLine(e.StackTrace);
                return View("Error");
                return RedirectToAction("Details", "Book", new { id = review.ISBN, username = review.Username });
            }
            ViewBag.ISBN = review.ISBN;

            return View(review);
        }

        // POST: CreateReview/{isbn}
        [HttpPost]
        public ActionResult CreateReview(Review reviewData)
        {
            if (string.IsNullOrEmpty(reviewData.Comment))
            {
                ModelState.AddModelError("Comment", "Comment cannot be empty.");
            }

            if (ModelState.IsValid)
            {
                FillReviewViewBag(reviewData, true);

                //Check if the user hasn't already made a review for the book, redirect to error page if true.
                var reviewUser = db.Reviews.Where(u => u.Username == reviewData.Username && u.ISBN == reviewData.ISBN).Select(u => u.Username).SingleOrDefault();
                if (reviewData.Username == reviewUser)
                {
                    //TempData["msg"] = "<script>alert('Only one review allowed per book');</script>";
                    return View("_ReviewError");

                    return RedirectToAction("Details", "Book", new { id = reviewData.ISBN, username = reviewData.Username });
                }
                //return View(); //for testing

                db.Reviews.Add(reviewData);

                //Try catch (just incase, functions the same as the previous code block) 
                try
                {
                    db.SaveChanges();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    return View("_ReviewError");
                }
                
                ModelState.Clear();

                //To Do: remember to route to Book/Details/{isbn}/{user}
                return RedirectToAction("Index");

                return RedirectToRoute("BookRoute", new { id = reviewData.ISBN });
            }
            else
            {
                FillReviewViewBag(reviewData, false);
                return View();
            }
        }

        //Pass Review Model data to CreateReview view for testing (with ViewBag, without saving to database)
        public void FillReviewViewBag(Review reviewData, bool flag)
        {
            if (flag)
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
            }
            else
            {
                ViewBag.ISBN = "No Data";
                ViewBag.Username = "No Data";
                ViewBag.Rating = "No Data";
                ViewBag.Comment = "No Data";
                ViewBag.DatePosted = "No Data";
                ViewBag.Anynomous = "No Data";
            }
        }
    }
}