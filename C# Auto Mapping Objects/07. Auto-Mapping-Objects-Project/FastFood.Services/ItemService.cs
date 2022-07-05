using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FastFood.Data;
using FastFood.Models;
using FastFood.Services.DTO.Item;
using FastFood.Services.Interfaces;

namespace FastFood.Services
{
    public class ItemService : IItemsService
    {
        private readonly IMapper mapper;
        private readonly FastFoodContext context;

        public ItemService(IMapper mapper, FastFoodContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public void Create(CreateItemDTO dto)
        {
            Item item = this.mapper.Map<Item>(dto);

            context.Add(item);
            context.SaveChanges();
        }

        public ICollection<ListAllitemsDTO> All()
            => this
                .context
                .Items
                .ProjectTo<ListAllitemsDTO>(this.mapper.ConfigurationProvider)
                .ToList();
    }
}
