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
            var wwbook = new WishlistWishlistBook
            {
                wishlistName = wishlistName,
                wishlistBooks = _context.WishlistBooks.Include(w => w.Book).Where(w => w.WishlistName == wishlistName && w.Username == username).ToList()
            };
            
            if (wwbook == null)
                return HttpNotFound();

            return View(wwbook);
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
    }
}