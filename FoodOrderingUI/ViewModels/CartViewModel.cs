using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodOrderingUI.Models;
using FoodOrderingUI.Services;
using System.Collections.ObjectModel;

namespace FoodOrderingUI.ViewModels
{
    public partial class CartViewModel : BaseViewModel
    {
        private readonly ICartService _cartService;
        private readonly IApiService _apiService;

        [ObservableProperty]
        ObservableCollection<CartItem> cartItems = new();

        [ObservableProperty]
        decimal totalAmount;

        [ObservableProperty]
        string specialInstructions = string.Empty;

        [ObservableProperty]
        bool hasItems;

        public CartViewModel(ICartService cartService, IApiService apiService)
        {
            _cartService = cartService;
            _apiService = apiService;
            Title = "Cart";

            _cartService.CartUpdated += OnCartUpdated;
        }

        private void OnCartUpdated(object? sender, EventArgs e)
        {
            LoadCart();
        }

        [RelayCommand]
        void LoadCart()
        {
            CartItems.Clear();
            var items = _cartService.GetCartItems();

            foreach (var item in items)
            {
                CartItems.Add(item);
            }

            TotalAmount = _cartService.GetTotal();
            HasItems = CartItems.Count > 0;
        }

        [RelayCommand]
        void UpdateQuantity(CartItem cartItem)
        {
            if (cartItem.Quantity <= 0)
            {
                _cartService.RemoveItem(cartItem.MenuItem.Id);
            }
            else
            {
                _cartService.UpdateQuantity(cartItem.MenuItem.Id, cartItem.Quantity);
            }
        }

        [RelayCommand]
        void RemoveItem(CartItem cartItem)
        {
            _cartService.RemoveItem(cartItem.MenuItem.Id);
        }

        [RelayCommand]
        void ClearCart()
        {
            _cartService.ClearCart();
        }

        [RelayCommand]
        async Task PlaceOrderAsync()
        {
            if (IsBusy) return;
            if (!HasItems) return;

            try
            {
                IsBusy = true;

                var orderRequest = new CreateOrderRequest
                {
                    VendorId = _cartService.GetVendorId(),
                    SpecialInstructions = SpecialInstructions,
                    Items = CartItems.Select(x => new CreateOrderItem
                    {
                        MenuItemId = x.MenuItem.Id,
                        Quantity = x.Quantity,
                        Notes = x.Notes
                    }).ToList()
                };

                var order = await _apiService.CreateOrderAsync(orderRequest);
                _cartService.ClearCart();

                await Shell.Current.DisplayAlert("Success", $"Order placed successfully! Order ID: {order.Id}", "OK");
                await Shell.Current.GoToAsync("//orders");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }

}
