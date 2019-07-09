using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using GeekBooks.Models;
using PagedList;
using GeekBooks.ViewModels;
//using GeekBooks.Models;

namespace GeekBooks.Controllers
{
    public class BookController : Controller
    {

        private BookContext db = new BookContext();
        //List<Book> booklist = new List<Book>(); //To be removed

        // GET: Book
        public ActionResult Index(string sortOrder, string movieGenre, string searchString,
                                  string currentFilter, int? page)
        {
            Search sh = new Search();
            Sort st = new Sort();
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";
            ViewBag.AuthorSortParm = sortOrder == "Author" ? "author_desc" : "Author";
            ViewBag.RatingSortParm = sortOrder == "Rating" ? "rating_desc" : "Rating";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";


            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var GenreList = sh.genreList(db);
            ViewBag.movieGenre = new SelectList(GenreList);



            var book = sh.search(db, searchString, movieGenre);
            book = st.sort(book, sortOrder);



            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(book.ToPagedList(pageNumber, pageSize));


            // return View(book);
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


            
            bookM.BookModel = db.Books.Find(id);
           
            bookM.BookGenreModel = db.BookGenres.SingleOrDefault(m => m.ISBN == id);

           /* var viewBook = from m in db.Books
                           join n in db.BookGenres on m.ISBN equals n.ISBN
                           where m.ISBN == id
                           select new BookeModel { BookModel = m, BookGenreModel = n };*/

            // var test = viewBook.Find(id);

            if (bookM == null)
            {
                return HttpNotFound();
            }
            return View(bookM);
        }

        /*I am still working on this Book Details View to 
          include add book to wishlist functionality. I am 
          using BookDetailsViewModel(also not finished)
          in ViewModels folder to pass all the data to the
          view (Borys).*/
        [Route("Book/BookDetails/{isbn}")]
        public ActionResult BookDetails(string isbn)
        {
            var viewModel = new BookDetailsViewModel
            {
                Book = db.Books.Find(isbn),
                BookGenre = db.BookGenres.Where(b => b.ISBN == isbn).FirstOrDefault(),
                Wishlists = db.Wishlists.Where(b => b.Username == "guest").ToList()
            };
            return View(viewModel);
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