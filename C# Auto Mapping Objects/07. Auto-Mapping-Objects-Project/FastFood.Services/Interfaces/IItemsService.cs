using System;
using System.Collections.Generic;
using System.Text;
using FastFood.Services.DTO.Item;

namespace FastFood.Services.Interfaces
{
    public interface IItemsService
    {
        void Create(CreateItemDTO dto);

        ICollection<ListAllitemsDTO> All();
    }
}
