using FoodOrdering.MAUI.Services;
using FoodOrdering.MAUI.ViewModels;
using FoodOrdering.MAUI.Views;

namespace FoodOrdering.MAUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        // Services
        builder.Services.AddSingleton<ITokenService, TokenService>();
        builder.Services.AddSingleton<ICartService, CartService>();
        builder.Services.AddHttpClient<IApiService, ApiService>();

        // ViewModels
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<MenuViewModel>();
        builder.Services.AddTransient<CartViewModel>();
        builder.Services.AddTransient<OrdersViewModel>();
        builder.Services.AddTransient<VendorOrdersViewModel>();

        // Views
        builder.Services.AddTransient<LoginPage>();
        //builder.Services.AddTransient<MenuPage>();
        //builder.Services.AddTransient<CartPage>();
        //builder.Services.AddTransient<OrdersPage>();
        //builder.Services.AddTransient<VendorOrdersPage>();

        return builder.Build();
    }
}