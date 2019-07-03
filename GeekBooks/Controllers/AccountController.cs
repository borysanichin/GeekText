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

        [Route("Account/WishListDetail/{wishlistName}/{username}")]
        public ActionResult WishListDetail(string wishlistName, string username)
        {
          //List<WishlistBook> books = _context.WishlistBooks.Include(w => w.Book).Where(w => w.WishlistName == wishlistName && w.Username == username).ToList();
            var wwbook = new WishlistWishlistBook
            {
                wishlistName = wishlistName,
                wishlistBooks = _context.WishlistBooks.Include(w => w.Book).Where(w => w.WishlistName == wishlistName && w.Username == username).ToList()
            };
            
            if (wwbook == null)
                return HttpNotFound();

            return View(wwbook);

        }
        public ActionResult AddWishlist(string id)
        {
            //var user = _context.Users.SingleOrDefault(w => w.Username == id);

            /*var viewModel = new UserWishlist
            {
                User = user,
                Wishlists = _context.Wishlists.ToList()
        };  */
            return View();
        }
        public ActionResult SaveWishlist(Wishlist wishlist)
        {
            _context.Wishlists.Add(wishlist);
            _context.SaveChanges();

            return RedirectToAction("Wishlist", "Account");
        }
    }
}