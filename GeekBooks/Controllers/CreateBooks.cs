using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GeekBooks.Models;

namespace GeekBooks.Controllers
{
    public class CreateBooksController : Controller
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

            // GenreList.AddRange(GenreQry.Distinct());

            ViewBag.movieGenre = new SelectList(GenreList);

            /* var books = from m in db.Books
                         select new BookeModel { BookModel = m };*/

            var viewBook = from m in db.Books
                           join n in db.BookGenres on m.ISBN equals n.ISBN
                           select new BookeModel { BookModel = m, BookGenreModel = n };

            if (!String.IsNullOrEmpty(searchString))
            {
                //books = books.Where(s => s.BookModel.Title.Contains(searchString));\
                viewBook = viewBook.Where(s => s.BookModel.Title.Contains(searchString));
            }
            /*
            if (!String.IsNullOrEmpty(movieGenre))
            {
                books = books.Where(x => x.GenreName == movieGenre);
            }*/

            return View(viewBook);
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


            // ViewBag.GenreName = new SelectList(db.Genres, "GenreName", "GenreName", book.GenreName);
            return View(viewBook);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
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
            var viewBook = from m in db.Books
                           join n in db.BookGenres on m.ISBN equals n.ISBN
                           select new BookeModel { BookModel = m, BookGenreModel = n };
            // ViewBag.GenreName = new SelectList(db.Genres, "GenreName", "GenreName", book.GenreName);
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
