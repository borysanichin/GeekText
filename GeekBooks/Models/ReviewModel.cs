using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GeekBooks.Models
{
    public class ReviewModel
    {
        public string ISBN { get; set; }
        public string Username { get; set; }
        public decimal Rating { get; set; }
        public string Comment { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public System.DateTime DatePosted { get; set; }
        public bool BoolValue { get { return Anonymous == 1; } set { Anonymous = value ? 1 : 0; } }
        public int Anonymous { get; set; }

        public virtual Book Book { get; set; }
        public virtual User User { get; set; }
    }
}