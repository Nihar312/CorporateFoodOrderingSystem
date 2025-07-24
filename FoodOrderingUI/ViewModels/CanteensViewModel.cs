using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodOrderingUI.Models;
using FoodOrderingUI.Services;
using System.Collections.ObjectModel;

namespace FoodOrderingUI.ViewModels
{
    [QueryProperty(nameof(BuildingId), "buildingId")]
    public partial class CanteensViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly ICartService _cartService;

        [ObservableProperty]
        ObservableCollection<Canteen> canteens = new();

        [ObservableProperty]
        Building? selectedBuilding;

        [ObservableProperty]
        bool hasCanteens;

        [ObservableProperty]
        int cartItemCount;

        [ObservableProperty]
        int buildingId;

        public CanteensViewModel(IApiService apiService, ICartService cartService)
        {
            _apiService = apiService;
            _cartService = cartService;
            Title = "Canteens";

            _cartService.CartUpdated += OnCartUpdated;
        }

        private void OnCartUpdated(object? sender, EventArgs e)
        {
            CartItemCount = _cartService.GetItemCount();
        }

        partial void OnBuildingIdChanged(int value)
        {
            if (value > 0)
            {
                _ = LoadCanteensAsync();
            }
        }

        [RelayCommand]
        async Task LoadCanteensAsync()
        {
            if (IsBusy || BuildingId <= 0) return;

            try
            {
                IsBusy = true;

                // Load building details
                SelectedBuilding = await _apiService.GetBuildingAsync(BuildingId);
                Title = $"Canteens - {SelectedBuilding?.Name}";

                // Load canteens
                var canteenList = await _apiService.GetCanteensByBuildingAsync(BuildingId);
                Canteens.Clear();

                foreach (var canteen in canteenList)
                {
                    Canteens.Add(canteen);
                }

                HasCanteens = Canteens.Count > 0;
                CartItemCount = _cartService.GetItemCount();
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
        async Task SelectCanteenAsync(Canteen canteen)
        {
            if (canteen == null) return;

            await Shell.Current.GoToAsync($"menu?canteenId={canteen.Id}");
        }

        [RelayCommand]
        async Task ViewCartAsync()
        {
            await Shell.Current.GoToAsync("cart");
        }
    }
}