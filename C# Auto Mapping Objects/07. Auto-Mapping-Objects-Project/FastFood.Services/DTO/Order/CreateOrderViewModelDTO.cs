using System;
using System.Collections.Generic;
using System.Text;

namespace FastFood.Services.DTO.Order
{
    public class CreateOrderViewModelDTO
    {
        public List<int> Items { get; set; }

        public List<int> Employees { get; set; }
    }
}
