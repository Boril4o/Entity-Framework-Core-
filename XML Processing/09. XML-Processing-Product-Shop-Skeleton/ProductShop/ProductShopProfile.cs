using AutoMapper;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            //Import
            this.CreateMap<UsersImportDto, User>();

            this.CreateMap<ProductImportDto, Product>();

            this.CreateMap<CategoryInputDto, Category>();

            this.CreateMap<CategoryProductImportDto, CategoryProduct>();

            //Export
            //Product
            this.CreateMap<Product, ProductExportInRangeDto>()
                .ForMember(x => x.Name, y
                    => y.MapFrom(s =>s.Buyer.FirstName + " " + s.Buyer.LastName));

            this.CreateMap<User, UserSoldProductsDto>()
                .ForMember(x => x.SoldProductDtos, y 
                    => y.MapFrom(s => s.ProductsSold));

        }
    }
}
