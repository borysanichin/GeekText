using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeekBooks.ViewModels
{
    public class BookDetailsViewModel
    {
        public Book Book { get; set; }
        public WishlistBook WishlistBook { get; set; }
        public BookGenre BookGenre { get; set; }
        public Review Review { get; set; }
        public Wrote Wrote { get; set; }
        public Author Author { get; set; }
        public IEnumerable<Wishlist> Wishlists { get; set; }

    }
}