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

        [Route("Account/AddWishlist/{Username}")]
        public ActionResult AddWishlist(string Username)
        {
            return View();
        }
        /*[Route("Account/RenameWishlist/{WishlistName}/{Username}")]
        public ActionResult RenameWishlist(string WishlistName, string Username)
        {
            Wishlist wl = _context.Wishlists.Find(Username, WishlistName);
            var viewModel = new RenameWishlist
            {
                oldName = WishlistName,
                Username = Username,
                Wishlist = wl
            };

            return View(viewModel);
        }*/

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

        //[Route("Account/SaveRenameWishlist/{oldName}/{wishlist}")]
        /*public ActionResult SaveRenameWishlist(string oldName, Wishlist wishlist)
        {
            //if (wishlist.Username == null)
                return Content(wishlist.WishlistName);

            Wishlist nw = _context.Wishlists.Find(newWishlist.Username, newWishlist.WishlistName);

            if (nw == null)
            {
                _context.Wishlists.Add(newWishlist);
                _context.SaveChanges();

                foreach (var wb in _context.WishlistBooks.Where(w => w.Username == newWishlist.Username))
                {
                    if (wb.WishlistName == oldName)
                    {
                        wb.WishlistName = newWishlist.WishlistName;
                    }
                }
                var wishlistInDB = _context.Wishlists.Find(newWishlist.Username, oldName);
                _context.Wishlists.Remove(wishlistInDB);
                _context.SaveChanges();
            }

            //return RedirectToAction("Wishlist", "Account");
        }*/

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
        public ActionResult SaveWishlistQuantity(WishlistBook id)
        {
            var wishlistOldBook = _context.WishlistBooks.Find(id.Username, id.ISBN, id.WishlistName);

            _context.WishlistBooks.Remove(wishlistOldBook);
            _context.WishlistBooks.Add(id);
            _context.SaveChanges();

            return RedirectToAction("WishlistDetail", "Account", new { id.WishlistName, id.Username });
        }
        [Route("Account/MakePrimary/{WishlistName}/{Username}")]
        public ActionResult MakePrimary(string WishlistName, string Username)
        {
            var wishlistInDb = _context.Wishlists.Find(Username, WishlistName);

            if (wishlistInDb == null)
                return HttpNotFound();

            foreach (var wlist in _context.Wishlists.Where(w => w.Username == Username))
            {
                if (wlist.Preferred)
                    wlist.Preferred = false;
            }

            wishlistInDb.Preferred = true;
            _context.SaveChanges();

            return RedirectToAction("Wishlist", "Account");
        }
    }
}