using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Payments.API.Public;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Explorer.Payments.Core.Mappers;
using Explorer.Payments.Core.UseCases;
using Explorer.Payments.Infrastructure.Database;
using Explorer.Payments.Infrastructure.Database.Repositories;
using Explorer.Stakeholders.API.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Explorer.Payments.Infrastructure;

public static class PaymentsStartup
{
    public static IServiceCollection ConfigurePaymentsModule(this IServiceCollection services)
    {
        // Registers all profiles since it works on the assembly
        services.AddAutoMapper(typeof(PaymentsProfile).Assembly);
        SetupCore(services);
        SetupInfrastructure(services);
        return services;
    }

    private static void SetupCore(IServiceCollection services)
    {
        services.AddScoped<IOrderItemService, OrderItemService>();
        services.AddScoped<IShoppingCartService, ShoppingCartService>();
        services.AddScoped<ITourPurchaseTokenService, TourPurchaseTokenService>();
        services.AddScoped<IBundlePriceService, BundlePriceService>();
        services.AddScoped<IWalletService, WalletService>();
        services.AddScoped<IPaymentRecordService, PaymentRecordService>();
        services.AddScoped<IWishListItemService, WishListItemService>();
        services.AddScoped<IWishListService, WishListService>();
        services.AddScoped<IDiscountService, DiscountService>();
        services.AddScoped<ITourDiscountService, TourDiscountService>();
        services.AddScoped<ICouponService, CouponService>();
    }

    private static void SetupInfrastructure(IServiceCollection services)
    {
        services.AddScoped(typeof(IOrderItemRepository), typeof(OrderItemRepository));
        services.AddScoped(typeof(IShoppingCartRepository), typeof(ShoppingCartRepository));
        services.AddScoped(typeof(ITourPurchaseTokenRepository), typeof(TourPurchaseTokenRepository));
        services.AddScoped(typeof(IBundlePriceRepository), typeof(BundlePriceRepository));
        services.AddScoped(typeof(IWalletRepository), typeof(WalletRepository));
        services.AddScoped(typeof(IPaymentRecordRepository), typeof(PaymentRecordRepository));
        services.AddScoped(typeof(IWishListItemRepository), typeof(WishListItemRepository));
        services.AddScoped(typeof(IWishListRepository), typeof(WishListRepository));
        services.AddScoped(typeof(IDiscountRepository), typeof(DiscountRepository));
        services.AddScoped(typeof(ITourDiscountRepository), typeof(TourDiscountRepository));
        services.AddScoped(typeof(ICouponRepository), typeof(CouponRepository));

        services.AddDbContext<PaymentsContext>(opt =>
            opt.UseNpgsql(DbConnectionStringBuilder.Build("payments"),
                x => x.MigrationsHistoryTable("__EFMigrationsHistory", "payments")));
    }
}