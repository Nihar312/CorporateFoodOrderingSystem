
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodOrdering.MAUI.Models;
using FoodOrdering.MAUI.Services;
using System.Collections.ObjectModel;

namespace FoodOrdering.MAUI.ViewModels
{
    public partial class MenuViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly ICartService _cartService;

        [ObservableProperty]
        ObservableCollection<MenuItom> menuItoms = new();

        [ObservableProperty]
        ObservableCollection<string> categories = new();

        [ObservableProperty]
        string selectedCategory = "All";

        [ObservableProperty]
        int cartItemCount;

        private List<MenuItom> _allMenuItoms = new();

        public MenuViewModel(IApiService apiService, ICartService cartService)
        {
            _apiService = apiService;
            _cartService = cartService;
            Title = "Menu";

            _cartService.CartUpdated += OnCartUpdated;
        }

        private void OnCartUpdated(object? sender, EventArgs e)
        {
            CartItemCount = _cartService.GetItemCount();
        }

        [RelayCommand]
        async Task LoadMenuAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                _allMenuItoms = await _apiService.GetMenuItomsAsync();
                menuItoms.Clear();

                foreach (var item in _allMenuItoms)
                {
                    menuItoms.Add(item);
                }

                LoadCategories();
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

        private void LoadCategories()
        {
            Categories.Clear();
            Categories.Add("All");

            var uniqueCategories = _allMenuItoms
                .Select(x => x.Category)
                .Distinct()
                .OrderBy(x => x);

            foreach (var category in uniqueCategories)
            {
                Categories.Add(category);
            }
        }

        [RelayCommand]
        void FilterByCategory(string category)
        {
            SelectedCategory = category;
            menuItoms.Clear();

            var filteredItems = category == "All"
                ? _allMenuItoms
                : _allMenuItoms.Where(x => x.Category == category);

            foreach (var item in filteredItems)
            {
                menuItoms.Add(item);
            }
        }

        [RelayCommand]
        async Task AddToCartAsync(MenuItom MenuItom)
        {
            try
            {
                _cartService.AddItem(MenuItom);
                await Shell.Current.DisplayAlert("Success", $"{MenuItom.Name} added to cart!", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task ViewCartAsync()
        {
            await Shell.Current.GoToAsync("cart");
        }
    }
}
