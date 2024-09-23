using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using CarDealer.Utilities;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            using CarDealerContext context = new CarDealerContext();
            string inputXml = string.Empty;

            //inputXml = File.ReadAllText("../../../Datasets/suppliers.xml");
            //Console.WriteLine(ImportSuppliers(context, inputXml));

            //inputXml = File.ReadAllText("../../../Datasets/parts.xml");
            //Console.WriteLine(ImportParts(context, inputXml));

            //inputXml = File.ReadAllText("../../../Datasets/cars.xml");
            //Console.WriteLine(ImportCars(context, inputXml));

            //inputXml = File.ReadAllText("../../../Datasets/customers.xml");
            //Console.WriteLine(ImportCustomers(context, inputXml));

            //inputXml = File.ReadAllText("../../../Datasets/sales.xml");
            //Console.WriteLine(ImportSales(context, inputXml));

            //Console.WriteLine(GetCarsWithDistance_R(context));
            //Console.WriteLine(GetCarsWithDistance_Helper(context));
            //Console.WriteLine(GetCarsWithDistance(context));
            //Console.WriteLine(GetCarsFromMakeBmw(context));
            //Console.WriteLine(GetLocalSuppliers(context));
            //Console.WriteLine(GetCarsWithTheirListOfParts(context));
            //Console.WriteLine(GetTotalSalesByCustomer(context));
            Console.WriteLine(GetSalesWithAppliedDiscount(context));

        }

        //Query 9. Import Suppliers
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute("Suppliers");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportSupplierDto[]), xmlRoot);

            using StringReader reader = new StringReader(inputXml);

            var importedDtos = (ImportSupplierDto[])xmlSerializer.Deserialize(reader);

            var suppliers = importedDtos
                .Select(dto => new Supplier()
                {
                    Name = dto.Name,
                    IsImporter = dto.IsImporter
                }).ToList();


            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();


            return $"Successfully imported {suppliers.Count}";
        }

        //Query 10. Import Parts
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportPartDto[]), new XmlRootAttribute("Parts"));

            using var reader = new StringReader(inputXml);

            ImportPartDto[] importPartDtos = (ImportPartDto[])xmlSerializer.Deserialize(reader);

            var validParts = new List<Part>();


            var validSuppliersIds = context.Suppliers.Select(s => s.Id).ToArray();
            //validParts = importPartDtos
            //    .Where(ipd => validSuppliersIds.Contains(ipd.SupplierId)).ToArray()
            //    .Select(dto => new Part()
            //    {
            //        Name = dto.Name,
            //        Price = dto.Price,
            //        Quantity = dto.Quantity,
            //        SupplierId = dto.SupplierId
            //    })
            //    .ToList();


            foreach (var importPartDto in importPartDtos)
            {
                if (!context.Suppliers.Any(s => s.Id == importPartDto.SupplierId))
                {
                    continue;
                }

                Part part = new Part()
                {
                    Name = importPartDto.Name,
                    Price = importPartDto.Price,
                    Quantity = importPartDto.Quantity,
                    SupplierId = importPartDto.SupplierId
                };
                validParts.Add(part);
            }

            context.Parts.AddRange(validParts);
            context.SaveChanges();

            return $"Successfully imported {validParts.Count}";
        }

        //Query 11. Import Cars
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CarDto[]), new XmlRootAttribute("Cars"));

            using var reader = new StringReader(inputXml);

            var carDtos = (CarDto[])serializer.Deserialize(reader);
            ICollection<Car> cars = new HashSet<Car>();

            foreach (var carDto in carDtos)
            {
                Car newCar = new Car()
                {
                    Model = carDto.Model,
                    Make = carDto.Make,
                    TraveledDistance = carDto.TraveledDistance
                };

                //var ids = carDto.Parts.Select(e => e.PartId).Distinct().ToArray();
                //foreach (var partDto in ids)

                //foreach (var partDto in carDto.Parts.Distinct()) -- НЕ !!!!!
                foreach (var partDto in carDto.Parts.DistinctBy(p => p.PartId))
                {
                    if (!context.Parts.Any(p => partDto.PartId == p.Id))
                    {
                        continue;
                    }

                    var partCar = new PartCar()
                    {
                        PartId = partDto.PartId
                    };
                    newCar.PartsCars.Add(partCar);
                }

                cars.Add(newCar);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }

        //Query 12. Import Customers
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(CustomerDtoImport[]), new XmlRootAttribute("Customers"));

            using var reader = new StringReader(inputXml);

            var customerDtos = (CustomerDtoImport[])serializer.Deserialize(reader);
            Customer[] customers = customerDtos.
                Select(dto => new Customer()
                {
                    Name = dto.Name,
                    //BirthDate = DateTime.Parse(customerDto.BirthDate!, CultureInfo.InvariantCulture),
                    BirthDate = dto.BirthDate,
                    IsYoungDriver = dto.IsYoungDriver
                })
                .ToArray();



            context.Customers.AddRange(customers);
            context.SaveChanges();
            return $"Successfully imported {customers.Length}";
        }

        //Query 13. Import Sales
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SaleDto_Import[]), new XmlRootAttribute("Sales"));
            using var reader = new StringReader(inputXml);

            var saleDtos = (SaleDto_Import[])serializer.Deserialize(reader);

            List<Sale> sales = new List<Sale>();


            int[] carsIds = context.Cars
                .Select(c => c.Id).ToArray();

            var validSaleDtos = saleDtos
                .Where(sd => carsIds.Contains(sd.CarId.Value))
                .ToArray();


            //sales = validSaleDtos.Select(vsd => new Sale()
            //{
            //    CarId = vsd.CarId.Value,
            //    CustomerId = vsd.CustomerId,
            //    Discount = vsd.Discount
            //}).ToList();



            foreach (var saleDto in saleDtos)
            {
                //if (!saleDto.CarId.HasValue || !context.Cars.Any(c => c.Id == saleDto.CarId))
                if (!saleDto.CarId.HasValue || carsIds.All(id => id != saleDto.CarId.Value))
                {
                    continue;
                }

                Sale sale = new Sale()
                {
                    CarId = saleDto.CarId.Value,
                    CustomerId = saleDto.CustomerId,
                    Discount = saleDto.Discount
                };
                sales.Add(sale);
            }

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}";
        }

        //Query 14. Export Cars With Distance - with XmlHelper
        public static string GetCarsWithDistance_Helper(CarDealerContext context)
        {
            var carDtos = context.Cars
                .Where(c => c.TraveledDistance > 2000000)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .Select(c => new CarDto_Export()
                {
                    Model = c.Model,
                    Make = c.Make,
                    TraveledDistance = c.TraveledDistance
                })
                .ToArray();

            XmlHelper helper = new();

            return helper.Serialize<CarDto_Export[]>(carDtos, "cars");
        }

        //Query 14. Export Cars With Distance - R
        public static string GetCarsWithDistance_R(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(c => c.TraveledDistance > 2000000)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .Select(c => new CarDto_Export()
                {
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance
                })
                .ToArray();

            XmlRootAttribute root = new XmlRootAttribute("cars");
            XmlSerializer serializer = new XmlSerializer(typeof(CarDto_Export[]), root);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            StringBuilder sb = new();
            StringWriter writer = new StringWriter(sb);

            serializer.Serialize(writer, cars, namespaces);

            return sb.ToString().TrimEnd();
        }

        //Query 14. Export Cars With Distance - with SerializeToXml
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var carDtos = context.Cars
                .Where(c => c.TraveledDistance > 2000000)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .Select(c => new CarDto_Export()
                {
                    Model = c.Model,
                    Make = c.Make,
                    TraveledDistance = c.TraveledDistance
                })
                .ToArray();

            SerializeXml serializeXml = new();

            return serializeXml.SerializeToXml(carDtos, "cars");
        }

        //Query 15. Export Cars from Make BMW
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var bmwDtos = context.Cars
                .Where(c => c.Make == "BMW")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .Select(c => new BmwDto_Export()
                {
                    Model = c.Model,
                    Id = c.Id,
                    TraveledDistance = c.TraveledDistance
                })
                .ToArray();

            var mySerializer = new XmlSerializer(typeof(BmwDto_Export[]), new XmlRootAttribute("cars"));

            StringBuilder sb = new();
            using StringWriter writer = new StringWriter(sb);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            mySerializer.Serialize(writer, bmwDtos, namespaces);

            return sb.ToString().TrimEnd();
        }

        //Query 16. Export Local Suppliers
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => new SupplierDto_export()
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count
                })
                .ToArray();

            var serializer = new XmlSerializer(typeof(SupplierDto_export[]), new XmlRootAttribute("suppliers"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            StringBuilder sb = new();
            using StringWriter writer = new StringWriter(sb);

            serializer.Serialize(writer, suppliers, namespaces);

            return sb.ToString().TrimEnd();
        }

        //Query 17. Export Cars with Their List of Parts
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var carsDtos = context.Cars
                .Select(c => new CarDtoWithPartsExport()
                {
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance,
                    PartDtoExports = c.PartsCars.Select(pc => new PartDtoExport()
                    {
                        Name = pc.Part.Name,
                        Price = pc.Part.Price,
                    })
                        .OrderByDescending(p => p.Price)
                        .ToArray()
                })
                .OrderByDescending(c => c.TraveledDistance)
                .ThenBy(c => c.Model)
                .Take(5)
                .ToArray();

            //var serializer = new XmlHelper();
            //return serializer.Serialize(carsDtos, "cars");
            //var serializer = new SerializeXml();
            //return serializer.SerializeToXml(carsDtos, "cars");

            XmlSerializer serializer = new XmlSerializer(typeof(CarDtoWithPartsExport[]), new XmlRootAttribute("cars"));

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            StringBuilder sb = new();
            using StringWriter writer = new StringWriter(sb);

            serializer.Serialize(writer, carsDtos, namespaces);
            return sb.ToString().TrimEnd();


        }

        //Query 18. Export Total Sales by Customer
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var temp = context.Customers
                .Where(c => c.Sales.Any())
                .Select(c => new
                {
                    Name = c.Name,
                    CarNumber = c.Sales.Count,
                    SalesInfo = c.Sales.Select(s => new
                    {
                        Prices = c.IsYoungDriver
                            ? s.Car.PartsCars.Sum(pc => Math.Round((double)pc.Part.Price * 0.95, 2))
                        //НИКЪДЕ НЕ СТЕ ОПИСАЛИ КОЛКО ОТСТЪПКА ДА СЕ ПРОЛОЖИ!!!!!!!!!!!!
                            : s.Car.PartsCars.Sum(pc => (double)pc.Part.Price)
                    }).ToArray()
                }).ToArray();


            var customers = temp
                .Select(t => new CustomerDtoExport()
                {
                    Name = t.Name,
                    BoughtCars = t.CarNumber,
                    SpentMoney = t.SalesInfo.Sum(g => (decimal)g.Prices)
                })
                .OrderByDescending(t => t.SpentMoney)
                .ToArray();

            XmlHelper xmlHelper = new XmlHelper();

            return xmlHelper.Serialize(customers, "customers");
        }

        //Query 19. Export Sales with Applied Discount
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Select(s => new SaleDtoExport
                {
                    CarDto = new CarSaleDto_Export()
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TraveledDistance = s.Car.TraveledDistance
                    },
                    Discount = (int)s.Discount,
                    CustomerName = s.Customer.Name,
                    Price = s.Car.PartsCars.Sum(pc => pc.Part.Price),
                    PriceWithDiscount = Math.Round((double)(s.Car.PartsCars.Sum(pc => pc.Part.Price) * (1 - s.Discount / 100)), 4)

                }).ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(SaleDtoExport[]), new XmlRootAttribute("sales"));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            StringBuilder sb = new();
            using StringWriter writer = new StringWriter(sb);

            serializer.Serialize(writer, sales, namespaces);

            return sb.ToString().TrimEnd();
            //return SerializeToXml<SaleDtoExport[]>(sales, "sales");

        }


        public static string SerializeToXml<T>(T dto, string xmlRootAttribute)
        {
            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(T), new XmlRootAttribute(xmlRootAttribute));

            StringBuilder stringBuilder = new StringBuilder();

            using (StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture))
            {
                XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
                xmlSerializerNamespaces.Add(string.Empty, string.Empty);

                try
                {
                    xmlSerializer.Serialize(stringWriter, dto, xmlSerializerNamespaces);
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return stringBuilder.ToString();
        }
    }

}
