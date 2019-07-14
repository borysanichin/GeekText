using GeekBooks.Models;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using GeekBooks.ViewModels;

namespace GeekBooks.Controllers
{
    public class AccountController : Controller
    {
        private BookContext _context;

        public AccountController()
        {
            _context = new BookContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult WishList()
        {
            List<Wishlist> wishlists = _context.Wishlists.Where(w => w.Username == "guest").ToList();
            return View(wishlists);
        }

   



        [Route("Account/AddWishlist/{Username}")]
        public ActionResult AddWishlist(string Username)
        {
            return View();
        }

        public ActionResult RenameWishlist(Wishlist id)
        {

            return View(id);
        }

        public ActionResult SaveWishlist(Wishlist id)
        {
            Wishlist wishlist = _context.Wishlists.Find(id.Username, id.WishlistName);

            if (wishlist == null)
            {
                _context.Wishlists.Add(id);
                _context.SaveChanges();
            }

            return RedirectToAction("Wishlist", "Account");
        }

        [Route("Account/SaveRenameWishList/{oldName}/{wishlist}")]
        public ActionResult SaveRenameWishlist(string oldName, Wishlist wishlist)
        {
            Wishlist newWishlist = _context.Wishlists.Find(wishlist.Username, wishlist.WishlistName);

            if (newWishlist == null)
            {
                var wishlistInDB = _context.Wishlists.Find(wishlist.Username, oldName);
                wishlistInDB.WishlistName = wishlist.WishlistName;
                _context.SaveChanges();
            }

            return RedirectToAction("Wishlist", "Account");
        }

        [Route("Account/DeleteWishList/{WishlistName}/{Username}")]
        public ActionResult DeleteWishlist(string WishlistName, string Username)
        {
            foreach (var wbook in _context.WishlistBooks)
            {
                if (wbook.Username == Username && wbook.WishlistName == WishlistName)
                {
                    _context.WishlistBooks.Remove(wbook);
                }
            }

            Wishlist wishlist = _context.Wishlists.Find(Username, WishlistName);

            if (wishlist == null)
                return HttpNotFound();

            _context.Wishlists.Remove(wishlist);
            _context.SaveChanges();

            return RedirectToAction("Wishlist", "Account");
        }

        //[Route("Account/AddBookToWishlist/{wishlistBook}")]
        public ActionResult AddBookToWishlist(WishlistBook wishlistBook)
        {
            WishlistBook wbook = _context.WishlistBooks.Find(wishlistBook.Username, wishlistBook.ISBN, wishlistBook.WishlistName);

            if (wbook == null)
            {
                _context.WishlistBooks.Add(wishlistBook);
                _context.SaveChanges();
            }

            return RedirectToAction("WishListDetail", "Account", new { wishlistBook.WishlistName, wishlistBook.Username });
        }

        [Route("Account/WishListDetail/{wishlistName}/{username}")]
        public ActionResult WishListDetail(string wishlistName, string username)
        {
            IEnumerable<Wishlist> wlists = _context.Wishlists.Where(w => w.Username == username).ToList();

            var wwbook = new WishlistWishlistBook
            {
                wishlistName = wishlistName,
                wishlistBooks = _context.WishlistBooks.Include(w => w.Book).Where(w => w.WishlistName == wishlistName && w.Username == username).ToList(),
                Wishlists = wlists.Where(w => w.WishlistName != wishlistName).ToList()
            };

            if (wwbook == null)
                return HttpNotFound();

            return View(wwbook);
        }


        public ActionResult AddBookToShoppingCart(ShoppingCart shoppingCart)
        {


            ShoppingCart sCart = _context.ShoppingCarts.Find(shoppingCart.Username, shoppingCart.ISBN);

          
            
            if (sCart == null)
            {
                 
                _context.ShoppingCarts.Add(shoppingCart);
                _context.SaveChanges();
            }

            return RedirectToAction("ShoppingCartDetail", "Account", new { shoppingCart.Username });
        }

        [Route("Account/ShoppingCartDetail/{username}")]
        
        public ActionResult ShoppingCartDetail(string username)
        {
            IEnumerable<ShoppingCart> sCart = _context.ShoppingCarts.Where(w => w.Username == username).ToList();
 
            if (sCart == null)
                return HttpNotFound();

            return View(sCart);
        }




        [Route("Account/DeleteWishlistBook/{username}/{isbn}/{wishlistname}")]
        public ActionResult DeleteWishlistBook(string username, string isbn, string wishlistname)
        {
            var wbook = _context.WishlistBooks.Find(username, isbn, wishlistname);

            if (wbook == null)
                return HttpNotFound();

            _context.WishlistBooks.Remove(wbook);
            _context.SaveChanges();

            return RedirectToAction("WishlistDetail", "Account", new { wishlistname, username});
        }

        [Route("Account/DeleteWishlistBook/{username}/{isbn}")]
        public ActionResult DeleteShoppingCartBook(string username, string isbn)
        {
            var sCart = _context.ShoppingCarts.Find(username, isbn);

            if (sCart == null)
                return HttpNotFound();

            _context.ShoppingCarts.Remove(sCart);
            _context.SaveChanges();

            return RedirectToAction("ShoppingCartDetail", "Account", new {username });
        }
        //[Route("Account/MoveWishlistBook/{wishlistname}/{wishlistbook}")]
        public ActionResult MoveWishlistBook(string wishlistname, WishlistBook wishlistbook)
        {
            if (_context.WishlistBooks.Find(wishlistbook.Username, wishlistbook.ISBN, wishlistbook.WishlistName) == null)
            {
                var wishlistOldBook = _context.WishlistBooks.Find(wishlistbook.Username, wishlistbook.ISBN, wishlistname);

                _context.WishlistBooks.Remove(wishlistOldBook);
                _context.WishlistBooks.Add(wishlistbook);
                _context.SaveChanges();
            }

            return RedirectToAction("WishlistDetail", "Account", new { wishlistname, wishlistbook.Username });
        }

        [Route("Account/UpdateWishlistQuantity/{Username}/{Isbn}/{Wishlistname}")]
        public ActionResult UpdateWishlistQuantity(string Username, string Isbn, string Wishlistname)
        {
            WishlistBook wbook = _context.WishlistBooks.Find(Username, Isbn, Wishlistname);

            return View(wbook);
        }

        [Route("Account/UpdateShoppingCartQuantity/{Username}/{Isbn}")]
        public ActionResult UpdateShoppingCartQuantity(string Username, string Isbn)
        {
            ShoppingCart sCart = _context.ShoppingCarts.Find(Username, Isbn);

            return View(sCart);
        }

        
        public ActionResult SaveWishlistQuantity(WishlistBook id)
        {
            var wishlistOldBook = _context.WishlistBooks.Find(id.Username, id.ISBN, id.WishlistName);

            _context.WishlistBooks.Remove(wishlistOldBook);
            _context.WishlistBooks.Add(id);
            _context.SaveChanges();

            return RedirectToAction("WishlistDetail", "Account", new { id.WishlistName, id.Username });
        }

        public ActionResult SaveShoppingCartQuantity(ShoppingCart id)
        {
            var oldShoppingCart = _context.ShoppingCarts.Find(id.Username, id.ISBN);

            _context.ShoppingCarts.Remove(oldShoppingCart);
            _context.ShoppingCarts.Add(id);
            _context.SaveChanges();

            return RedirectToAction("ShoppingCartDetail", "Account", new { id.Username });
        }
    }
}