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
            string inputJson = string.Empty;

            //inputJson = File.ReadAllText("../../../Datasets/users.json");
            //Console.WriteLine(ImportUsers(context, inputJson));

            //inputJson = File.ReadAllText("../../../Datasets/products.json");
            //Console.WriteLine(ImportProducts(context, inputJson));

            //inputJson = File.ReadAllText("../../../Datasets/categories.json");
            //Console.WriteLine(ImportCategories(context, inputJson));

            //inputJson = File.ReadAllText("../../../Datasets/categories-products.json");
            //Console.WriteLine(ImportCategoryProducts(context, inputJson));

            Console.WriteLine(GetProductsInRange(context));

        }

        //Query 1. Import Users
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            User[] users = JsonConvert.DeserializeObject<User[]>(inputJson);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Length}";
        }

        //Query 2. Import Products
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var products = JsonConvert.DeserializeObject<List<Product>>(inputJson);

            context.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

        //Query 3. Import Categories
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categories = JsonConvert.DeserializeObject<List<Category>>(inputJson);

            //foreach (var category in categories)
            //{
            //    if (category.Name != null)
            //    { context.Categories.Add(category); }
            //}

            categories.RemoveAll(c => c.Name == null);
            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        //Query 4. Import Categories and Products
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categoryProducts = JsonConvert.DeserializeObject<CategoryProduct[]>(inputJson);

            context.CategoriesProducts.AddRange(categoryProducts);
            context.SaveChanges();

            //var settings = new JsonSerializerSettings()
            //{
            //    NullValueHandling = NullValueHandling.Ignore,
            //    ContractResolver = new CamelCasePropertyNamesContractResolver(),
            //    Formatting = Formatting.Indented
            //};

            //return JsonConvert.SerializeObject(productsInRange, settings);

            return $"Successfully imported {categoryProducts.Length}";
        }

        //Query 5. Export Products in Range
        public static string GetProductsInRange(ProductShopContext context)
        {
            var sellers = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(p => new
                {
                    name = p.Name,
                    price = p.Price,
                    seller = p.Seller.FirstName + " " + p.Seller.LastName
                })
                .ToList();
            var settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            };

            return JsonConvert.SerializeObject(sellers, settings);
            //return JsonConvert.SerializeObject(sellers, Formatting.Indented);
        }
    }
}