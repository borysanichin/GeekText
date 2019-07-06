using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Routing;
using GeekBooks.Models;
//using GeekBooks.Models;

namespace GeekBooks.Controllers
{
    public class BookController : Controller
    {

        private BookContext db = new BookContext();
        //List<Book> booklist = new List<Book>(); //To be removed

        // GET: Book
        public ActionResult Index(string movieGenre, string searchString)
        {

            var GenreList = new List<string>();

            var GenreQry = from d in db.Genres
                           orderby d.GenreName
                           select d.GenreName;


           GenreList.AddRange(GenreQry.Distinct());

            ViewBag.movieGenre = new SelectList(GenreList);

            var book = from m in db.Books
                           join n in db.BookGenres on m.ISBN equals n.ISBN
                           select new BookeModel { BookModel = m, BookGenreModel = n };

            if (!String.IsNullOrEmpty(searchString))
            {
                //books = books.Where(s => s.BookModel.Title.Contains(searchString));\
                book = book.Where(s => s.BookModel.Title.Contains(searchString));
            }

            
            if (!String.IsNullOrEmpty(movieGenre))
            {
                book = book.Where(x => x.BookGenreModel.GenreName == movieGenre);
            }
            

            return View(book);
        }
         //Can u please integrate this with the other index method
        // POST: Book
       /* [HttpPost]
        public ActionResult Index(Review review)
        {
            decimal rating = review.Rating;
            string comment = review.Comment;
            return View();
        }*/


        
        public ActionResult Details(string id, BookeModel bookM)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            //BookeModel booke = new BookeModel();
            bookM.BookModel = db.Books.Find(id);
            //bookM.BookGenreModel = db.BookGenres.Find();
            bookM.BookGenreModel = db.BookGenres.SingleOrDefault(m => m.ISBN == id);

            var viewBook = from m in db.Books
                           join n in db.BookGenres on m.ISBN equals n.ISBN
                           where m.ISBN == id
                           select new BookeModel { BookModel = m, BookGenreModel = n };

           // var test = viewBook.Find(id);


            if (viewBook == null)
            {
                return HttpNotFound();
            }
            return View(bookM);
        }

        public ActionResult WishList()
        {
            return RedirectToAction("WishList", "Account");
           
            return View();
        }
        
        public ActionResult ShoppingCart()
        {
            return RedirectToAction("ShoppingCart", "Home");
            return View();
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