using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GeekBooks.Models
{
    [Table("Book")]
    public class Book
    {
        [Key]
        public string ISBN { get; set; }
        //public int BookId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string BookDescription { get; set; }
        public string PublisherName { get; set; }
        public System.DateTime DatePublished { get; set; }
        public List<Review> Reviews { get; set; }
    }
}