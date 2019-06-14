using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace GeekBooks.Models
{
    public class BookContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
    }
}