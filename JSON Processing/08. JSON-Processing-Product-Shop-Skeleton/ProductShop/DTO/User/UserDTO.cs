using System;
using System.Collections.Generic;
using System.Text;
using ProductShop.DTO.Product;

namespace ProductShop.DTO.User
{
    public class UserDTO
    {
        public UserDTO()
        {
            SoldProducts = new List<SoldProductDTO>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IEnumerable<SoldProductDTO> SoldProducts { get; set; }
    }
}
