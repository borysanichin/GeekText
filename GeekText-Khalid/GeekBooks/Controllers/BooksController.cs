using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GeekBooks.DAL;

namespace GeekBooks.Controllers
{
    public class BooksController : Controller
    {
        private BookContext db = new BookContext();

        // GET: Books
        public ActionResult Index(string movieGenre, string searchString)
        {

            var GenreList = new List<string>();

           // var GenreLst = from d in db.Genres
                 //          select d.GenreName;

            var GenreQry = from d in db.Genres
                           orderby d.GenreName
                           select d.GenreName;

            GenreList.AddRange(GenreQry.Distinct());
            
            ViewBag.movieGenre = new SelectList(GenreList);

            var books = from m in db.Books
                        select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.Title.Contains(searchString));
            }
            /*
            if (!String.IsNullOrEmpty(movieGenre))
            {
                books = books.Where(x => x.GenreName == movieGenre);
            }*/

            return View(books);
        }

        // GET: Books/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

       public ActionResult ShoppingCart(int? id)//id + userID i assume
        {
            //Code to replace, just added to compile
            Book book = db.Books.Find(id);
            return View(book);
        }

        public ActionResult WishList(int? id)//id + userID i assume)
        {
            //Code to replace, just added to compile
            Book book = db.Books.Find(id);
            return View(book);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
