using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using FoodOrdering.MAUI.Models;
using FoodOrdering.MAUI.Services;

namespace FoodOrdering.MAUI.ViewModels
{
    public partial class VendorOrdersViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;

        [ObservableProperty]
        ObservableCollection<Order> orders = new();

        [ObservableProperty]
        ObservableCollection<Order> pendingOrders = new();

        [ObservableProperty]
        ObservableCollection<Order> activeOrders = new();

        [ObservableProperty]
        bool hasOrders;

        public VendorOrdersViewModel(IApiService apiService)
        {
            _apiService = apiService;
            Title = "Vendor Orders";
        }

        [RelayCommand]
        async Task LoadOrdersAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                var orderList = await _apiService.GetVendorOrdersAsync();
                Orders.Clear();
                PendingOrders.Clear();
                ActiveOrders.Clear();

                foreach (var order in orderList.OrderByDescending(x => x.OrderDate))
                {
                    Orders.Add(order);

                    if (order.Status == OrderStatus.Pending)
                        PendingOrders.Add(order);
                    else if (order.Status == OrderStatus.Accepted || order.Status == OrderStatus.Preparing)
                        ActiveOrders.Add(order);
                }

                HasOrders = Orders.Count > 0;
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

        [RelayCommand]
        async Task AcceptOrderAsync(Order order)
        {
            try
            {
                await _apiService.UpdateOrderStatusAsync(order.Id, OrderStatus.Accepted);
                await LoadOrdersAsync();
                await Shell.Current.DisplayAlert("Success", "Order accepted!", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task StartPreparingAsync(Order order)
        {
            var readyTime = await Shell.Current.DisplayPromptAsync(
                "Estimated Ready Time",
                "Enter estimated preparation time (minutes):",
                "OK", "Cancel",
                "15",
                keyboard: Keyboard.Numeric);

            if (string.IsNullOrWhiteSpace(readyTime) || !int.TryParse(readyTime, out int minutes))
            {
                minutes = 15; // Default to 15 minutes
            }

            try
            {
                await _apiService.UpdateOrderStatusAsync(order.Id, OrderStatus.Preparing, minutes);
                await LoadOrdersAsync();
                await Shell.Current.DisplayAlert("Success", $"Order preparation started! Ready in {minutes} minutes.", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task MarkReadyAsync(Order order)
        {
            try
            {
                await _apiService.UpdateOrderStatusAsync(order.Id, OrderStatus.Ready);
                await LoadOrdersAsync();
                await Shell.Current.DisplayAlert("Success", "Order marked as ready for pickup!", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task CompleteOrderAsync(Order order)
        {
            try
            {
                await _apiService.UpdateOrderStatusAsync(order.Id, OrderStatus.Completed);
                await LoadOrdersAsync();
                await Shell.Current.DisplayAlert("Success", "Order completed!", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}