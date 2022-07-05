using System;
using System.Collections.Generic;
using System.Text;
using FastFood.Services.DTO.Position;

namespace FastFood.Services.Interfaces
{
    public interface IPositionService
    {
        ICollection<EmployeeRegisterPositionsAvailable> GetPositionsAvailable();
    }
}
