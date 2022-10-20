using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using Core.Entities.OrderAggregate;

namespace API.Helpers
{
  public class MappingProfiles : Profile
  {
    public MappingProfiles()
    {
      CreateMap<Product, ProductToReturnDto>()
      .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
      .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
      .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());
      CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();
      CreateMap<CustomerBasketDto, CustomerBasket>();
      CreateMap<BasketItemDto, BasketItem>();
      CreateMap<AddressDto, Core.Entities.OrderAggregate.Address>();
      CreateMap<Order, OrderToReturnDto>()
      .ForMember(d => d.ShipingPrice, o => o.MapFrom(o => o.DeliveryMethod.Price))
      .ForMember(d => d.DeliveryMethod, o => o.MapFrom(o => o.DeliveryMethod.ShortName))
      ;
      CreateMap<OrderItem, OrderItemDto>()
      .ForMember(d => d.ProductId, o => o.MapFrom(o => o.ItemOrdered.ProductItemId))
      .ForMember(d => d.ProductName, o => o.MapFrom(o => o.ItemOrdered.ProductName))
      .ForMember(d => d.PictureUrl, o => o.MapFrom(o => o.ItemOrdered.PictureUrl))
      .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemUrlResolver>());
      ;
    }
  }
}