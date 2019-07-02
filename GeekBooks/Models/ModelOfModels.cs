using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace GeekBooks.Models
{
    public class ModelOfModels
    {


        public class BookeModel
        {
            public Book BookModel { get; set; }
            public BookGenre BookGenreModel { get; set; }


        }
    }

    public class BookeModel
    {
        public Book BookModel { get; set; }
        public BookGenre BookGenreModel { get; set; }
        public Review ReviewModel { get; set; }
        public Genre GenreModel { get; set; }
        public IPagedList PageListModel { get; set; }

    }
}