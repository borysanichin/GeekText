﻿using GeekBooks.Models;
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
        BookContext objBookContext = new BookContext();

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
            LoginModel objLoginModel = new LoginModel();
            return View(objLoginModel);
        }



        [HttpPost]
        public ActionResult Login(LoginModel objLoginModel)
        {
            Password encPassword = new Password();

           
            if (ModelState.IsValid)
            {
                string entryPassword = encPassword.Encode(objLoginModel.UserPassword);
                if (objBookContext.Users.Where(m => m.Username == objLoginModel.Username && m.UserPassword ==  entryPassword).FirstOrDefault() == null)
                {
                    ModelState.AddModelError("Error", "The password you entered is incorrect. Please try again.");
                    return View();
                }
                else
                {
                    Session["Username"] = objLoginModel.Username;
                   
                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
           
        }


        public ActionResult Address()
        {
            string usernames = Convert.ToString(Session["Username"]);

            var variable = objBookContext.ShippingAddresses.Where(m => m.Username == usernames).ToList();


            return View(variable);
        }

        //display details of single address
        public ActionResult DetailsAddress(string username, int id)
        {
            return View(objBookContext.ShippingAddresses.Find(username, id));
        }


        //adding a new address
        public ActionResult CreateAddress()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateAddress(ShippingAddress newaddress)
        {
            using (objBookContext)
            {
               
                objBookContext.ShippingAddresses.Add(newaddress);
                objBookContext.SaveChanges();
            }
            return RedirectToAction("Address");
        }
        //Edit an address
        public ActionResult EditAddress(string username, int id)
        {
            return View(objBookContext.ShippingAddresses.Find(username, id));
        }


        [HttpPost]
        public ActionResult EditAddress(ShippingAddress address)
        {
            objBookContext.Entry(address).State = EntityState.Modified;
            objBookContext.SaveChanges();
            return RedirectToAction("Address");
        }


        //delete an address

        public ActionResult DeleteAddress(string username, int id)
        {
            return View(objBookContext.ShippingAddresses.Find(username, id));

        }

        [HttpPost, ActionName("DeleteAddress")]
        public ActionResult Delete_confirm(string username, int id)
        {
            ShippingAddress c = objBookContext.ShippingAddresses.Find(username, id);
            objBookContext.ShippingAddresses.Remove(c);
            objBookContext.SaveChanges();
            return RedirectToAction("Address");
        }


        //display list of credit cards of the user

        public ActionResult CreditCards()
        {
            string usernames = Convert.ToString(Session["Username"]);

            var variable = objBookContext.CreditCards.Where(m => m.Username == usernames).ToList();


            return View(variable);
        }

        //display details of single credit card
        public ActionResult Details(string ccn, string username)
        {
            return View(objBookContext.CreditCards.Find(ccn, username));
        }


        //adding a new credit card
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(CreditCard newcard)
        {
            using(objBookContext)
            {
                //objBookContext.CreditCards.
                objBookContext.CreditCards.Add(newcard);
                objBookContext.SaveChanges();
            }
            return RedirectToAction("CreditCards");
        }
        //Credit Card edit
        public ActionResult Edit(string ccn, string username)
        {
            return View(objBookContext.CreditCards.Find(ccn, username));
        }


        [HttpPost]
        public ActionResult Edit(CreditCard card)
        {
            objBookContext.Entry(card).State = EntityState.Modified;
            objBookContext.SaveChanges();
            return RedirectToAction("CreditCards");
        }


        //delete a credit card

        public ActionResult Delete(string ccn, string username)
        {
            return View(objBookContext.CreditCards.Find(ccn, username));

        }

        [HttpPost, ActionName("Delete")]
        public ActionResult Delete_confirm(string ccn, string username)
        {
            CreditCard c = objBookContext.CreditCards.Find(ccn, username);
            objBookContext.CreditCards.Remove(c);
            objBookContext.SaveChanges();
            return RedirectToAction("CreditCards");
        }
        public ActionResult Profile()
        {

            
            string usernames = Convert.ToString(Session["Username"]);
            if (usernames == "")
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                return View(objBookContext.Users.Find(usernames));
            }
            

        }
        public ActionResult EditProfile(string username)
        {
            
            return View(objBookContext.Users.Find(username));
        }

        [HttpPost]
        public ActionResult EditProfile(User user)
        {
            objBookContext.Entry(user).State = EntityState.Modified;
            objBookContext.SaveChanges();
            return RedirectToAction("Profile");
        }

        public ActionResult Register()
        {
            Usermodel objUserModel = new Usermodel();
            return View(objUserModel);
        }

        [HttpPost]
        public ActionResult Register(Usermodel objUserModel)
        {
            Password encPassword = new Password();
            if (ModelState.IsValid)
            {

                if (!objBookContext.Users.Any(m => m.Email == objUserModel.Email || m.Username == objUserModel.Username))
                {
                    //object created from DBModel/Model1.tt/User
                    User objUser = new User();


                    objUser.Username = objUserModel.Username;
                    objUser.UserFirst = objUserModel.UserFirst;
                    objUser.UserMiddle = objUserModel.UserMiddle;
                    objUser.UserLast = objUserModel.UserLast;
                    objUser.Email = objUserModel.Email;
                    objUser.UserPassword = encPassword.Encode(objUserModel.UserPassword);


                    objBookContext.Users.Add(objUser);
                    objBookContext.SaveChanges();
                    objUserModel.Success = "User is succesfully Added";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("Error", "Account already exists.");
                    return View();
                }
            }
            return View();
        }


        
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult WishList(String id)
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            string username = id;
            List<Wishlist> wishlists = _context.Wishlists.Where(w => w.Username == username).ToList();
            return View(wishlists);
        }

        [Route("Account/AddWishlist/{Username}")]
        public ActionResult AddWishlist(string Username)
        {
            return View();
        }

        public ActionResult SaveWishlist(Wishlist id)
        {
            //validation of a new wishlist name
            if (id.WishlistName == null || id.WishlistName.Length > 25)
            {
                var User = (string)Session["Username"];
                return RedirectToAction("AddWishlist", "Account", new { Username = User });
            }
            Wishlist wishlist = _context.Wishlists.Find(id.Username, id.WishlistName);

            if (wishlist == null)
            {
                _context.Wishlists.Add(id);

                if (id.Preferred == true)
                {
                    foreach (var wlist in _context.Wishlists.Where(w => w.Username == id.Username))
                    {
                        if (wlist.Preferred && wlist.WishlistName != id.WishlistName)
                            wlist.Preferred = false;
                    }
                }
                _context.SaveChanges();
            }

            return RedirectToAction("Wishlist", "Account", new { id = Session["Username"] });
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

            return RedirectToAction("Wishlist", "Account", new { id = Session["Username"] });
        }


        public ActionResult AddBookToWishlist(WishlistBook wishlistBook)
        {
            string user = (string)Session["Username"];

            // redirects to login if not sign in
            if (user == null)
            {
               return RedirectToAction("Login", "Account");
            }

            //validates quantity and sets to 1 if negative
            if (wishlistBook.Quantity < 1)
                wishlistBook.Quantity = 1;

            //if wishlist name was not selected
            if (wishlistBook.WishlistName == null)
            {
                List<Wishlist> wishlists = _context.Wishlists.Where(w => w.Username == user).ToList();

                // chooses preferred wishlist
                var preferredWishList = wishlists.Where(w => w.Preferred).FirstOrDefault();

                // chooses any wishlist
                var anyWishList = wishlists.FirstOrDefault();

                //checks if preferred wishlist exists
                if (preferredWishList != null)
                {
                    WishlistBook wbook = _context.WishlistBooks.Find(preferredWishList.Username, wishlistBook.ISBN, preferredWishList.WishlistName);

                    if (wbook == null)
                    {
                        wishlistBook.WishlistName = preferredWishList.WishlistName;

                        // adds to preferred wishlist
                        _context.WishlistBooks.Add(wishlistBook);
                        _context.SaveChanges();
                    }
                    return RedirectToAction("WishListDetail", "Account", new { preferredWishList.WishlistName, preferredWishList.Username });
                }

                // if preferred wishlist does not exist checks for any wishlist
                else if (anyWishList != null)
                {
                    WishlistBook wbook = _context.WishlistBooks.Find(anyWishList.Username, wishlistBook.ISBN, anyWishList.WishlistName);

                    if (wbook == null)
                    {
                        wishlistBook.WishlistName = anyWishList.WishlistName;

                        // adds to any wishlist
                        _context.WishlistBooks.Add(wishlistBook);
                        _context.SaveChanges();
                    }
                
                return RedirectToAction("WishListDetail", "Account", new { anyWishList.WishlistName, anyWishList.Username });
                }

                // if user does not have any wishlists, redirects back to book details
                else
                {
                    string isbn = wishlistBook.ISBN;
                    return RedirectToAction("Details", "Book", new { id = isbn, username = user });
                }
               
            }
            //if user selects a wishlist
            else
            {
                WishlistBook wbook = _context.WishlistBooks.Find(wishlistBook.Username, wishlistBook.ISBN, wishlistBook.WishlistName);

                if (wbook == null)
                {
                    // adds to the selected wishlist
                    _context.WishlistBooks.Add(wishlistBook);
                    _context.SaveChanges();
                }

                return RedirectToAction("WishListDetail", "Account", new { wishlistBook.WishlistName, wishlistBook.Username });
            }
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

        public ActionResult MoveWishlistBook(string wishlistname, WishlistBook wishlistbook)
        {
            if (wishlistbook.WishlistName != null)
            {
                if (_context.WishlistBooks.Find(wishlistbook.Username, wishlistbook.ISBN, wishlistbook.WishlistName) == null)
                {
                    var wishlistOldBook = _context.WishlistBooks.Find(wishlistbook.Username, wishlistbook.ISBN, wishlistname);

                    _context.WishlistBooks.Remove(wishlistOldBook);
                    _context.WishlistBooks.Add(wishlistbook);
                    _context.SaveChanges();
                }
            }

            return RedirectToAction("WishlistDetail", "Account", new { wishlistname, wishlistbook.Username });
            
        }
        [Route("Account/MoveWishlistBookToCart/{Username}/{ISBN}/{WishlistName}")]
        public ActionResult MoveWishlistBookToCart(string Username, string ISBN, string WishlistName)
        {

            if (_context.ShoppingCarts.Find(Username, ISBN) == null)
            {
                var wb = _context.WishlistBooks.Find(Username, ISBN, WishlistName);

                _context.WishlistBooks.Remove(wb);

                Book book = _context.Books.Find(ISBN);

                ShoppingCart shoppingCart = new ShoppingCart
                {
                    Username = wb.Username,
                    ISBN = wb.ISBN,
                    PriceEach = book.Price,
                    Quantity = wb.Quantity
                };

                _context.ShoppingCarts.Add(shoppingCart);

                _context.SaveChanges();

                return RedirectToAction("ShoppingCartDetail", "ShoppingCart", new { Username });
            }
            else
            {
                var wb = _context.WishlistBooks.Find(Username, ISBN, WishlistName);

                _context.WishlistBooks.Remove(wb);
                _context.SaveChanges();

                return RedirectToAction("ShoppingCartDetail", "ShoppingCart", new { Username });
            }
                
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

            return RedirectToAction("Wishlist", "Account", new { id = Session["Username"] });
        }

        [Route("Account/UpdateWishlistQuantity/{Username}/{Isbn}/{Wishlistname}")]
        public ActionResult UpdateWishlistQuantity(string Username, string Isbn, string Wishlistname)
        {
            WishlistBook wbook = _context.WishlistBooks.Find(Username, Isbn, Wishlistname);

            return View(wbook);
        }

        
        
        public ActionResult SaveWishlistQuantity(WishlistBook id)
        {
            // validates quantity and makes it 1 if negative
            if (id.Quantity < 1)
                id.Quantity = 1;

            var wishlistOldBook = _context.WishlistBooks.Find(id.Username, id.ISBN, id.WishlistName);

            _context.WishlistBooks.Remove(wishlistOldBook);
            _context.WishlistBooks.Add(id);
            _context.SaveChanges();

            return RedirectToAction("WishlistDetail", "Account", new { id.WishlistName, id.Username });
        }

       
        
    }
}