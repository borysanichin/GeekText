//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GeekBooks
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Wishlist
    {
        public string Username { get; set; }

        [Display(Name = "Wishlist Name")]
        public string WishlistName { get; set; }
        public bool Preferred { get; set; }
    
        public virtual User User { get; set; }
    }
}
