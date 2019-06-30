using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeekBooks.Views.ShoppingCart
{
    public class ShoppingCartVM
    {
        public string ISBM { get; set; }
        public string Title { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get { return Quantity * Price; } }
        public string Image { get; set; }
    }
}