using FastFood.Core.ViewModels.Categories;
using FastFood.Core.ViewModels.Employees;
using FastFood.Core.ViewModels.Items;
using FastFood.Core.ViewModels.Orders;
using FastFood.Services.DTO.Category;
using FastFood.Services.DTO.Employee;
using FastFood.Services.DTO.Item;
using FastFood.Services.DTO.Order;
using FastFood.Services.DTO.Position;

namespace FastFood.Core.MappingConfiguration
{
    using AutoMapper;
    using FastFood.Models;
    using ViewModels.Positions;

    public class FastFoodProfile : Profile
    {
        public FastFoodProfile()
        {
            //Positions
            this.CreateMap<CreatePositionInputModel, Position>()
                .ForMember(x => x.Name, y 
                    => y.MapFrom(s => s.PositionName));

            this.CreateMap<Position, PositionsAllViewModel>()
                .ForMember(x => x.Name, y 
                    => y.MapFrom(s => s.Name));

            this.CreateMap<Position, EmployeeRegisterPositionsAvailable>()
                .ForMember(x => x.PositionId, y
                    => y.MapFrom(s => s.Id));

            //Category
            this.CreateMap<CreateCategoryInputModel, CreateCategoryDTO>();

            this.CreateMap<ListAllCategoriesDTO, CategoryAllViewModel>();

            this.CreateMap<CreateCategoryDTO, Category>()
                .ForMember(x => x.Name, y
                    => y.MapFrom(s => s.CategoryName));

            this.CreateMap<Category, ListAllCategoriesDTO>();

            //Employee
            this.CreateMap<EmployeeRegisterPositionsAvailable, RegisterEmployeeViewModel>();

            this.CreateMap<RegisterEmployeeInputModel, RegisterEmployeeDTO>();

            this.CreateMap<ListAllEmployeesDTO, EmployeesAllViewModel>();

            this.CreateMap<RegisterEmployeeDTO, Employee>();

            this.CreateMap<Employee, ListAllEmployeesDTO>();

            //Item
            this.CreateMap<CreateItemViewModel, Category>()
                .ForMember(x => x.Id, y
                    => y.MapFrom(s => s.CategoryId));

            this.CreateMap<CategoriesAvailable, CreateItemViewModel>();

            this.CreateMap<Category, CategoriesAvailable>()
                .ForMember(x => x.CategoryId, y 
                    => y.MapFrom(s => s.Id));

            this.CreateMap<CreateItemInputModel, CreateItemDTO>();

            this.CreateMap<ListAllitemsDTO, ItemsAllViewModels>();

            this.CreateMap<CreateItemDTO, Item>();

            this.CreateMap<Item, ListAllitemsDTO>()
                .ForMember(x => x.Category, y 
                    => y.MapFrom(s => s.Category.Name));

            //Order
            this.CreateMap<CreateOrderViewModelDTO, CreateOrderViewModel>();

            this.CreateMap<CreateOrderInputModel, CreateOrderDTO>();

            this.CreateMap<ListAllOrdersDTO, OrderAllViewModel>();

            this.CreateMap<CreateOrderDTO, Order>();

            this.CreateMap<Order, ListAllOrdersDTO>()
                .ForMember(x => x.Employee, y
                    => y.MapFrom(s => s.Employee.Name))
                .ForMember(x => x.OrderId, y 
                    => y.MapFrom(s => s.Id));
        }
            
    }
}
