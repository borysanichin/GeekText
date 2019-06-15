using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace GeekBooks.Models
{
    [Table("Reviews")]
    public class Review
    {
        [Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ISBN { get; set; }
        [Key, Column(Order = 1)]
        public string Username { get; set; }
        public decimal rating { get; set; }
        public string comment { get; set; } 
    }
}