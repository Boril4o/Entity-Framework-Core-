using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.DTO;
using ProductShop.DTO.Product;
using ProductShop.DTO.UsersAndProducts;
using ProductShop.Models;
using ProductDTO = ProductShop.DTO.Product.ProductDTO;
using UserDTO = ProductShop.DTO.User.UserDTO;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg
                => cfg.AddProfile(new ProductShopProfile()));

            var context = new ProductShopContext();
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();
            // string inputJson = File.ReadAllText("../../../Datasets/categories-products.json");

            Console.WriteLine(GetUsersWithProducts(context));
        }

        //Problem 1
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            List<User> usersToImprot = JsonConvert.DeserializeObject<List<User>>(inputJson);

            context.Users.AddRange(usersToImprot);
            context.SaveChanges();

            return $"Successfully imported {usersToImprot.Count}";

        }

        //Problem 2
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            List<Product> productsToImprot = JsonConvert.DeserializeObject<List<Product>>(inputJson);

            context.Products.AddRange(productsToImprot);
            context.SaveChanges();

            return $"Successfully imported {productsToImprot.Count}";
        }

        //Problem 3
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            List<Category> categoriesToImport = JsonConvert.DeserializeObject<List<Category>>(inputJson)
                .Where(u => u.Name != null)
                .ToList();

            context.Categories.AddRange(categoriesToImport);
            context.SaveChanges();

            return $"Successfully imported {categoriesToImport.Count}";
        }

        //Problem 4
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            List<CategoryProduct> categoriesProductsToImport =
                JsonConvert.DeserializeObject<List<CategoryProduct>>(inputJson);

            context.CategoryProducts.AddRange(categoriesProductsToImport);
            context.SaveChanges();

            return $"Successfully imported {categoriesProductsToImport.Count}";
        }

        //Problem 5
        public static string GetProductsInRange(ProductShopContext context)
        {
            IEnumerable<ProductDTO> products = context
                .Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .Select(p => new ProductDTO
                {
                    Name = p.Name,
                    Price = p.Price,
                    Seller = string.Concat(p.Seller.FirstName, ' ', p.Seller.LastName)
                })
                .OrderBy(p => p.Price)
                .ToArray();

            DefaultContractResolver contractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = contractResolver
            };

            string json = JsonConvert.SerializeObject(products, settings);
            return json;
        }

        //Problem 6
        public static string GetSoldProducts(ProductShopContext context)
        {
            IEnumerable<UserDTO> sellers = context
                .Users
                .Where(u => u.ProductsSold.Count >= 1)
                .Select(x => new UserDTO
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    SoldProducts = x.ProductsSold.Where(ps => ps.Buyer != null).Select(ps => new SoldProductDTO
                    {
                        Name = ps.Name,
                        Price = ps.Price,
                        BuyerFirstName = ps.Buyer.FirstName,
                        BuyerLastName = ps.Buyer.LastName
                    })
                })
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToArray();

            DefaultContractResolver contractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = contractResolver,
                NullValueHandling = NullValueHandling.Ignore
            };

            return JsonConvert.SerializeObject(sellers, settings);
        }

        //Problem 7
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context
                .Categories
                .Select(c => new
                {
                    Category = c.Name,
                    ProductsCount = c.CategoryProducts.Count(),
                    AveragePrice = c.CategoryProducts.Average(p => p.Product.Price)
                        .ToString("F2"),
                    TotalRevenue = c.CategoryProducts.Sum(p => p.Product.Price)
                        .ToString("F2")
                })
                .OrderByDescending(c => c.ProductsCount)
                .ToArray();

            var contractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = contractResolver
            };

            string json = JsonConvert.SerializeObject(categories, settings);

            return json;
        }

        //Problem 8
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context
                .Users
                .Include(x => x.ProductsSold)
                .ToArray()
                .Where(u => u.ProductsSold.Any(ps => ps.BuyerId != null))
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts = new
                    {
                        count = u.ProductsSold.Count(p => p.BuyerId != null),
                        products = u.ProductsSold
                            .Where(ps => ps.BuyerId != null)
                            .Select(ps => new
                            {
                                name = ps.Name,
                                price = ps.Price
                            })
                    }
                })
                .OrderByDescending(u => u.soldProducts.products.Count())
                .ToArray();

            var resultObjects = new
            {
                usersCount = users.Length,
                users
            };

            var contractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var setting = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            };


            string json = JsonConvert.SerializeObject(resultObjects, setting);

            return json;
        }
    }
}
