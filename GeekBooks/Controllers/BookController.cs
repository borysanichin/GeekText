using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeekBooks.Models;

namespace GeekBooks.Controllers
{
    public class BookController : Controller
    {
        //List<Book> booklist = new List<Book>(); //To be removed

        // GET: Book
        public ActionResult Index()
        {
            GeektextContext bookContext = new GeektextContext();
            List<Book> books = bookContext.Books.ToList();
            return View(books);
        }

        // POST: Book
        [HttpPost]
        public ActionResult Index(Review review)
        {
            decimal rating = review.Rating;
            string comment = review.Comment;
            return View();
        }


        [HttpGet]
        public ActionResult Details(string id)
        {
            GeektextContext bookContext = new GeektextContext();
            Book book = bookContext.Books.Single(bk => bk.ISBN == id);
            return View(book);
        }

        /*[HttpGet] //To be removed
        public ActionResult Details(int id)
        {
            booklist.Add(new Book() { Id = 1, Name = "Sorcerer's Stone" });
            booklist.Add(new Book() { Id = 2, Name = "Chamber of Secrets" });

            if (id > booklist.Count)
                return HttpNotFound();
    
            return View(booklist[id - 1]);
        }*/
    }
}