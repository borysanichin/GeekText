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
            //Init the cart list
            var cart = Session["cart"] as List<ShoppingCart> ?? new List<ShoppingCart>();
            //Check if cart is empty
            if (cart.Count == 0 || Session["cart"] == null)
            {
                ViewBag.Message = "Your cart is empty.";
                return View();
            }

            //Calculate total and save to Viewbag
            decimal total = 0m;
            foreach (var item in cart)
            {
                total += item.Total;
            }

            ViewBag.GrandTotal = total;
            //Return the view with list
            return View(cart);
        }
        public ActionResult CartPartial()
        {
            //Initialize CartVM
            ShoppingCart model = new ShoppingCart();
            // Initilize qty
            decimal qty = 0;
            //Initialize price 
            decimal price = 0m;

            //Check for cart session
            if (Session["cart"] != null)
            {
                //get total qty and price
                var list = (List<ShoppingCart>)Session["cart"];

                foreach (var item in list)
                {
                    qty += item.Quantity;
                    price += item.Quantity * item.PriceEach;

                }
            }
            else
            {
                //Or set qty and price to 0
                model.Quantity = 0;
                model.PriceEach = 0m;
            }

            //Return partial view with model
            return PartialView(model);
        }
        public ActionResult AddToCartPartial(string isbn, string username) {

            //Init ShoppingCart View Model list

            List<ShoppingCart> cart = Session["cart"] as List<ShoppingCart> ?? new List<ShoppingCart>();

            //Initialize ShoppingCart view Model
            ShoppingCart model = new ShoppingCart();

            using (BookContext db = new BookContext())
            {
                //Get Book
                Book book = db.Books.Find(isbn);
                //Check if the book is already in the cart
                var bookInCart = cart.FirstOrDefault(x => x.ISBN == isbn );
                var userInCart = cart.FirstOrDefault(u => u.Username == username);
                //If not, Add new ShoppingCart View Model
                if (bookInCart == null)
                {
                    cart.Add(new ShoppingCart()
                    {
                        ISBN = book.ISBN,
                        Username = username,
                        PriceEach = book.Price,
                        Quantity = 1

                    });
                }
                else
                {
                    //If it is, increment
                    bookInCart.Quantity++;
                }
            }
            //Get Total qty and price and add to model
            short qty = 0;
            decimal price = 0m;

            foreach (var item in cart)
            {
                qty += item.Quantity;
                price += item.Quantity * item.PriceEach;
            }

            model.Quantity = qty;
            model.PriceEach = price;

            //Save cart back to session
            Session["cart"] = cart;

                return PartialView(model);
        }

        //all working good with the functions below
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