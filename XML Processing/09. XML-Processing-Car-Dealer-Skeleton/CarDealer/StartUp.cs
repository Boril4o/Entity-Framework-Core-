using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Serialization;
using AutoMapper;
using CarDealer.Data;
using CarDealer.Dtos01.Export;
using CarDealer.Dtos01.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;

namespace CarDealer
{
    public class StartUp
    {
        private static IMapper mapper = new Mapper(new MapperConfiguration(cfg
            => cfg.AddProfile(new CarDealerProfile())));

        public static void Main(string[] args)
        {
            var context = new CarDealerContext();
            //context.Database.EnsureCreated();

            Console.WriteLine(GetSalesWithAppliedDiscount(context));
        }

        //Problem 9
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportSupplierDto[]),
                new XmlRootAttribute("Suppliers"));

            var suppliersDtos = (ImportSupplierDto[])serializer.Deserialize(new StringReader(inputXml));

            Supplier[] suppliers = mapper.Map<Supplier[]>(suppliersDtos);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Length}";
        }

        //Problem 10
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportPartDto[]),
                new XmlRootAttribute("Parts"));

            ImportPartDto[] partsDtos =
                (ImportPartDto[])serializer.Deserialize(new StringReader(inputXml));

            partsDtos = partsDtos
                .Where(p
                    => context.Suppliers
                        .Select(s => s.Id)
                        .Contains(p.SupplierId))
                .ToArray();

            Part[] parts = mapper.Map<Part[]>(partsDtos);

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Length}";
        }

        //Problem 11
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportCarDto[]),
                new XmlRootAttribute("Cars"));

            ImportCarDto[] carDtos = 
                (ImportCarDto[])serializer.Deserialize(new StringReader(inputXml));

            List<Car> cars = new List<Car>();
            foreach (var carDto in carDtos)
            {
                Car c = new Car()
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TravelledDistance = carDto.TraveledDistance
                };

                List<PartCar> partCars = new List<PartCar>();

                foreach (var partId in carDto.PartsDtos.Select(p => p.Id).Distinct())
                {
                    Part part = context
                        .Parts
                        .Find(partId);

                    if (part == null) continue;

                    PartCar partCar = new PartCar()
                    {
                        Car = c,
                        Part = part
                    };

                    partCars.Add(partCar);
                }
                
                c.PartCars = partCars;
                cars.Add(c);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }

        //Problem 12
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<ImportCustomerDto>),
                new XmlRootAttribute("Customers"));

            var customerDtos = 
                (List<ImportCustomerDto>)serializer.Deserialize(new StringReader(inputXml));

            List<Customer> customers = mapper.Map<List<Customer>>(customerDtos);

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count}";
        }

        //Problem 13
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer =
                new XmlSerializer(typeof(List<ImportSalesDto>), new XmlRootAttribute("Sales"));

            var customersDtos = (List<ImportSalesDto>)serializer.Deserialize(new StringReader(inputXml));

            List<Sale> sales = mapper.Map<List<Sale>>(customersDtos)
                .Where(c => context.Cars.Select(x => x.Id).Contains(c.CarId))
                .ToList();

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}";
        }

        //Problem 14
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            CarDistanceExportModel[] cars = context
                .Cars
                .Where(c => c.TravelledDistance > 2_000_000)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .Select(c => new CarDistanceExportModel()
                {
                    Model = c.Model,
                    Make = c.Make,
                    TravelledDistance = c.TravelledDistance
                })
                .ToArray();

            var serializer = new XmlSerializer(typeof(CarDistanceExportModel[]), new XmlRootAttribute("Cars"));

            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            serializer.Serialize(new StringWriter(sb), cars,  namespaces);

            return sb.ToString().TrimEnd();
        }

        //Problem 15
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            BmwCarExportModel[] cars = context
                .Cars
                .Where(c => c.Make == "BMW")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .Select(c => new BmwCarExportModel()
                {
                    Id = c.Id,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .ToArray();

            var serializer = new XmlSerializer(typeof(BmwCarExportModel[]), new XmlRootAttribute("cars"));
            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            serializer.Serialize(new StringWriter(sb), cars, namespaces);

            return sb.ToString().TrimEnd();
        }

        //Problem 16
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context
                .Suppliers
                .Where(s => !s.IsImporter)
                .Select(s => new LocalSupplierModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count
                })
                .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(LocalSupplierModel[]),
                new XmlRootAttribute("suppliers"));

            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            serializer.Serialize(new StringWriter(sb), suppliers, namespaces);

            return sb.ToString().TrimEnd();
        }

        //Problem 17
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context
                .Cars
                .Select(c => new CarWithPartsModel
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance,
                    Parts = c.PartCars.Select(p => new CarWithPartsModel.CarPartDto
                        {
                            Name = p.Part.Name,
                            Price = p.Part.Price
                        })
                        .OrderByDescending(p => p.Price)
                        .ToArray()
                })
                .OrderByDescending(c => c.TravelledDistance)
                .ThenBy(c => c.Model)
                .Take(5)
                .ToArray();

            var serializer = new XmlSerializer(typeof(CarWithPartsModel[]), new XmlRootAttribute("cars"));

            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            serializer.Serialize(new StringWriter(sb), cars, namespaces);

            return sb.ToString().TrimEnd();
        }

        //Problem 18
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context
                .Customers
                .ToArray()
                .Where(c => c.Sales.Any())
                .Select(c => new TotalSalesExportModel()
                {
                    BoughtCars = c.Sales.Count,
                    FullName = c.Name,
                    SpentMoney = c.Sales.Sum(s => s.Car.PartCars.Sum(pc => pc.Part.Price))
                })
                .OrderByDescending(c => c.SpentMoney)
                .ToArray();

            var serializer =
                new XmlSerializer(typeof(TotalSalesExportModel[]), new XmlRootAttribute("customers"));

            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            serializer.Serialize(new StringWriter(sb), customers, namespaces);

            return sb.ToString().TrimEnd();
        }

        //Problem 19
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context
                .Sales
                .Select(s => new SalesExportModel
                {
                    Car = new CarSalesDto
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TravelledDistance

                    },
                    CustomerName = s.Customer.Name,
                    Discount = s.Discount,
                  
                    Price = s.Car.PartCars.Sum(p => p.Part.Price),
                    PriceWithDiscount = (s.Car.PartCars.Sum(p => p.Part.Price)
                                         - s.Car.PartCars.Sum(p => p.Part.Price) * s.Discount / 100),
                })
                .ToArray();

            var serializer = new XmlSerializer(typeof(SalesExportModel[]), new XmlRootAttribute("sales"));

            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            serializer.Serialize(new StringWriter(sb), sales, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}