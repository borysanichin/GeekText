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
                case "Price":
                    book = book.OrderBy(s => s.BookModel.Price);
                    break;
                case "price_desc":
                    book = book.OrderByDescending(s => s.BookModel.Price);
                    break;
                case "Author":
                    book = book.OrderBy(s => s.BookModel.PublisherName);
                    break;
                case "author_desc":
                    book = book.OrderByDescending(s => s.BookModel.PublisherName);
                    break;
                case "Rating":
                    book = book.OrderBy(s => s.reviews);
                    break;
                case "rating_desc":
                    book = book.OrderByDescending(s => s.reviews);
                    break;
                default:
                    book = book.OrderBy(s => s.BookModel.Title);
                    break;
            }

            return book;



        }
    }
}