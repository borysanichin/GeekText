using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeekBooks.Models
{
    public class Sort
    {


        public IQueryable<BookeModel> sort(IQueryable<BookeModel> book, string sortOrder, int? authorID)
        {

            if (authorID != null)
            {
                book = book.Where(a => a.AuthorModel.AuthorID == authorID);
            }

            switch (sortOrder)
            {
                case "title_desc":
                    book = book.OrderByDescending(s => s.BookModel.Title);
                    break;
                case "Date":
                    // book = book.OrderBy(s => s.BookModel.DatePublished);
                    book = book.OrderBy(s => s.BookModel.DatePublished == null).ThenBy(a => a.BookModel.DatePublished);
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
                    book = book.OrderBy(s => s.AuthorModel.AuthorFirst);
                    break;
                case "author_desc":
                    book = book.OrderByDescending(s => s.AuthorModel.AuthorFirst);
                    break;
                case "Rating":
                    book = book.OrderBy(s => s.reviews == 0).ThenBy(a => a.reviews);
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