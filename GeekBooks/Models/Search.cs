using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using GeekBooks.Models;
using PagedList;

namespace GeekBooks.Models
{
    public class Search
    {

        public IQueryable<BookeModel> search(BookContext db, string searchString, string movieGenre, int? authorID, IQueryable<BookeModel> book)
        {




            // BookM.BookGenreModel = db.BookGenres.FirstOrDefault(a => a.ISBN == BookM.BookModel.ISBN);


          

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