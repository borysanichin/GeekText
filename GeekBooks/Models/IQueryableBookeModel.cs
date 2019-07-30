using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;

namespace GeekBooks.Models
{
    public class IQueryableBookeModel
    {

        public IQueryable<BookeModel> newBookModel(BookContext db, List<string> gL, List<string> ISBNL, Nullable<int> TopSeller,
            Nullable<int>rate)
        {



            




            var book = from a in db.Books
                       join b in db.BookGenres on a.ISBN equals b.ISBN
                       join c in db.Wrotes on b.ISBN equals c.ISBN
                       join d in db.Authors on c.AuthorID equals d.AuthorID
                       
                      // join e in topBooks on a.ISBN equals e.ISBN
                       select new BookeModel
                       {
                           BookModel = a,
                           BookGenreModel = b,
                           WroteModel = c,
                           AuthorModel = d,
                           genres = db.BookGenres.Where(z => z.ISBN == a.ISBN).Select(a => a.GenreName).ToList(),
                           viewGenres = (from m in db.BookGenres
                                         where m.ISBN == a.ISBN
                                         select m.GenreName + " ").ToList(),
                           GenreModel = db.Genres.ToList(),
                           GenreForView = db.Genres.Select(a => a.GenreName).ToList(),
                           reviews = db.Reviews.Where(b => b.ISBN == a.ISBN).Select(a => a.Rating).DefaultIfEmpty(0).Average(),
                           ViewBagGenreList = gL,
                           quantity = db.Purchaseds.Where(b => b.ISBN == a.ISBN).Select(a => a.qty).DefaultIfEmpty(0).Sum(),
                           //Wishlists = db.Wishlists.Where(b => b.Username == Session["Username"].ToString()).ToList()


                       };





                book = book.GroupBy(x => x.BookModel.ISBN).Select(x => x.FirstOrDefault());

            if (rate != null)
            {
                decimal rate1 = (decimal)(rate - 0.5);
                decimal rate2 = (decimal)(rate + 0.4);
                if (rate == 1)
                {
                    rate1 = (decimal)0.1;
                    

                }
               
                book = book.Where(x => (x.reviews >= rate1 && x.reviews <= rate2));
            }

                

               if(TopSeller != null){


                var topBooksPurchased = (from p in db.Purchaseds
                             select new
                             {
                                 ISBN = p.ISBN,
                                 qty = db.Purchaseds.Where(b => b.ISBN == p.ISBN).Select(a => a.qty).DefaultIfEmpty(0).Sum()

                             }).Distinct().OrderByDescending(a => a.qty).Select(b => b.qty).ToList();


                var trimTopBooks = new List<int?>(topBooksPurchased.ToList());
                int? topRange = 0;

                var TopSoldBooksByQuantity = 4;
                if (topBooksPurchased.Count() > TopSoldBooksByQuantity - 1)
                {

                    trimTopBooks = trimTopBooks.GetRange(0, TopSoldBooksByQuantity);
                    topRange = trimTopBooks[TopSoldBooksByQuantity - 1];
                    string msg = "";
                    foreach(var item in topBooksPurchased)
                    {
                        msg = msg + item + " ";
                    }

                    System.Windows.Forms.MessageBox.Show("List: " + msg + "\nTopRange:" + topRange);

                }
                else
                {
                    topRange = trimTopBooks[trimTopBooks.Count() - 1];
                }

                

                foreach (var item in trimTopBooks)
                {
                    book = book.Where(x => x.quantity >=  topRange);
                }

       

              


            }



                return book;
           

            }



        }





    
}