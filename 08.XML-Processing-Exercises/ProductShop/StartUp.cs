using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            using ProductShopContext context = new ProductShopContext();
            string inputXml = String.Empty;

            //inputXml = File.ReadAllText("../../../Datasets/users.xml");
            //Console.WriteLine(ImportUsers(context, inputXml));

            //inputXml = File.ReadAllText("../../../Datasets/products.xml");
            //Console.WriteLine(ImportProducts(context, inputXml));

            //inputXml = File.ReadAllText("../../../Datasets/categories.xml");
            //Console.WriteLine(ImportCategories(context, inputXml));

            //inputXml = File.ReadAllText("../../../Datasets/categories-products.xml");
            //Console.WriteLine(ImportCategoryProducts(context, inputXml));

            //Console.WriteLine(GetProductsInRange(context));
            //Console.WriteLine(GetSoldProducts(context));
            //Console.WriteLine(GetCategoriesByProductsCount(context));
            Console.WriteLine(GetUsersWithProducts(context));
        }

        //Query 1. Import Users
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(UserDtoImport[]), new XmlRootAttribute("Users"));

            using StringReader reader = new StringReader(inputXml);

            var userDtos = (UserDtoImport[])serializer.Deserialize(reader);
            HashSet<User> users = new HashSet<User>();

            foreach (var dto in userDtos)
            {
                if (dto.Age == null)
                {
                    continue;
                }

                User user = new User()
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Age = dto.Age
                };
                users.Add(user);
            }

            context.Users.AddRange(users);
            context.SaveChanges();
            return $"Successfully imported {users.Count}";
        }

        //Query 2. Import Products
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ProductDtoImport[]), new XmlRootAttribute("Products"));

            using StringReader reader = new StringReader(inputXml);

            var productsDtos = (ProductDtoImport[])serializer.Deserialize(reader);

            var products = productsDtos
                .Select(p => new Product()
                {
                    Name = p.Name,
                    Price = p.Price,
                    BuyerId = p.BuyerId,
                    SellerId = p.SellerId
                })
                .ToArray();

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Length}";
        }

        //Query 3. Import Categories
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer =
                new XmlSerializer(typeof(CategoryDtoImport[]), new XmlRootAttribute("Categories"));

            using StringReader reader = new StringReader(inputXml);

            CategoryDtoImport[] categoryDtos = (CategoryDtoImport[])serializer.Deserialize(reader);

            var categories = categoryDtos
                .Select(c => new Category()
                {
                    Name = c.Name
                })
                .ToList<Category>();

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        //Query 4. Import Categories and Products
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CategoryProductDtoImport[]),
                new XmlRootAttribute("CategoryProducts"));

            using StringReader reader = new StringReader(inputXml);

            CategoryProductDtoImport[] dtos = (CategoryProductDtoImport[])serializer.Deserialize(reader);

            var validProductsIds = context.Products.Select(p => p.Id);
            var validCategoryIds = context.Categories.Select(c => c.Id);

            var validProCat = new List<CategoryProduct>();

            //foreach (var dto in dtos)
            //{
            //    if (validCategoryIds.All(id => id != dto.CategoryId) ||
            //        validProductsIds.All(id => id != dto.ProductId))
            //    {
            //        continue;
            //    }

            //    CategoryProduct cp = new CategoryProduct()
            //    {
            //        CategoryId = dto.CategoryId,
            //        ProductId = dto.ProductId
            //    };

            //    validProCat.Add(cp);
            //}

            //context.CategoryProducts.AddRange(validProCat);
            //context.SaveChanges();
            //return $"Successfully imported {validProCat.Count}";

            var categoryProducts = dtos
                .Where(dto => validCategoryIds.Contains(dto.CategoryId) &&
                              validProductsIds.Contains(dto.ProductId))
                .Select(c => new CategoryProduct()
                {
                    ProductId = c.ProductId,
                    CategoryId = c.CategoryId
                })
                .ToArray();

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Length}";

        }

        //Query 5. Export Products In Range
        public static string GetProductsInRange(ProductShopContext context)
        {
            var productsInRange = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Take(10)
                .Select(p => new ProductDtoExport
                {
                    Name = p.Name,
                    Price = p.Price,
                    BuyerNames = p.Buyer.FirstName + " " + p.Buyer.LastName
                })
                .ToArray();


            XmlSerializer serializer = new XmlSerializer(typeof(ProductDtoExport[]), new XmlRootAttribute("Products"));

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            StringBuilder sb = new();
            using StringWriter writer = new StringWriter(sb);

            serializer.Serialize(writer, productsInRange, namespaces);

            return sb.ToString().TrimEnd();

        }

        //Query 6. Export Sold Products
        public static string GetSoldProducts(ProductShopContext context)
        {
            var soldProductsDtos = context.Users
                .Where(u => u.ProductsSold.Any(ps => ps.Buyer != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Take(5)
                .Select(u => new UserSoldProductsDtoExport()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SoldProducts = u.ProductsSold
                        .Select(ps => new ProductSoldDtoExport()
                        {
                            ProductName = ps.Name,
                            Price = ps.Price
                        }).ToArray()
                }).ToArray();

            XmlSerializer serializer =
                new XmlSerializer(typeof(UserSoldProductsDtoExport[]), new XmlRootAttribute("Users"));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            StringBuilder sb = new();
            using StringWriter writer = new StringWriter(sb);

            serializer.Serialize(writer, soldProductsDtos, namespaces);

            return sb.ToString().TrimEnd();
        }

        //Query 7. Export Categories By Products Count
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(c => new CategoryByCountDtoExport()
                {
                    Name = c.Name,
                    ProductsCount = c.CategoryProducts.Count,
                    AveragePrice = c.CategoryProducts.Average(cp => cp.Product.Price),
                    TotalRevenue = c.CategoryProducts.Sum(cp => cp.Product.Price)
                })
                .OrderByDescending(x => x.ProductsCount)
                .ThenBy(x => x.TotalRevenue)
                .ToArray();

            XmlSerializer serializer =
                new XmlSerializer(typeof(CategoryByCountDtoExport[]), new XmlRootAttribute("Categories"));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            StringBuilder sb = new();
            using StringWriter writer = new StringWriter(sb);

            serializer.Serialize(writer, categories, namespaces);

            return sb.ToString().TrimEnd();
        }

        //Query 8. Export Users and Products
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            // Ебахти зора!!!!
            var users = new MainUserDto()
            {
                Count = context.Users.Where(u => u.ProductsSold.Any()).Count(),
                UsersInfo = context.Users.Where(u => u.ProductsSold.Any(ps => ps.Buyer != null))

                    .Select(u => new UserInfosDtoExport()
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Age = u.Age,
                        SoldProducts = new SoldProductsDto()
                        {
                            SoldProductsCount = u.ProductsSold.Count,
                            Products = u.ProductsSold
                                .Select(ps => new ProductNamePriceDtoExport()
                                {
                                    Name = ps.Name,
                                    Price = ps.Price
                                })
                                .OrderByDescending(x => x.Price)
                                .ToArray()
                        }

                    })
                    .OrderByDescending(u => u.SoldProducts.SoldProductsCount)
                    .Take(10)
                    .ToArray()
            };


            XmlSerializer serializer = new XmlSerializer(typeof(MainUserDto), new XmlRootAttribute("Users"));

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            StringBuilder sb = new();
            using StringWriter writer = new StringWriter(sb);

            serializer.Serialize(writer, users, namespaces);

            return sb.ToString().TrimEnd();

        }

    }
}

