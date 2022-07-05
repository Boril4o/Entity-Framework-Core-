using System.Collections.Generic;
using FastFood.Services.DTO.Category;
using FastFood.Services.DTO.Item;
using FastFood.Services.Interfaces;

namespace FastFood.Core.Controllers
{
    using System;
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels.Items;

    public class ItemsController : Controller
    {
        private readonly IItemsService itemsService;
        private readonly IMapper mapper;
        private readonly ICategoryService categoryService;

        public ItemsController(IItemsService itemsService, IMapper mapper, ICategoryService categoryService)
        {
            this.itemsService = itemsService;
            this.mapper = mapper;
            this.categoryService = categoryService;
        }

        public IActionResult Create()
        {
            ICollection<CategoriesAvailable> categoriesAvailable
                = this.categoryService.GetPositionsAvailable();

            IList<CreateItemViewModel> categories = this.mapper
                .Map<ICollection<CategoriesAvailable>, ICollection<CreateItemViewModel>>(categoriesAvailable)
                .ToList();

            return this.View(categories);
        }

        [HttpPost]
        public IActionResult Create(CreateItemInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.RedirectToAction("Create");
            }

            CreateItemDTO crtItemDto 
                = this.mapper.Map<CreateItemDTO>(model);

            itemsService.Create(crtItemDto);

            return this.RedirectToAction("All");
        }

        public IActionResult All()
        {
            ICollection<ListAllitemsDTO> listAllitemsDtos = itemsService.All();

            List<ItemsAllViewModels> itemsAllViewModels = this.mapper.Map<ICollection<ListAllitemsDTO>,
                    ICollection<ItemsAllViewModels>>(listAllitemsDtos)
                .ToList();

            return this.View(itemsAllViewModels);
        }
    }
}
