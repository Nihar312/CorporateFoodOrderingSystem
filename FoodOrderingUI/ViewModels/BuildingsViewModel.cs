using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodOrderingUI.Models;
using FoodOrderingUI.Services;
using System.Collections.ObjectModel;

namespace FoodOrderingUI.ViewModels
{
    public partial class BuildingsViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly ICartService _cartService;

        [ObservableProperty]
        ObservableCollection<Building> buildings = new();

        [ObservableProperty]
        bool hasBuildings;

        [ObservableProperty]
        int cartItemCount;

        public BuildingsViewModel(IApiService apiService, ICartService cartService)
        {
            _apiService = apiService;
            _cartService = cartService;
            Title = "Corporate Food Hub";

            _cartService.CartUpdated += OnCartUpdated;
        }

        private void OnCartUpdated(object? sender, EventArgs e)
        {
            CartItemCount = _cartService.GetItemCount();
        }

        [RelayCommand]
        async Task LoadBuildingsAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                var buildingList = await _apiService.GetBuildingsAsync();
                Buildings.Clear();

                foreach (var building in buildingList)
                {
                    Buildings.Add(building);
                }

                HasBuildings = Buildings.Count > 0;
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
        async Task SelectBuildingAsync(Building building)
        {
            if (building == null) return;

            await Shell.Current.GoToAsync($"canteens?buildingId={building.Id}");
        }

        [RelayCommand]
        async Task ViewCartAsync()
        {
            await Shell.Current.GoToAsync("cart");
        }
    }
}