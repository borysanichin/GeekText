//using GeekBooks.Models;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;

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
            List<Wishlist> books = _context.Wishlists.Include(w => w.Book).Where(w => w.WishlistName == wishlistName && w.Username == username).ToList();

            if (books == null)
                return HttpNotFound();

            return View(books);
            
        }
    }
}