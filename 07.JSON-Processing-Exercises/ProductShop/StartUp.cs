using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext context = new ProductShopContext();
            var input = string.Empty;

            //input = File.ReadAllText("../../../Datasets/users.json");
            //Console.WriteLine(ImportUsers(context, input));

            //input = File.ReadAllText("../../../Datasets/products.json");
            //Console.WriteLine(ImportProducts(context, input));

            //input = File.ReadAllText("../../../Datasets/categories.json");
            //Console.WriteLine(ImportCategories(context, input));

            //input = File.ReadAllText("../../../Datasets/categories-products.json");
            //Console.WriteLine(ImportCategoryProducts(context, input));

            Console.WriteLine(GetProductsInRange(context));
            Console.WriteLine(GetSoldProducts(context));
            Console.WriteLine(GetCategoriesByProductsCount(context));
            Console.WriteLine(GetUsersWithProducts(context));

        }

        //Query 1. Import Users
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var users = JsonConvert.DeserializeObject<List<User>>(inputJson);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }

        //Query 2. Import Products
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var products = JsonConvert.DeserializeObject<List<Product>>(inputJson);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

        //Query 3. Import Categories
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categories = JsonConvert.DeserializeObject<List<Category>>(inputJson);

            categories.RemoveAll(c => c.Name == null);
            context.Categories.AddRange(categories);
            context.SaveChanges();
            return $"Successfully imported {categories.Count}";
        }

        //Query 4. Import Categories and Products
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categoryProducts = JsonConvert.DeserializeObject<List<CategoryProduct>>(inputJson);

            context.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count}";
        }

        //Query 5. Export Products in Range
        public static string GetProductsInRange(ProductShopContext context)
        {
            var productsInRange = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(p => new
                {
                    p.Name,
                    p.Price,
                    Seller = p.Seller.FirstName + " " + p.Seller.LastName
                })
                .AsNoTracking()
                .ToArray();

            var settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()

            };

            string json = JsonConvert.SerializeObject(productsInRange, settings);

            //return string.Join(Environment.NewLine, productsInRange.Select(p => $"{p.Name} {p.Price} {p.Seller}"));
            return json;

        }

        //Query 6. Export Sold Products
        public static string GetSoldProducts(ProductShopContext context)
        {
            var sellers = context.Users
                .Where(u => u.ProductsSold.Any(ps => ps.Buyer != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    SoldProducts = u.ProductsSold
                        .Select(p => new
                        {
                            p.Name,
                            p.Price,
                            BuyerFirstName = p.Buyer.FirstName,
                            BuyerLastName = p.Buyer.LastName
                        })
                })
                .AsNoTracking()
                .ToArray();

            var settings = FormatToJson();


            return JsonConvert.SerializeObject(sellers, settings);
        }

        //Query 7. Export Categories by Products Count
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {

            var categories = context.Categories
                .OrderByDescending(c => c.CategoriesProducts.Count)
                .Select(c => new
                {
                    Category = c.Name,
                    ProductsCount = c.CategoriesProducts.Count,
                    AveragePrice = (c.CategoriesProducts.Any() ? c.CategoriesProducts.Average(cp => cp.Product.Price) : 0).ToString("f2"),
                    TotalRevenue = (c.CategoriesProducts.Any() ? c.CategoriesProducts.Sum(cp => cp.Product.Price) : 0).ToString("f2")
                })
                .ToArray();

            var settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return JsonConvert.SerializeObject(categories, settings);
        }

        //Query 8. Export Users and Products
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    u.Age,
                    SoldProducts = new
                    {
                        Count = u.ProductsSold.Count(p => p.Buyer != null),
                        Products = u.ProductsSold
                            .Where(p => p.Buyer != null)
                            .Select(p => new
                            {
                                p.Name,
                                p.Price
                            })
                            .ToArray()
                    }
                })
                .OrderByDescending(u => u.SoldProducts.Count)
                .ToArray();

            var result = new
            {
                UsersCount = users.Length,
                Users = users
            };

            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            };

            return JsonConvert.SerializeObject(result, settings);
        }



        private static JsonSerializerSettings FormatToJson()
        {
            var settings = new JsonSerializerSettings()
            {

                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                //  NullValueHandling = NullValueHandling.Ignore

            };
            return settings;
        }

        private static string FormatToJson(object obj)
        {
            var settings = new JsonSerializerSettings()
            {

                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                //  NullValueHandling = NullValueHandling.Ignore

            };
            return JsonConvert.SerializeObject(obj, settings);
        }


    }
}