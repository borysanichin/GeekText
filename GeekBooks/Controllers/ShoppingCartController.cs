using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeekBooks.Controllers
{
    public class  
        ShoppingCartController : Controller
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
        public ActionResult Index()//String username)
        {
          
            var carts = from sc in _context.ShoppingCarts
                        where sc.Username == "guest"
                        select sc;

            return View(carts);
        }
       

        //all working good with the functions below
        public ActionResult AddBookToShoppingCart(ShoppingCart shoppingCart)
        {
            //Check if user is logged in
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ShoppingCart sCart = _context.ShoppingCarts.Find(shoppingCart.Username, shoppingCart.ISBN);



            if (sCart == null)
            {

                _context.ShoppingCarts.Add(shoppingCart);
                _context.SaveChanges();
            }

            //return RedirectToAction("DisplayShoppingCartDetail", "ShoppingCart", new { shoppingCart.Username });
            return RedirectToAction("ShoppingCartDetail", "ShoppingCart", new { shoppingCart.Username });
        }
       
       

        //[Route("ShoppingCart/ShoppingCartDetail/{username}")]

        public ActionResult ShoppingCartDetail(string username)
        {
            IEnumerable<ShoppingCart> sCart = _context.ShoppingCarts.Where(w => w.Username == username ).ToList();

            if (sCart == null)
                return HttpNotFound();

            return View(sCart);
        }

        //[Route("ShoppingCart/DeleteShoppingCartBook/{username}/{isbn}")]
        public ActionResult DeleteShoppingCartBook(string username, string isbn)
        {
            var sCart = _context.ShoppingCarts.Find(username, isbn);

            if (sCart == null)
                return HttpNotFound();

            _context.ShoppingCarts.Remove(sCart);
            _context.SaveChanges();

            return RedirectToAction("ShoppingCartDetail", "ShoppingCart", new { username });
        }
        [Route("ShoppingCart/UpdateShoppingCartQuantity/{Username}/{Isbn}")]
        public ActionResult UpdateShoppingCartQuantity(string Username, string Isbn)
        {
            ShoppingCart sCart = _context.ShoppingCarts.Find(Username, Isbn);

            return View(sCart);
        }
        public ActionResult SaveShoppingCartQuantity(ShoppingCart id)
        {
            var oldShoppingCart = _context.ShoppingCarts.Find(id.Username, id.ISBN);

            _context.ShoppingCarts.Remove(oldShoppingCart);
            _context.ShoppingCarts.Add(id);
            _context.SaveChanges();

            return RedirectToAction("ShoppingCartDetail", "ShoppingCart", new { id.Username });
        }
        //Update save for later
        [Route("ShoppingCart/UpdateShoppingCartSaveForLater/{Username}/{Isbn}")]
        public ActionResult UpdateShoppingCartSaveForLater(string Username, string Isbn)
        {
            ShoppingCart sCart = _context.ShoppingCarts.Find(Username, Isbn);
            //sCart.SaveForLater = true;
            return View(sCart);
        }
 
        public ActionResult SaveForLater(ShoppingCart id)
        {
            var oldShoppingCart = _context.ShoppingCarts.Find(id.Username, id.ISBN);

            _context.ShoppingCarts.Remove(oldShoppingCart);
            id.SaveForLater = true;
            _context.ShoppingCarts.Add(id);
            _context.SaveChanges();

            return RedirectToAction("ShoppingCartDetail", "ShoppingCart", new { id.Username });
        }

        public ActionResult BackToShoppingCart(ShoppingCart id)
        {
            var oldShoppingCart = _context.ShoppingCarts.Find(id.Username, id.ISBN);

            _context.ShoppingCarts.Remove(oldShoppingCart);
            id.SaveForLater = false;
            _context.ShoppingCarts.Add(id);
            _context.SaveChanges();

            return RedirectToAction("ShoppingCartDetail", "ShoppingCart", new { id.Username });
        }
    }
}