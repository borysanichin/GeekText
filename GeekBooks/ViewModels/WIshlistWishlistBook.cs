using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeekBooks.ViewModels
{
    public class WishlistWishlistBook
    {
        public string wishlistName { get; set; }
        public IEnumerable<WishlistBook> wishlistBooks { get; set; }
        public IEnumerable<Wishlist> Wishlists { get; set; }
        public WishlistBook WishlistBook { get; set; }
        public decimal totalPrice { get; set; }
    }
}