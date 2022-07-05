using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FastFood.Data;
using FastFood.Models;
using FastFood.Services.DTO.Order;
using FastFood.Services.Interfaces;

namespace FastFood.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper mapper;

        private readonly FastFoodContext context;

        public OrderService(IMapper mapper, FastFoodContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public void Create(CreateOrderDTO dto)
        {
            Order order = this.mapper.Map<Order>(dto);
            order.DateTime = DateTime.Now;
            context.Orders.Add(order);
            context.SaveChanges();
        }

        public ICollection<ListAllOrdersDTO> All()
            => this
                .context
                .Orders
                .ProjectTo<ListAllOrdersDTO>(this.mapper.ConfigurationProvider)
                .ToList();

        public CreateOrderViewModelDTO GetItemsAndEmployees()
        {
            var viewOrder = new CreateOrderViewModelDTO
            {
                Items = this.context.Items.Select(x => x.Id).ToList(),
                Employees = this.context.Employees.Select(x => x.Id).ToList(),
            };

            return viewOrder;
        }
    }
}
