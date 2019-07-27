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

        public IQueryable<BookeModel> search(BookContext db, string searchString, int? authorID, IQueryable<BookeModel> book,
            List<string> gL)
        {




            // BookM.BookGenreModel = db.BookGenres.FirstOrDefault(a => a.ISBN == BookM.BookModel.ISBN);




            if (!String.IsNullOrEmpty(searchString))
            {
                book = book.Where(s => s.BookModel.Title.Contains(searchString));
            }

            /*
            if (!String.IsNullOrEmpty(bookGenre))
            {
                book = book.Where(x => x.BookGenreModel.GenreName == bookGenre);
            }
            */


            var compareList = new List<string>(gL);

            compareList.Remove("All");
            /*string fok = "";
            foreach (var item in compareList)
            {
                fok += item + " ";
            }
            System.Windows.Forms.MessageBox.Show("Compare List is: " + fok + "\nCount is" + compareList.Count());
            */

            if (compareList.Count() > 0)
            {

                foreach (var item in compareList)
                {
                    book = book.Where(a => a.genres.Contains(item));
                }
            }

            return book;


        }
        public List<string> genreList(BookContext db)
        {
            var GenreList = new List<string>();

            var GenreQry = from d in db.Genres
                           select d.GenreName;


            GenreList.AddRange(GenreQry.Distinct());
            return GenreList;
        }


    }
}