using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using AutoMapper;
using CarDealer.Data;
using CarDealer.Dtos01.Import;
using CarDealer.Models;

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

            string inputXml = File.ReadAllText("Datasets/parts.xml");
            Console.WriteLine(ImportCars(context, inputXml));
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
            return "";
        }
    }
}