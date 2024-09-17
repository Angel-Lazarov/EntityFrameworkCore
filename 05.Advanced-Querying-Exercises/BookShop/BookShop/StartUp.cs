namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using System.Globalization;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            //Console.WriteLine(GetBooksByAgeRestriction(db, "miNor"));
            //Console.WriteLine(GetGoldenBooks(db));
            //Console.WriteLine(GetBooksByPrice(db));
            //Console.WriteLine(GetBooksNotReleasedIn(db, 1998));
            //Console.WriteLine(GetBooksByCategory(db, "horror mystery drama"));
            //Console.WriteLine(GetBooksReleasedBefore(db, "12-04-1992"));
            //Console.WriteLine(GetAuthorNamesEndingIn(db, "e"));
            //Console.WriteLine(GetBookTitlesContaining(db, "WOR"));
            //Console.WriteLine(GetBooksByAuthor(db, "po"));
            //Console.WriteLine(CountBooks(db, 40));
            //Console.WriteLine(CountCopiesByAuthor(db));
            //Console.WriteLine(GetTotalProfitByCategory(db));
            Console.WriteLine(GetMostRecentBooks(db));


        }

        //2.	Age Restriction
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            if (!Enum.TryParse(command, true, out AgeRestriction AgeRestriction))
            {
                return string.Empty;
            }

            var bookTitles = context.Books
                .Where(b => b.AgeRestriction == AgeRestriction)
                .Select(b => b.Title)
                .OrderBy(t => t)
                .ToArray();

            return string.Join(Environment.NewLine, bookTitles);
        }

        //3.	Golden Books
        public static string GetGoldenBooks(BookShopContext context)
        {

            var goldenBooks = context.Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .Select(b => new
                {
                    b.Title,
                    b.BookId
                })
                .OrderBy(t => t.BookId)
                .ToArray();

            //var sb = new StringBuilder();

            //foreach (var book in goldenBooks)
            //{
            //    sb.AppendLine(book.Title);
            //}

            //return sb.ToString().TrimEnd();
            return string.Join(Environment.NewLine, goldenBooks.Select(a => a.Title));
        }

        //4.	Books by Price
        public static string GetBooksByPrice(BookShopContext context)
        {
            var booksPrices = context.Books
                .Where(b => b.Price > 40)
                .Select(b => new
                {
                    b.Title,
                    b.Price
                })
                .OrderByDescending(b => b.Price)
                .ToArray();

            //var sb = new StringBuilder();

            //foreach (var book in booksPrices)
            //{
            //    sb.AppendLine($"{book.Title} - ${book.Price:f2}");
            //}

            //return sb.ToString().TrimEnd();
            return string.Join(Environment.NewLine, booksPrices.Select(a => $"{a.Title} - ${a.Price:f2}"));
        }

        //5.	Not Released In
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .Select(b => new
                {
                    b.Title,
                    b.BookId
                })
                .OrderBy(b => b.BookId)
                .ToArray();

            return string.Join(Environment.NewLine, books.Select(a => $"{a.Title}"));
        }

        //6.	Book Titles by Category
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] inputTokens = input.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var targetBooks = context.BooksCategories
                .Where(bc => inputTokens.Contains(bc.Category.Name.ToLower()))
                .Select(bc => bc.Book.Title)
                .OrderBy(t => t)
                .ToArray();

            return string.Join(Environment.NewLine, targetBooks.Select(tb => $"{tb}"));
        }

        //7.	Released Before Date
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime dt = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = context.Books
                .Where(b => b.ReleaseDate < dt)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:f2}")
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        //8.	Author Search
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => $"{a.FirstName} {a.LastName}")
                .AsEnumerable()
                .OrderBy(a => a);

            return string.Join(Environment.NewLine, authors);
        }

        //9.	Book Search
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .OrderBy(a => a)
                .Select(b => b.Title)
                .ToArray();


            return string.Join(Environment.NewLine, books);
        }

        //10.	Book Search by Author
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => $"{b.Title} ({b.Author.FirstName} {b.Author.LastName})")
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        //11.	Count Books
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            //var books = context.Books
            //    .Where(b => b.Title.Length > lengthCheck)
            //   .ToArray();
            //return books.Count();

            //return context.Books
            //.Where(b => b.Title.Length > lengthCheck).Count();.

            return context.Books
                .Count(b => b.Title.Length > lengthCheck);
        }

        //12.	Total Book Copies
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var copies = context.Authors
                .Select(a => new
                {
                    a.FirstName,
                    a.LastName,
                    TotalCopies = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(a => a.TotalCopies)
                .ToArray();

            return string.Join(Environment.NewLine, copies.Select(c => $"{c.FirstName} {c.LastName} - {c.TotalCopies}"));
        }

        //13.	Profit by Category
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var profit = context.Categories
                 .Select(c => new
                 {
                     c.Name,
                     Profit = c.CategoryBooks.Sum(cb => cb.Book.Copies * cb.Book.Price)
                 })
                 .OrderByDescending(c => c.Profit)
                 .ThenBy(c => c.Name)
                 .ToArray();


            return string.Join(Environment.NewLine, profit.Select(p => $"{p.Name} ${p.Profit:f2}"));
        }

        //14.	Most Recent Booksk

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var books = context.Categories
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    c.Name,
                    BookInfo = c.CategoryBooks
                    .OrderByDescending(cb => cb.Book.ReleaseDate)
                    .Take(3)
                    .Select(b => new
                    {
                        b.Book.Title,
                        b.Book.ReleaseDate!.Value.Year
                    })
                    .ToArray()

                }).ToArray();



            var sb = new StringBuilder();

            foreach (var item in books)
            {
                sb.AppendLine($"--{item.Name}");

                foreach (var bi in item.BookInfo)
                {
                    sb.AppendLine($"{bi.Title} ({bi.Year})");
                }
            }

            return sb.ToString().TrimEnd();
        }
        // shutdown /s /t 223200
    }
}


