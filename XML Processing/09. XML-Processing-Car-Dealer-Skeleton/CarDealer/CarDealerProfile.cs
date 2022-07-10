using System;
using AutoMapper;
using CarDealer.Dtos01.Import;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<ImportSupplierDto, Supplier>();

            this.CreateMap<ImportPartDto, Part>();

            this.CreateMap<PartIdDto, PartCar>()
                .ForMember(x => x.CarId, y
                    => y.MapFrom(s => s.Id));

            this.CreateMap<ImportCustomerDto, Customer>();

            this.CreateMap<ImportSalesDto, Sale>();
        }
    }
}
