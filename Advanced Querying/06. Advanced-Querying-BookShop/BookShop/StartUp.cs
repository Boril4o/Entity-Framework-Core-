using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookShop.Models;
using BookShop.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookShop
{
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);
        }

        //Problem 1
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            IEnumerable<string> booksTitles = context
                 .Books
                 .Where(b => b.AgeRestriction == Enum.Parse<AgeRestriction>(command, true))
                 .Select(b => b.Title)
                 .OrderBy(b => b)
                 .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var title in booksTitles)
            {
                sb.AppendLine(title);
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 2
        public static string GetGoldenBooks(BookShopContext context)
        {
            IEnumerable<string> booksTitles = context
                .Books
                .Where(b => b.EditionType == EditionType.Gold &&
                            b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var title in booksTitles)
            {
                sb.AppendLine(title);
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 3
        public static string GetBooksByPrice(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            context
                .Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => new
                {
                    b.Title,
                    b.Price
                })
                .ToList()
                .ForEach(b => sb.AppendLine($"{b.Title} - ${b.Price:f2}"));

            return sb
                .ToString()
                .TrimEnd();
        }

        //Problem 4
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            IEnumerable<string> bookTitles = context
                .Books
                .Where(b => b.ReleaseDate != null)
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var title in bookTitles)
            {
                sb.AppendLine(title);
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 5
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.ToLower())
                .ToArray();


            List<string> titles = new List<string>();
            foreach (var category in categories)
            {
                IEnumerable<string> booksTitles = context
                    .Books
                    .Where(b => b.BookCategories
                        .Select(c => c.Category.Name).Contains(category))
                    .Select(b => b.Title)
                    .ToArray();

                foreach (var title in booksTitles)
                {
                    titles.Add(title);
                }
            }

            StringBuilder sb = new StringBuilder();
            foreach (var title in titles.Distinct().OrderBy(b => b))
            {
                sb.AppendLine(title);
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 6
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var books = context
                .Books
                .Where(b => b.ReleaseDate != null)
                .Where(b => DateTime.Parse(date) > b.ReleaseDate)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new
                {
                    b.Title,
                    b.EditionType,
                    b.Price
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 7
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var AuthorsNames = context
                .Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => new
                {
                    a.FirstName,
                    a.LastName,
                })
                .OrderBy(a => a.FirstName)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var authors in AuthorsNames)
            {
                sb.AppendLine(authors.FirstName + ' ' + authors.LastName);
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 8
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            string[] booksTitles = context
                .Books
                .Where(b => b.Title.ToLower().Contains(input))
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var title in booksTitles)
            {
                sb.AppendLine(title);
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 9
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var booksTitlesAndAuthors = context
                .Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input))
                .OrderBy(b => b.BookId)
                .Select(b => new
                {
                    b.Title,
                    AuthorFullName = string.Concat(b.Author.FirstName, ' ', b.Author.LastName)
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var item in booksTitlesAndAuthors)
            {
                sb.AppendLine($"{item.Title} ({item.AuthorFullName})");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 10
        public static int CountBooks(BookShopContext context, int lengthCheck)
            => context.Books.Count(b => b.Title.Length > lengthCheck);

        //Problem 11
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authorsCopies = context
                .Authors
                .Select(a => new
                {
                    FullName = string.Concat(a.FirstName, ' ', a.LastName),
                    CopiesCount = a.Books.Sum(c => c.Copies)
                })
                .OrderByDescending(a => a.CopiesCount)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var item in authorsCopies)
            {
                sb.AppendLine($"{item.FullName} - {item.CopiesCount}");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 12
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var booksProfit = context
                .BooksCategories
                .Include(x => x.Category)
                .Include(x => x.Book)
                .AsEnumerable()
                .GroupBy(bc => bc.Category.Name)
                .Select(bk => new
                {
                    Category = bk.Key,
                    TotlaPrice = bk.Sum(b => b.Book.Copies * b.Book.Price)
                })
                .OrderByDescending(b => b.TotlaPrice)
                .ThenBy(b => b.Category)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var bp in booksProfit)
            {
                sb.AppendLine($"{bp.Category} ${bp.TotlaPrice:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 13
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categories = context
                .Categories
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    CategoryName = c.Name,
                    books = c.CategoryBooks
                        .Where(b => b.Book.ReleaseDate != null)
                        .OrderByDescending(b => b.Book.ReleaseDate)
                        .Select(b => new
                            {
                                b.Book.Title,
                                b.Book.ReleaseDate.Value.Year
                            })
                        .Take(3)
                });

            StringBuilder sb = new StringBuilder();
            foreach (var category in categories)
            {
                sb.AppendLine($"--{category.CategoryName}");
                foreach (var book in category.books)
                {
                    sb.AppendLine($"{book.Title} ({book.Year})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 14
        public static void IncreasePrices(BookShopContext context)
        {
            const decimal increasePriceValue = 5;

            IQueryable<Book> books = context
                .Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                .AsQueryable();

            foreach (var book in books)
            {
                book.Price += increasePriceValue;
            }

            context.SaveChanges();
        }

        //Problem 15
        public static int RemoveBooks(BookShopContext context)
        {
            IQueryable<BookCategory> bookCategoriesToDelete = context
                .BooksCategories
                .Where(b => b.Book.Copies < 4200)
                .AsQueryable();

            foreach (var bookCategory in bookCategoriesToDelete)
            {
                context.Remove(bookCategory);
            }

            context.SaveChanges();

            IQueryable<Book> booksToDelete = context
                .Books
                .Where(b => b.Copies < 4200)
                .AsQueryable();

            int countOfDeletedBooks = booksToDelete.Count();

            foreach (var book in booksToDelete)
            {
                context.Remove(book);
            }

            context.SaveChanges();

            return countOfDeletedBooks;
        }
    }
}
