using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeekBooks.Models
{
    public class IQueryableBookeModel
    {

        public IQueryable<BookeModel> newBookModel(BookContext db){



            var book = from a in db.Books
                   join b in db.BookGenres on a.ISBN equals b.ISBN
                   join c in db.Wrotes on b.ISBN equals c.ISBN
                   join d in db.Authors on c.AuthorID equals d.AuthorID
                   select new BookeModel
                   {
                       BookModel = a,
                       BookGenreModel = b,
                       WroteModel = c,
                       AuthorModel = d,
                       reviews = db.Reviews.Where(b => b.ISBN == a.ISBN).Select(a => a.Rating).DefaultIfEmpty(0).Average()

                   };

            return book;

        }




    }
}