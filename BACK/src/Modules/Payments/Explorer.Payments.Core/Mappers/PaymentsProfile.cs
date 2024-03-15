using AutoMapper;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.Core.Domain;

namespace Explorer.Payments.Core.Mappers;

public class PaymentsProfile : Profile
{
    public PaymentsProfile()
    {
        CreateMap<OrderItemDto, OrderItem>().ReverseMap();
        CreateMap<ShoppingCartDto, ShoppingCart>().ReverseMap();
        CreateMap<TourPurchaseTokenDto, TourPurchaseToken>().ReverseMap();
        CreateMap<BundlePriceDto, BundlePrice>().ReverseMap();
        CreateMap<WalletDto, Wallet>().ReverseMap();
        CreateMap<PaymentRecordDto, PaymentRecord>().ReverseMap();
        CreateMap<WishListItemDto, WishListItem>().ReverseMap();
        CreateMap<WishListDto, WishList>().ReverseMap();
        CreateMap<DiscountDto, Discount>().ReverseMap();
        CreateMap<TourDiscountDto, TourDiscount>().ReverseMap();
        CreateMap<CouponDto, Coupon>().ReverseMap();
    }
}