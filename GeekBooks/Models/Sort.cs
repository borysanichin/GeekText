using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeekBooks.Models
{
    public class Sort
    {

        
        public IQueryable<BookeModel> sort(IQueryable<BookeModel> book, string sortOrder)
        {
            
            switch (sortOrder)
            {
                case "name_desc":
                    book = book.OrderByDescending(s => s.BookModel.Title);
                    break;
                case "Date":
                    book = book.OrderBy(s => s.BookModel.DatePublished);
                    break;
                case "date_desc":
                    book = book.OrderByDescending(s => s.BookModel.DatePublished);
                    break;
                default:
                    book = book.OrderBy(s => s.BookModel.Title);
                    break;
            }

            return book;



        }
    }
}