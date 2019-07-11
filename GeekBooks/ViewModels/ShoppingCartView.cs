using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeekBooks.ViewModels
{
    public class ShoppingCartView
    {

        public IEnumerable<ShoppingCart> shoppingCart { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
    }
}