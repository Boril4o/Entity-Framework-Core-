using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop.DTO.Product
{
    public class SoldProductDTO
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string BuyerFirstName { get; set; }

        public string BuyerLastName { get; set; }
    }
}
