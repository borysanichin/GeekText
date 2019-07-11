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


        public ActionResult Index(string sortOrder, string movieGenre, string searchString,
                                    string currentFilter, int? page, int? authorID)
        {
            Search sh = new Search();
            Sort st = new Sort();
            IQueryableBookeModel bm = new IQueryableBookeModel();

            var book = bm.newBookModel(db);

            if (authorID != null)
            {
                book = book.Where(a => a.AuthorModel.AuthorID == authorID);
            }


            ViewBag.AuthorID = authorID;
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
            ViewBag.GenreFilter = movieGenre;



            book = sh.search(db, searchString, movieGenre, authorID, book);
            book = st.sort(book, sortOrder, authorID);



            int pageSize = 10;
            int pageNumber = (page ?? 1);


            return View(book.ToPagedList(pageNumber, pageSize));





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

        [Route("Book/Details/{id}/{username}")]
        public ActionResult Details(string id, string username)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var bmodel = new BookeModel
            {
                WroteModel = db.Wrotes.SingleOrDefault(w => w.ISBN == id)
            };

            var bookmodel = new BookeModel
            {
                BookModel = db.Books.Find(id),
                BookGenreModel = db.BookGenres.SingleOrDefault(m => m.ISBN == id),
                AuthorModel = db.Authors.SingleOrDefault(b => b.AuthorID == bmodel.WroteModel.AuthorID),
                Wishlists = db.Wishlists.Where(b => b.Username == username).ToList(),
                username = username
            };

            //bookM.BookModel = db.Books.Find(id);

            //bookM.BookGenreModel = db.BookGenres.SingleOrDefault(m => m.ISBN == id);

            /* var viewBook = from m in db.Books
                            join n in db.BookGenres on m.ISBN equals n.ISBN
                            where m.ISBN == id
                            select new BookeModel { BookModel = m, BookGenreModel = n };*/

            // var test = viewBook.Find(id);

            if (bookmodel == null)
            {
                return HttpNotFound();
            }
            
            return View(bookmodel);
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
            return RedirectToAction("ShoppingCart", "Account");
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