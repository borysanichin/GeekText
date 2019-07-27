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
       


        public static List<string> gL = new List<string>() { "All" };




        public static BookContext db = new BookContext();

        public static List<string> GenreList = (from m in db.Genres
                                                select m.GenreName).ToList();




        public ActionResult Index(string sortOrder, string searchString, string bookGenre, string removeGenre,
                                    string currentFilter, int? page, int? authorID)
        {

            Search sh = new Search();
            Sort st = new Sort();
            IQueryableBookeModel bm = new IQueryableBookeModel();



            
            if (bookGenre != null)
            {
                /*
                while (gL.Contains(""))
                {
                    gL.Remove("");
                }

                if (gL.Contains(bookGenre))
                {

                    gL.Remove(removeGenre);
                }
                else
                {
                    gL.Add(bookGenre);
                }*/

                
                if (!gL.Contains(bookGenre) && GenreList.Contains(bookGenre))
                {
                    gL.Add(bookGenre);
                }

            }

            
            gL.Remove(removeGenre);
            removeGenre = "";

            var book = bm.newBookModel(db, gL);

            /*
             string fok = "";
             string bok = "";
             for(int i = 0; i < gL.Count();i++)
             {
                 fok += gL[i] + ", ";
             }

             for (int i = 0; i < book.First().ViewBagGenreList.Count(); i++)
             {
                 bok += book.First().ViewBagGenreList[i] + ", ";
             }


             System.Windows.Forms.MessageBox.Show("gL: " + fok + "\ngL length: " + gL.Count() + "\nViewBag: " + bok + "\nViewBag length: ");


            */







            if (authorID != null)
            {
                book = book.Where(a => a.AuthorModel.AuthorID == authorID);
            }


            ViewBag.AuthorID = authorID;
            ViewBag.CurrentSort = sortOrder;
            //  ViewBag.BookGenreFilter = gL;
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


            ViewBag.bookGenre = new SelectList(GenreList.Except(gL));
            ViewBag.GenreFilter = bookGenre;



            book = sh.search(db, searchString, authorID, book, gL);
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
                username = username,
                reviews = db.Reviews.Where(a => a.ISBN == id).Select(a => a.Rating).DefaultIfEmpty(0).Average(),
                ReviewModel = db.Reviews.Where(b => b.ISBN == id)
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
        /*commented out to test for deletion Isumy
        public ActionResult ShoppingCart()
        {
            return RedirectToAction("ShoppingCart", "Account");
            return View();
        }*/
        
    }
}