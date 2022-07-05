using System.Collections.Generic;
using System.Linq;
using FastFood.Services.DTO.Category;
using FastFood.Services.Interfaces;

namespace FastFood.Core.Controllers
{
    using System;
    using AutoMapper;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels.Categories;

    public class CategoriesController : Controller
    {
        private readonly IMapper mapper;
        private readonly ICategoryService service;

        public CategoriesController(ICategoryService service, IMapper mapper)
        {
            this.mapper = mapper;
            this.service = service;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateCategoryInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.RedirectToAction("Create");
            }

            CreateCategoryDTO categoryDTO = this.mapper.Map<CreateCategoryDTO>(model);

            this.service.Create(categoryDTO);

            return this.RedirectToAction("All");
        }

        public IActionResult All()
        {
            ICollection<ListAllCategoriesDTO> categoriesDtos = this.service.All();

            var categoryViewModels = this.mapper.Map<ICollection<ListAllCategoriesDTO>,
                ICollection<CategoryAllViewModel>>(categoriesDtos).ToList();

            return this.View("All", categoryViewModels);
        }
    }
}
