using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodOrderingUI.Models;
using FoodOrderingUI.Services;
using System.Collections.ObjectModel;

namespace FoodOrderingUI.ViewModels
{
    public partial class OrdersViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;

        [ObservableProperty]
        ObservableCollection<Order> orders = new();

        [ObservableProperty]
        bool hasOrders;

        public OrdersViewModel(IApiService apiService)
        {
            _apiService = apiService;
            Title = "My Orders";
        }

        [RelayCommand]
        async Task LoadOrdersAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                var orderList = await _apiService.GetMyOrdersAsync();
                Orders.Clear();

                foreach (var order in orderList.OrderByDescending(x => x.OrderDate))
                {
                    Orders.Add(order);
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
        async Task CancelOrderAsync(Order order)
        {
            if (order.Status != OrderStatus.Pending) return;

            var confirmed = await Shell.Current.DisplayAlert(
                "Cancel Order",
                "Are you sure you want to cancel this order?",
                "Yes", "No");

            if (!confirmed) return;

            try
            {
                var success = await _apiService.CancelOrderAsync(order.Id);
                if (success)
                {
                    await LoadOrdersAsync();
                    await Shell.Current.DisplayAlert("Success", "Order cancelled successfully.", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Unable to cancel order.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }

}
