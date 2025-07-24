using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodOrderingUI.Services;
using System.Collections.ObjectModel;
using MenuItem = FoodOrderingUI.Models.MenuItem;

namespace FoodOrderingUI.ViewModels
{
    public partial class MenuViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly ICartService _cartService;

        [ObservableProperty]
        ObservableCollection<MenuItem> menuItems = new();

        [ObservableProperty]
        ObservableCollection<string> categories = new();

        [ObservableProperty]
        string selectedCategory = "All";

        [ObservableProperty]
        int cartItemCount;

        private List<MenuItem> _allMenuItems = new();

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

                _allMenuItems = await _apiService.GetMenuItemsAsync();
                MenuItems.Clear();

                foreach (var item in _allMenuItems)
                {
                    MenuItems.Add(item);
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

            var uniqueCategories = _allMenuItems
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
            MenuItems.Clear();

            var filteredItems = category == "All"
                ? _allMenuItems
                : _allMenuItems.Where(x => x.Category == category);

            foreach (var item in filteredItems)
            {
                MenuItems.Add(item);
            }
        }

        [RelayCommand]
        async Task AddToCartAsync(MenuItem menuItem)
        {
            try
            {
                _cartService.AddItem(menuItem);
                await Shell.Current.DisplayAlert("Success", $"{menuItem.Name} added to cart!", "OK");
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
