  using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        public Wrote WroteModel { get; set; }
        public Author AuthorModel { get; set; }
        public decimal reviews { get; set; }


    }
}