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
            BookContext bookContext = new BookContext();
            List<Book> books = bookContext.Books.ToList();
            return View(books);
        }

        // POST: Book
        [HttpPost]
        /*public ActionResult Index(Review review)
        {
            decimal rating = review.rating;
            string comment = review.comment;
            return View();
        }*/

        // GET: Book/Details/{isbn}
        [HttpGet]
        public ActionResult Details(string isbn)
        {
            BookContext bookContext = new BookContext();
            Book book = bookContext.Books.Where(bk => bk.ISBN == isbn).FirstOrDefault();
            return View(book);
        }

        /*[HttpGet] //To be removed
        public ActionResult Details(int id)
        {
            booklist.Add(new Book() { ISBN = "1", Title = "Sorcerer's Stone" });
            booklist.Add(new Book() { ISBN = "2", Title = "Chamber of Secrets" });

            if (id > booklist.Count)
                return HttpNotFound();
    
            return View(booklist[id - 1]);
        }*/
    }
}