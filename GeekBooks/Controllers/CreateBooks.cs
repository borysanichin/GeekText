using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GeekBooks.Models;
using PagedList;

namespace GeekBooks.Controllers
{
    public class CreateBooksController : Controller
    {
        private BookContext db = new BookContext();

        // GET: Books
        public ActionResult Index(string sortOrder, string movieGenre, string searchString,
                                  string currentFilter, int? page)
        {

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
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

            var GenreList = new List<string>();

            // var GenreLst = from d in db.Genres
            //          select d.GenreName;

            var GenreQry = from d in db.Genres
                           orderby d.GenreName
                           select d.GenreName;

            GenreList.AddRange(GenreQry.Distinct());

            ViewBag.movieGenre = new SelectList(GenreList);

            /* var books = from m in db.Books
                         select new BookeModel { BookModel = m };*/

            var book = from m in db.Books
                           join n in db.BookGenres on m.ISBN equals n.ISBN
                           select new BookeModel { BookModel = m, BookGenreModel = n };

            if (!String.IsNullOrEmpty(searchString))
            {
                //books = books.Where(s => s.BookModel.Title.Contains(searchString));\
                book = book.Where(s => s.BookModel.Title.Contains(searchString));
            }
            /*
            if (!String.IsNullOrEmpty(movieGenre))
            {
                viewBook = viewBook.Where(x => x.BookGenreModel.GenreName == movieGenre);
            }*/

           

            switch (sortOrder)
            {
                case "name_desc":
                    book = book.OrderByDescending(s => s.BookModel.Title);
                    break;
                case "Date":
                    book = book.OrderBy(s => s.BookModel.DatePublished);
                    break;
                case "date_desc":
                    book = book.OrderByDescending(s => s.BookModel.DatePublished);
                    break;
                default:
                    book = book.OrderBy(s => s.BookModel.Title);
                    break;
            }


            int pageSize = 3;
            int pageNumber = (page ?? 1);

            return View(book.ToPagedList(pageNumber, pageSize));


            //return View(book);
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

        // GET: Books/Create
        public ActionResult Create()
        {
            ViewBag.GenreName = new SelectList(db.Genres, "GenreName", "GenreName");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookeModel bookM)
        {
            var book = bookM.BookModel;
            var bookGenre = bookM.BookGenreModel;


            if (ModelState.IsValid)
            {

                db.Books.Add(book);
                bookGenre.ISBN = book.ISBN;
                db.BookGenres.Add(bookGenre);
                db.SaveChanges();


                return RedirectToAction("Index");


            }
            var viewBook = from m in db.Books
                           join n in db.BookGenres on m.ISBN equals n.ISBN
                           select new BookeModel { BookModel = m, BookGenreModel = n };


            ViewBag.GenreName = new SelectList(db.Genres, "GenreName", "GenreName", db.Genres);
            return View(viewBook);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            var genre = from n in db.Genres
                        select n;
            if (book == null)
            {
                return HttpNotFound();
            }
            var viewBook = from m in db.Books
                           join n in db.BookGenres on m.ISBN equals n.ISBN
                           select new BookeModel { BookModel = m, BookGenreModel = n };
           // ViewBag.GenreName = new SelectList(db.Genres, "GenreName", "GenreName", genre);
            return View(viewBook);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ISBN,Title,Price, BookDescription,PublisherName,DatePublished")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var viewBook = from m in db.Books
                           join n in db.BookGenres on m.ISBN equals n.ISBN
                           select new BookeModel { BookModel = m, BookGenreModel = n };
            //  ViewBag.GenreName = new SelectList(db.Genres, "GenreName", "GenreName", book.GenreName);
            return View(viewBook);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(string id)
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

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
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
