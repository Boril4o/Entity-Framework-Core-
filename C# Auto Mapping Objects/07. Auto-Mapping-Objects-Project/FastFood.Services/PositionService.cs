using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FastFood.Data;
using FastFood.Services.DTO.Position;
using FastFood.Services.Interfaces;

namespace FastFood.Services
{
    public class PositionService : IPositionService
    {
        private readonly FastFoodContext context;
        private readonly IMapper mapper;

        public PositionService(FastFoodContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public ICollection<EmployeeRegisterPositionsAvailable> GetPositionsAvailable()
        {
            return this.context
                .Positions
                .ProjectTo<EmployeeRegisterPositionsAvailable>(this.mapper.ConfigurationProvider)
                .ToList();
        }
    }
}
