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
            return View();
        }

        public ActionResult ShoppingCartPartial()
        {
            //Initialize ShoppingCart View Model

            ShoppingCart model = new ShoppingCart();

            //Initialize Quantity
            int quantity = 0;

            //Initilaize price

            decimal price = 0m;

            //Check for ShoppinCart session

            if (Session["cart"] != null)
            {
                //Get total quatity and price
                var ShoppingCartList = (List<ShoppingCart>)Session["cart"];

                foreach (var item in ShoppingCartList)
                {
                    quantity += item.Quantity;
                    price += item.Quantity * item.PriceEach;

                }
            }
            else
            {
                //or set quantity and price to zero
                model.Quantity = 0;
                model.PriceEach = 0m;


            }
            //Return Partial View with model
            return PartialView(model);
        }
    }
}