using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FastFood.Data;
using FastFood.Models;
using FastFood.Services.DTO.Category;
using FastFood.Services.Interfaces;

namespace FastFood.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly FastFoodContext context;
        private readonly IMapper mapper;

        public CategoryService(FastFoodContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public void Create(CreateCategoryDTO dto)
        {
            Category category = this.mapper.Map<Category>(dto);

            this.context.Categories.Add(category);
            context.SaveChanges();
        }

        public ICollection<ListAllCategoriesDTO> All()
            => this.context
                .Categories
                .ProjectTo<ListAllCategoriesDTO>(this.mapper.ConfigurationProvider)
                .ToList();

        public ICollection<CategoriesAvailable> GetPositionsAvailable()
            => this
                .context
                .Categories
                .ProjectTo<CategoriesAvailable>(this.mapper.ConfigurationProvider)
                .ToList();
    }
}
