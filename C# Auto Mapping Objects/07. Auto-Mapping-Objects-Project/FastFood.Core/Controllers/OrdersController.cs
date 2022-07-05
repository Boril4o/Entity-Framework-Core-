using System.Collections.Generic;
using FastFood.Services;
using FastFood.Services.DTO.Order;
using FastFood.Services.Interfaces;

namespace FastFood.Core.Controllers
{
    using System;
    using System.Linq;
    using AutoMapper;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels.Orders;

    public class OrdersController : Controller
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            this.orderService = orderService;
            this.mapper = mapper;
        }

        public IActionResult Create()
        {
            CreateOrderViewModelDTO crtViewModelDto = orderService.GetItemsAndEmployees();
            CreateOrderViewModel viewOrder = this.mapper.Map<CreateOrderViewModel>(crtViewModelDto);

            return this.View(viewOrder);
        }

        [HttpPost]
        public IActionResult Create(CreateOrderInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.RedirectToAction("Create");
            }

            CreateOrderDTO crtOrderDto = this.mapper.Map<CreateOrderDTO>(model);
            orderService.Create(crtOrderDto);

            return this.RedirectToAction("All");
        }

        public IActionResult All()
        {
            ICollection<ListAllOrdersDTO> allOrdersDtos = orderService.All();

            List<OrderAllViewModel> orderAllViewModels = this.mapper.Map<ICollection<ListAllOrdersDTO>,
                ICollection<OrderAllViewModel>>(allOrdersDtos)
                .ToList();

            return this.View(orderAllViewModels);
        }
    }
}
