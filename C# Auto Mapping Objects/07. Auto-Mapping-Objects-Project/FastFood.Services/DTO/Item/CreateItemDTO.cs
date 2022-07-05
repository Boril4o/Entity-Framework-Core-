using System;
using System.Collections.Generic;
using System.Text;

namespace FastFood.Services.DTO.Item
{
    public class CreateItemDTO
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public int CategoryId { get; set; }
    }
}
