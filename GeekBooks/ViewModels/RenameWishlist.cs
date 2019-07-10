using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeekBooks.ViewModels
{
    public class RenameWishlist
    {
        public Wishlist Wishlist { get; set; }
        public string oldName { get; set; }
        public string Username { get; set; }
    }
}