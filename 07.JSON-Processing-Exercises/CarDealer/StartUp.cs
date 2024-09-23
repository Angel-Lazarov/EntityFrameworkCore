using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext context = new CarDealerContext();
            string inputJson = string.Empty;

            //inputJson = File.ReadAllText("../../../Datasets/suppliers.json");
            //Console.WriteLine(ImportSuppliers(context, inputJson));

            //inputJson = File.ReadAllText("../../../Datasets/parts.json");
            //Console.WriteLine(ImportParts(context, inputJson));

            //inputJson = File.ReadAllText("../../../Datasets/cars.json");
            //Console.WriteLine(ImportCars(context, inputJson));

            //inputJson = File.ReadAllText("../../../Datasets/customers.json");
            //Console.WriteLine(ImportCustomers(context, inputJson));

            //inputJson = File.ReadAllText("../../../Datasets/sales.json");
            //Console.WriteLine(ImportSales(context, inputJson));

            Console.WriteLine(GetOrderedCustomers(context));
            Console.WriteLine(GetCarsFromMakeToyota(context));
            Console.WriteLine(GetLocalSuppliers(context));
            Console.WriteLine(GetCarsWithTheirListOfParts(context));
            Console.WriteLine(GetTotalSalesByCustomer(context));
            Console.WriteLine(GetSalesWithAppliedDiscount(context));
        }

        //Query 9.Import Suppliers
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var suppliers = JsonConvert.DeserializeObject<List<Supplier>>(inputJson);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}.";
        }

        //Query 10. Import Parts
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var parts = JsonConvert.DeserializeObject<List<Part>>(inputJson);


            //int[] validSupplierIds = context.Suppliers.Select(s => s.Id).ToArray();

            //Part[] validParts = parts.Where(p => validSupplierIds.Contains(p.SupplierId)).ToArray();

            var validParts = new List<Part>();

            foreach (Part part in parts)
            {
                if (!context.Suppliers.Any(s => s.Id == part.SupplierId))
                {
                    continue;
                }
                validParts.Add(part);
            }

            context.Parts.AddRange(validParts);
            context.SaveChanges();

            return $"Successfully imported {validParts.Count}.";
        }

        //Query 11. Import Cars
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var carsDtos = JsonConvert.DeserializeObject<List<ImportCarDto>>(inputJson);

            var cars = new HashSet<Car>();
            var partsCars = new HashSet<PartCar>();

            foreach (var carDto in carsDtos)
            {
                var newCar = new Car()
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TraveledDistance = carDto.TraveledDistance
                };
                cars.Add(newCar);

                foreach (int partId in carDto.PartsId.Distinct())
                {
                    var carPart = new PartCar()
                    {
                        PartId = partId,
                        Car = newCar
                    };
                    partsCars.Add(carPart);
                }
            }

            context.Cars.AddRange(cars);
            context.PartsCars.AddRange(partsCars);

            context.SaveChanges();

            return $"Successfully imported {cars.Count}.";
        }

        //Query 12. Import Customers
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var customers = JsonConvert.DeserializeObject<List<Customer>>(inputJson);

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count}.";
        }

        //Query 13. Import Sales
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var sales = JsonConvert.DeserializeObject<List<Sale>>(inputJson);

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}.";
        }

        //Query 14. Export Ordered Customers
        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var orderedCustomers = context.Customers
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver)
                .Select(c => new
                {
                    c.Name,
                    BirthDate = c.BirthDate.ToString("dd/MM/yyyy"),
                    c.IsYoungDriver
                })
                .ToArray();

            var settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                //NullValueHandling = NullValueHandling.Ignore
            };

            return JsonConvert.SerializeObject(orderedCustomers, settings);
        }

        //Query 15. Export Cars from Make Toyota
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var toyotaCars = context.Cars
                .Where(c => c.Make == "Toyota")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .Select(c => new
                {
                    c.Id,
                    c.Make,
                    c.Model,
                    c.TraveledDistance
                })
                .ToArray();

            //var settings = new JsonSerializerSettings()
            //{
            //    Formatting = Formatting.Indented
            //};

            return JsonConvert.SerializeObject(toyotaCars, Formatting.Indented);
        }

        //Query 16. Export Local Suppliers
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var localSuppliers = context.Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    PartsCount = s.Parts.Count
                })
                .ToArray();

            JsonSerializerSettings setting = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            };

            return JsonConvert.SerializeObject(localSuppliers, setting);
        }

        //Query 17. Export Cars with Their List of Parts
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var carWithParts = context.Cars
                .Select(c => new
                {
                    car = new
                    {
                        c.Make,
                        c.Model,
                        c.TraveledDistance

                    },
                    parts = c.PartsCars.Select(pc => new
                    {
                        Name = pc.Part.Name,
                        Price = pc.Part.Price.ToString("f2")
                    })
                    .ToArray()

                })
                .ToArray();

            return JsonConvert.SerializeObject(carWithParts, Formatting.Indented);

        }

        //Query 18. Export Total Sales by Customer
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(c => c.Sales.Any())
                .Select(c => new
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Count,
                    SpentMoney = c.Sales.SelectMany(s => s.Car.PartsCars.Select(pc => pc.Part.Price)).Sum()
                })

                .OrderByDescending(c => c.SpentMoney)
                .ThenByDescending(c => c.BoughtCars)
                .ToArray();

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return JsonConvert.SerializeObject(customers, settings);

        }

        //Query 19. Export Sales with Applied Discount
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Where(s => s.Car != null && s.Customer != null)
                .Take(10)
                .Select(s => new
                {
                    car = new
                    {
                        s.Car.Make,
                        s.Car.Model,
                        s.Car.TraveledDistance
                    },
                    customerName = s.Customer.Name,
                    discount = s.Discount.ToString("f2"),

                    price = s.Car.PartsCars.Select(pc => pc.Part.Price).Sum().ToString("f2"),

                    priceWithDiscount = (s.Car.PartsCars.Select(pc => pc.Part.Price).Sum() -
                        s.Car.PartsCars.Select(pc => pc.Part.Price).Sum() * s.Discount / 100).ToString("f2")
                })
                .ToArray();

            return JsonConvert.SerializeObject(sales, Formatting.Indented);

        }

    }
}