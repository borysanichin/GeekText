//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GeekBooks.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ShippingAddress
    {
        public string Username { get; set; }
        public decimal AddressNum { get; set; }
        public bool PreferredAddress { get; set; }
        public string Street { get; set; }
        public Nullable<decimal> Apt { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
    
        public virtual User User { get; set; }
    }
}
