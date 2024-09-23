using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace BookShop;

using Data;
using Models.Enums;

public class StartUp
{
    public static void Main()
    {
        using var db = new BookShopContext();
        // DbInitializer.ResetDatabase(db);

        Stopwatch sw = Stopwatch.StartNew();
        //Console.WriteLine(GetBooksByAgeRestriction(db, "miNor"));
        //Console.WriteLine(GetGoldenBooks(db));
        //Console.WriteLine(GetBooksByPrice(db));
        //Console.WriteLine(GetBooksNotReleasedIn(db, 1998));
        //Console.WriteLine(GetBooksByCategory(db, "horror mystery drama"));
        //Console.WriteLine(GetBooksReleasedBefore(db, "30-12-1989"));
        //Console.WriteLine(GetAuthorNamesEndingIn(db, "dy"));
        //Console.WriteLine(GetBookTitlesContaining(db, "WOR"));
        //Console.WriteLine(GetBooksByAuthor(db, "po"));
        //Console.WriteLine(CountBooks(db, 12));
        //Console.WriteLine(CountCopiesByAuthor(db));
        //Console.WriteLine(GetTotalProfitByCategory(db));
        //Console.WriteLine(GetMostRecentBooks(db));
        IncreasePrices(db);
        //Console.WriteLine(RemoveBooks(db));
        sw.Stop();
        Console.WriteLine(sw.ElapsedMilliseconds);



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
            .OrderBy(b => b.BookId)
            .Select(b => b.Title)
            .ToArray();

        //var sb = new StringBuilder();

        //foreach (var book in goldenBooks)
        //{
        //    sb.AppendLine(book.Title);
        //}

        //return sb.ToString().TrimEnd();
        return string.Join(Environment.NewLine, goldenBooks);
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

        var targetBooks = context.Books
            .Where(b => b.BookCategories.Any(bc => inputTokens.Contains(bc.Category.Name.ToLower())))
            .Select(b => b.Title)
            .OrderBy(b => b)
            .ToArray();


        //var targetBooks = context.BooksCategories
        //    .Where(bc => inputTokens.Contains(bc.Category.Name.ToLower()))
        //    .Select(bc => bc.Book.Title)
        //    .OrderBy(t => t)
        //    .ToArray();

        return string.Join(Environment.NewLine, targetBooks);
    }

    //7.	Released Before Date
    public static string GetBooksReleasedBefore(BookShopContext context, string date)
    {
        DateTime dt = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

        var books = context.Books
            .Where(b => b.ReleaseDate.Value < dt)
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
            .OrderBy(a => a)
            .ToArray();

        return string.Join(Environment.NewLine, authors);
    }

    //9.	Book Search
    public static string GetBookTitlesContaining(BookShopContext context, string input)
    {
        var books = context.Books
            .Where(b => b.Title.ToLower().Contains(input.ToLower()))
            .Select(b => b.Title)
            .OrderBy(b => b)
            .ToArray();

        //var books = context.Books
        //    .AsEnumerable()
        //    .Where(b => b.Title.IndexOf(input, StringComparison.OrdinalIgnoreCase) >= 0)
        //    .Select(b => b.Title)
        //    .OrderBy(b => b)
        //.ToArray();


        return string.Join(Environment.NewLine, books);
    }

    //10.	Book Search by Author
    public static string GetBooksByAuthor(BookShopContext context, string input)
    {
        var books = context.Books
            .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
            .OrderBy(b => b.BookId)
            .Select(b => $"{b.Title} ({b.Author.FirstName!} {b.Author.LastName})")
            .ToArray();


        return string.Join(Environment.NewLine, books);
    }

    //11.	Count Books
    public static int CountBooks(BookShopContext context, int lengthCheck)
    {
        //var books = context.Books
        //    .Where(b => b.Title.Length > lengthCheck)
        //    .ToArray();

        //return books.Count();

        return context.Books
            .Count(b => b.Title.Length > lengthCheck);
    }

    //12.	Total Book Copies
    public static string CountCopiesByAuthor(BookShopContext context)
    {
        var totalCopies = context.Authors
            .Select(a => new
            {
                FullName = $"{a.FirstName} {a.LastName}",
                copies = a.Books.Sum(b => b.Copies)
            })
            .OrderByDescending(tc => tc.copies)
            .ToArray();

        return string.Join(Environment.NewLine, totalCopies.Select(tc => $"{tc.FullName} - {tc.copies}"));
    }

    //13.	Profit by Category
    public static string GetTotalProfitByCategory(BookShopContext context)
    {
        var totalProfit = context.Categories
            .Select(c => new
            {
                c.Name,
                TotalProfit = c.CategoryBooks
                    .Sum(cb => cb.Book.Copies * cb.Book.Price)
                //    .Select(bc => bc.Book.Copies * bc.Book.Price)
                //    .Sum()
            })
            .OrderByDescending(c => c.TotalProfit)
            .ThenBy(c => c.Name)
            .ToArray();


        return string.Join(Environment.NewLine, totalProfit.Select(tp => $"{tp.Name} ${tp.TotalProfit:f2}"));
    }

    //14.	Most Recent Books
    public static string GetMostRecentBooks(BookShopContext context)
    {
        var sb = new StringBuilder();

        //var mostRecent = context.Categories
        //    .OrderBy(c => c.Name)
        //    .Select(c => new
        //    {
        //        c.Name,
        //        ResultBooks = c.CategoryBooks
        //            .OrderByDescending(cb => cb.Book.ReleaseDate)
        //            .Take(3)
        //            .Select(cb => $"{cb.Book.Title} ({cb.Book.ReleaseDate.Value.Year})")
        //    })
        //    .ToArray();

        //foreach (var item in mostRecent)
        //{
        //    sb.AppendLine($"--{item.Name}");

        //    foreach (var book in item.ResultBooks)
        //    {
        //        sb.AppendLine($"{book}");
        //    }
        //}

        var mostRecent = context.Categories
            .OrderBy(c => c.Name)
            .Select(c => new
            {
                c.Name,
                ResultBooks = c.CategoryBooks
                    .OrderByDescending(cb => cb.Book.ReleaseDate)
                    .Take(3)
                    .Select(cb => new
                    {
                        cb.Book.Title,
                        cb.Book.ReleaseDate
                    })
                    .ToArray()
            })
            .ToArray();

        foreach (var item in mostRecent)
        {
            sb.AppendLine($"--{item.Name}");

            foreach (var book in item.ResultBooks)
            {
                sb.AppendLine($"{book.Title} ({book.ReleaseDate.Value.Year})");
            }
        }

        return sb.ToString().TrimEnd();
    }

    //15.	Increase Prices
    public static void IncreasePrices(BookShopContext context)
    {
        var allBooks = context.Books
            .Where(b => b.ReleaseDate.HasValue && b.ReleaseDate.Value.Year < 2010)
            .ToArray();

        foreach (var book in allBooks)
        {
            book.Price += 5;
        }

        //context.BulkUpdate(allBooks);
        context.SaveChanges();
    }

    //16.	Remove Books
    public static int RemoveBooks(BookShopContext context)
    {
        var books = context.Books
            .Where(b => b.Copies < 4200)
            .ToArray();
        context.RemoveRange(books);
        context.SaveChanges();

        return books.Length;
    }


}


