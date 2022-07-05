using System;
using System.Collections.Generic;
using System.Text;

namespace FastFood.Services.DTO.Order
{
    public class ListAllOrdersDTO
    {
        public int OrderId { get; set; }

        public string Customer { get; set; }

        public string Employee { get; set; }

        public string DateTime { get; set; }
    }
}
