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
        public ActionResult AddBookToShoppingCart(ShoppingCart shoppingCart, string controller, string action)

        {
            //Check if user is logged in
            if (Session["Username"] == null)
            {
                RedirectToAction("Login", "Account");
            }

            ShoppingCart sCart = _context.ShoppingCarts.Find(shoppingCart.Username, shoppingCart.ISBN);



            if (sCart == null)
            {

                _context.ShoppingCarts.Add(shoppingCart);
                _context.SaveChanges();
            }

            //return new EmptyResult();
            //return RedirectToAction("DisplayShoppingCartDetail", "ShoppingCart", new { shoppingCart.Username });
            if (controller == null || action == null)
            {
                return RedirectToAction("DisplayShoppingCartDetail", "ShoppingCart", new { shoppingCart.Username });
            }
            if (action == "Details")
            {
                return RedirectToAction(action, controller, new { username = Session["Username"].ToString(), id = shoppingCart.ISBN });
            }

            return RedirectToAction(action, controller);
            // return RedirectToAction("ShoppingCartDetail", "ShoppingCart", new { shoppingCart.Username });
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

        public ActionResult PurchaseItems(ShoppingCart cart)
        {


            if (Session["Username"] == null)
            {

                RedirectToAction("Login", "Account");

            }


            List<ShoppingCart> ShoppingCartList = _context.ShoppingCarts.Where(a => a.Username == cart.Username).ToList();


            foreach (var book in ShoppingCartList)
            {

                Purchased newPurchase = new Purchased()
                {
                    Username = Session["Username"].ToString(),
                    ISBN = book.ISBN,
                    qty = 1
                };

                var oldPurchase = _context.Purchaseds.Find(Session["Username"].ToString(), book.ISBN);
                if (oldPurchase != null)
                {
                    _context.Purchaseds.Remove(oldPurchase);
                    (newPurchase.qty) += book.Quantity;
                    _context.Purchaseds.Add(newPurchase);
                }
                else
                {
                    _context.Purchaseds.Add(newPurchase);
                }
                _context.SaveChanges();
                _context.ShoppingCarts.Remove(book);
                _context.SaveChanges();


            }

            return RedirectToAction("ShoppingCartDetail", "ShoppingCart", new { cart.Username });

        }
    }
}