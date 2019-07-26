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
        //Initialize db object
        static BookContext db;

        public ReviewController()
        {
            db = new BookContext();
        }

        //free db object from memory
        ~ReviewController()
        {
            db.Dispose();
        }

        //Temporarily list reviews in Review/Index
        // GET: Review
        public ActionResult Index()
        {
            List<Review> reviews =  db.Reviews.ToList();              
          
            return View(reviews);
        }

        // GET: CreateReview/{id}
        [HttpGet]
        public ActionResult CreateReview(string id = "1")
        {
            bool flag = false;
            //Check if user is logged in
            if(Session["Username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            //To Do: Check if user has purchased the book

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Creating review model objects to exchange data for display
            ReviewModel reviewM = new ReviewModel();
            Review review = new Review();
 
            reviewM.DatePosted = System.DateTime.Now;
            reviewM.Username = (string)Session["Username"];

            //query to search user-book purchases
            var userPurchasedQuery = from u in db.Users
                                join p in db.Purchaseds
                                on u.Username equals p.Username
                                join b in db.Books
                                on p.ISBN equals b.ISBN
                                select new
                                {
                                    p.ISBN,
                                    p.Username
                                };

            //if one of the queried records matches the logged in user, set flag to true
            foreach(var userPurchases in userPurchasedQuery)
            {
                if(userPurchases.Username == (string)Session["Username"] && userPurchases.ISBN == id)
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }
            }

            //if user purchased the book, allow him or her to review it
            if (flag)
            {
                flag = false;
                //Check if book isbn is valid
                try
                {
                    var isbn = db.Books.Where(i => i.ISBN == id).Select(i => i.ISBN).Single();
                    reviewM.ISBN = isbn;
                }
                catch (ArgumentNullException e)
                {
                    Console.WriteLine(e.StackTrace);
                    return View("Error");
                    return RedirectToAction("Details", "Book", new { id = review.ISBN, username = review.Username });
                }
                ViewBag.ISBN = reviewM.ISBN;

                //exchange data between review objects
                FillReviewModel(review, reviewM);

                return View(reviewM);
            }
            else
            {
                return Content("<script>alert('You have not purchased this book'); window.history.go(-1);</script>");
            }
        }

        // POST: CreateReview/{isbn}
        [HttpPost]
        public ActionResult CreateReview(ReviewModel reviewData)
        {
            if (string.IsNullOrEmpty(reviewData.Comment))
            {
                ModelState.AddModelError("Comment", "Comment cannot be empty.");
            }

            if (ModelState.IsValid)
            {

                //BookContext.edmx/BookContext.tt/Review object
                Review review = new Review();

                FillReviewModel(review, reviewData);

                FillReviewViewBag(reviewData, true);

                //Check if the user hasn't already made a review for the book, redirect to error page if true.
                var reviewUser = db.Reviews.Where(r => r.Username == reviewData.Username && r.ISBN == reviewData.ISBN).Select(u => u.Username).SingleOrDefault();
                if (reviewData.Username == reviewUser)
                {
                    return View("_ReviewError");
                }
                return View(); //for testing

                db.Reviews.Add(review);

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
                //return RedirectToAction("Index");

                return RedirectToRoute("BookRoute", new { id = reviewData.ISBN, username = reviewData.Username });
            }
            else
            {
                FillReviewViewBag(reviewData, false);
                return View();
            }
        }

        //Pass Review Model data to CreateReview view for testing (with ViewBag, without saving to database)
        public void FillReviewViewBag(ReviewModel reviewData, bool flag)
        {
            if (flag)
            {
                var nickname = db.Users.Where(u => u.Username == reviewData.Username).Select(u => u.Nickname).SingleOrDefault();
                ViewBag.ISBN = reviewData.ISBN;
                if (reviewData.BoolValue)
                    ViewBag.UserName = "Anonymous";
                else
                    ViewBag.Username = reviewData.Username;
                if (reviewData.Nickname)
                    ViewBag.UserName = nickname;
                else
                    ViewBag.UserName = reviewData.Username;
                ViewBag.Rating = reviewData.Rating;
                ViewBag.Comment = reviewData.Comment;
                ViewBag.DatePosted = reviewData.DatePosted;
                ViewBag.BoolValue = reviewData.BoolValue;
                ViewBag.Anonymous = reviewData.Anonymous;
                ViewBag.NickName = reviewData.Nickname;
            }
            else
            {
                ViewBag.ISBN = "No Data";
                ViewBag.Username = "No Data";
                ViewBag.Rating = "No Data";
                ViewBag.Comment = "No Data";
                ViewBag.DatePosted = "No Data";
                ViewBag.Anynomous = "No Data";
                ViewBag.Nickname = "No Data";
            }
        }

        //Used to fill /BookContext.edmx/BookContext.tt/Review.cs object with ReviewModel.cs object data
        public void FillReviewModel(Review review, ReviewModel reviewM)
        {
            review.ISBN = reviewM.ISBN;
            review.Username = reviewM.Username;
            review.Rating = reviewM.Rating;
            review.Comment = reviewM.Comment;
            review.DatePosted = reviewM.DatePosted;
            review.Anonymous = reviewM.Anonymous;
            review.Nickname = reviewM.Nickname;
        }
    }
}