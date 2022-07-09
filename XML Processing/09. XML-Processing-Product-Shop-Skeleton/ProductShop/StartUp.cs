using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ProductShop.Data;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        private static IMapper mapper = new Mapper(new MapperConfiguration(cfg
            => cfg.AddProfile(new ProductShopProfile())));

        public static void Main(string[] args)
        {
            var context = new ProductShopContext();
            //context.Database.EnsureCreated();

            Console.WriteLine(GetUsersWithProducts(context));
        }

        //Problem 1
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(UsersImportDto[]),
                new XmlRootAttribute("Users"));

            UsersImportDto[] usersDtos = (UsersImportDto[])serializer.Deserialize(new StringReader(inputXml));


            User[] users = mapper.Map<User[]>(usersDtos);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Length}";
        }

        //Problem 2
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ProductImportDto[]),
                new XmlRootAttribute("Products"));

            ProductImportDto[] productsDtos = 
                (ProductImportDto[])serializer.Deserialize(new StringReader(inputXml));

            Product[] products = mapper.Map<Product[]>(productsDtos);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Length}";
        }

        //Problem 3
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CategoryInputDto[]), 
                new XmlRootAttribute("Categories"));

            CategoryInputDto[] categoryInputDtos = 
                (CategoryInputDto[])serializer.Deserialize(new StringReader(inputXml));

            Category[] categories = mapper.Map<Category[]>(categoryInputDtos)
                .Where(x => x.Name != null)
                .ToArray();

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Length}";
        }

        //Problem 4
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CategoryProductImportDto[]), 
                new XmlRootAttribute("CategoryProducts"));

            CategoryProductImportDto[] CategoryProductDtos =
                (CategoryProductImportDto[])serializer.Deserialize(new StringReader(inputXml));


            CategoryProductDtos = CategoryProductDtos
                .Where(x => x.CategoryId != null && x.ProductId != null)
                .ToArray();

            CategoryProduct[] categoryProducts = mapper.Map<CategoryProduct[]>(CategoryProductDtos);

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Length}";
        }

        //Problem 5
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context
                .Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(p => new ProductExportInRangeDto
                {
                    Name = p.Name,
                    Price = p.Price,
                    Buyer = p.Buyer.FirstName + " " + p.Buyer.LastName
                })
                .Take(10)
                .ToArray();


            XmlSerializer serializer = new XmlSerializer(typeof(ProductExportInRangeDto[]),
                new XmlRootAttribute("Products"));

            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            serializer.Serialize(new StringWriter(sb), products, namespaces);

            return sb.ToString().TrimEnd();
        }

        //Problem 6
        public static string GetSoldProducts(ProductShopContext context)
        {
            UserSoldProductsDto[] users = context
                .Users
                .Where(u => u.ProductsSold.Any())
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Select(u => new UserSoldProductsDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SoldProductDtos = u.ProductsSold.Select(p => new SoldProductDto
                        {
                            Name = p.Name,
                            Price = p.Price
                        })
                        .ToArray()
                })
                .Take(5)
                .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(UserSoldProductsDto[]),
                new XmlRootAttribute("Users"));

            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            serializer.Serialize(new StringWriter(sb), users, namespaces);

            return sb.ToString().TrimEnd();
        }

        //Problem 7
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            CategoryDtoModel[] categories = context
                .Categories
                .Select(c => new CategoryDtoModel
                {
                    Name = c.Name,
                    Count = c.CategoryProducts.Count,
                    AveragePrice = c.CategoryProducts.Average(p => p.Product.Price),
                    TotalRevenue = c.CategoryProducts.Sum(p => p.Product.Price)
                })
                .OrderByDescending(c => c.Count)
                .ThenBy(c => c.TotalRevenue)
                .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(CategoryDtoModel[]),
                new XmlRootAttribute("Category"));

            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            serializer.Serialize(new StringWriter(sb), categories, namespaces);

            return sb.ToString().TrimEnd();
        }

        //Problem 8
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            UserProductDto[] userProducts = context
                .Users
                .Include(x => x.ProductsSold)
                .ToArray()
                .Where(u => u.ProductsSold.Any())
                .OrderByDescending(u => u.ProductsSold.Count)
                .Select(u => new UserProductDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProduct = new SoldUserProductDto
                    {
                        Count = u.ProductsSold.Count,
                        Products = u.ProductsSold.Select(p => new ProductDto
                        {
                            Name = p.Name,
                            Price = p.Price
                        })
                            .OrderByDescending(p => p.Price)
                            .ToArray()

                    }
                })
                .Take(10)
                .ToArray();

            var fullModel = new UserProductsDtoModel
            {
                Count = context.Users.Count(x => x.ProductsSold.Any()),
                Users = userProducts
            };

            XmlSerializer serializer = new XmlSerializer(typeof(UserProductsDtoModel), new XmlRootAttribute());

            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            serializer.Serialize(new StringWriter(sb), fullModel, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}