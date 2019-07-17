using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeekBooks.Controllers
{
    public class ShoppingCartController : Controller
    {
        private BookContext _context;

        public ShoppingCartController()
        {
            _context = new BookContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: ShoppingCart
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddBookToShoppingCart(ShoppingCart shoppingCart)
        {


            ShoppingCart sCart = _context.ShoppingCarts.Find(shoppingCart.Username, shoppingCart.ISBN);



            if (sCart == null)
            {

                _context.ShoppingCarts.Add(shoppingCart);
                _context.SaveChanges();
            }

            return RedirectToAction("DisplayShoppingCartDetail", "ShoppingCart", new { shoppingCart.Username });
        }
        // oh my god y just noticed it another thing is
        

        [Route("ShoppingCart/DisplayShoppingCartDetail/{username}")]

        public ActionResult DisplayShoppingCartDetail(string username)
        {
            IEnumerable<ShoppingCart> sCart = _context.ShoppingCarts.Where(w => w.Username == username).ToList();

            if (sCart == null)
                return HttpNotFound();

            return View(sCart);
        }
    }
}