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
        public IEnumerable<Review> ReviewModel { get; set; }
        public List<Genre> GenreModel { get; set; }
        public Wrote WroteModel { get; set; }
        public Author AuthorModel { get; set; }
        public List<string> genres { get; set; }
        public List<string> ViewBagGenreList { get; set; }
        public List<string> viewGenres { get; set; }
        public List<string> GenreForView { get; set; }
        public decimal reviews { get; set; }
        public string username { get; set; }
        public IEnumerable<Wishlist> Wishlists { get; set; }
        public WishlistBook WishlistBook { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public Nullable<int> quantity { get; set; }

    }
}