using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ProductShop.DTO.Product;
using ProductShop.DTO.User;
using ProductShop.DTO.UsersAndProducts;
using ProductShop.Models;
using ProductDTO = ProductShop.DTO.Product.ProductDTO;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            //Product
            this.CreateMap<Product, ProductDTO>()
                .ForMember(x => x.Seller, y
                    => y.MapFrom(s => s.Seller.FirstName + " " + s.Seller.LastName));

            this.CreateMap<Product, SoldProductDTO>()
                .ForMember(x => x.BuyerFirstName, y 
                    => y.MapFrom(s => s.Buyer.FirstName))
                .ForMember(x => x.BuyerLastName, y 
                    => y.MapFrom(s => s.Buyer.LastName));

            //User
            this.CreateMap<User, UserDTO>();

            //UsersAndProducts
            //this.CreateMap<User, UserDto>();
            //this.CreateMap<List<UserDto>, UsersAndProductsDTO>();

            CreateMap<User, UserDto>()
                .ForMember(x => x.SoldProducts, y => y.MapFrom(obj => obj));

            CreateMap<User, SoldProducts>()
                .ForMember(x => x.Products, y => y.MapFrom(obj => obj.ProductsSold.Where(x => x.Buyer != null)));

            CreateMap<Product, ProductsDTO>();

            CreateMap<List<UserDto>, UsersAndProductsDTO>()
                .ForMember(x => x.Users, y => y.MapFrom(obj => obj));
        }
    }
}
