using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using GeekBooks.Models;
using PagedList;
using Brick.PagedList;

namespace GeekBooks.Models
{
    public class Search
    {

        public IQueryable<BookeModel> search(BookContext db, string searchString, string movieGenre)
        {


            var Review = from m in db.Books
                         join r in db.Reviews on m.ISBN equals r.ISBN
                         select new
                         {
                             ISBN = m.ISBN,
                             Rating = r.Rating
                         };

            var aReview = Review.GroupBy(g => g.ISBN, r => r.Rating).Select(g => new
            {
                ISBN = g.Key,
                Rating = g.Average()
            });


            /*
                        aReview = (
                            (from r in aReview
                                   select r)
                                   .Union(from b in Review
                                    select b)).Distinct()
                                    ;
                                    */




            var book = from m in db.Books
                       join n in db.BookGenres on m.ISBN equals n.ISBN
                       join b in db.Wrotes on m.ISBN equals b.ISBN
                       join c in db.Authors on b.AuthorID equals c.AuthorID
                       //  join r in db.Reviews on m.ISBN equals r.ISBN
                       // join p in aReview on m.ISBN equals p.ISBN
                       select new BookeModel
                       {
                           BookModel = m,
                           BookGenreModel = n,
                           WroteModel = b,
                           AuthorModel = c,
                           // ReviewModel = aReview.FirstOrDefault(n => n.ISBN == m.ISBN),
                           reviews = db.Reviews.Where(b => b.ISBN == m.ISBN).Select(a => a.Rating).DefaultIfEmpty(0).Average()
                           /*ReviewModel = r,*/
                           /* reviews = p.Rating */

                       };






            if (!String.IsNullOrEmpty(searchString))
            {
                book = book.Where(s => s.BookModel.Title.Contains(searchString));
            }


            if (!String.IsNullOrEmpty(movieGenre))
            {
                book = book.Where(x => x.BookGenreModel.GenreName == movieGenre);
            }
            return book;
        }
        public List<string> genreList(BookContext db)
        {
            var GenreList = new List<string>();

            var GenreQry = from d in db.Genres
                           orderby d.GenreName
                           select d.GenreName;


            GenreList.AddRange(GenreQry.Distinct());
            return GenreList;
        }


    }
}