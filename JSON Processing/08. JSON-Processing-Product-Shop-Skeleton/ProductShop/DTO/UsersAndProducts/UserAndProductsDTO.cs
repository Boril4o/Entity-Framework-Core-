using System;
using System.Collections.Generic;
using System.Text;
using ProductShop.DTO.Product;

namespace ProductShop.DTO.UsersAndProducts
{
    public class UserDto
    {
        public string FistName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public SoldProducts SoldProducts { get; set; }
    }

    public class SoldProducts
    {
        public SoldProducts()
        {
            Products = new List<ProductDTO>();
        }
        public int Count { get; set; }

        public IEnumerable<ProductDTO> Products { get; set; }
    }

    public class ProductsDTO
    {
        public string Name { get; set; }

        public decimal Price { get; set; }
    }

    public class UsersAndProductsDTO
    {
        public UsersAndProductsDTO()
        {
            this.Users = new List<UserDto>();
        }

        public int UsersCount { get => this.Users.Count; }

        public List<UserDto> Users { get; set; }
    }
}
