using GeekBooks.Views.ShoppingCart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeekBooks.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        public ActionResult Index()
        {
            //Initialize the cart list
            var shoppingcart = Session["ShoppingCart"] as List<ShoppingCartVM> ?? new List<ShoppingCartVM>();
            //Check if Cart is empty
            if (shoppingcart.Count == 0 || Session["ShoppingCart"] == null)
            {
                ViewBag.Message = "Your shopping cart is empty.";
                return View();
            }

            //Calculate total and save to ViewBag
            decimal total = 0m;

            foreach (var item in shoppingcart)
            {
                total += item.Total;
            }

            ViewBag.GrandTotal = total;

            //Return view list
            return View(shoppingcart);
        }
        public ActionResult CartPartial()
        {
            //Initialize CartVM
            ShoppingCartVM model = new ShoppingCartVM();
            // Initilize qty
            decimal qty = 0;
            //Initialize price 
            decimal price = 0m;

            //Check for cart session
            if (Session["cart"] != null)
            {
                //get total qty and price
                var list = (List<ShoppingCartVM>)Session["cart"];

                foreach (var item in list)
                {
                    qty += item.Quantity;
                    price += item.Quantity * item.Price;

                }
            }
            else
            {
                //Or set qty and price to 0
                model.Quantity = 0;
                model.Price = 0m;
            }

            //Return partial view with model
            return PartialView(model);
        }
    }
}