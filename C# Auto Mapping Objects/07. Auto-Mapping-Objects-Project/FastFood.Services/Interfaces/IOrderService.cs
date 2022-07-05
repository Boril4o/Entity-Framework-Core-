using System;
using System.Collections.Generic;
using System.Text;
using FastFood.Services.DTO.Order;

namespace FastFood.Services.Interfaces
{
    public interface IOrderService
    {
        void Create(CreateOrderDTO dto);

        ICollection<ListAllOrdersDTO> All();

        CreateOrderViewModelDTO GetItemsAndEmployees();
    }
}
