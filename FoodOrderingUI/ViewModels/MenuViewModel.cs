using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodOrderingUI.Services;
using System.Collections.ObjectModel;
using MenuItem = FoodOrderingUI.Models.MenuItem;

namespace FoodOrderingUI.ViewModels
{
    [QueryProperty(nameof(CanteenId), "canteenId")]
    public partial class MenuViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly ICartService _cartService;

        [ObservableProperty]
        ObservableCollection<MenuItem> menuItems = new();

        [ObservableProperty]
        ObservableCollection<string> categories = new();

        [ObservableProperty]
        string selectedCategory = "Breakfast";

        [ObservableProperty]
        int cartItemCount;

        [ObservableProperty]
        int canteenId;

        [ObservableProperty]
        Canteen? selectedCanteen;

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

        partial void OnCanteenIdChanged(int value)
        {
            if (value > 0)
            {
                _ = LoadMenuAsync();
            }
        }

        [RelayCommand]
        async Task LoadMenuAsync()
        {
            if (IsBusy || CanteenId <= 0) return;

            try
            {
                IsBusy = true;

                // Load canteen details
                SelectedCanteen = await _apiService.GetCanteenAsync(CanteenId);
                Title = $"Menu - {SelectedCanteen?.Name}";

                // Load menu items
                _allMenuItems = await _apiService.GetMenuByCanteenAsync(CanteenId);
                MenuItems.Clear();

                foreach (var item in _allMenuItems)
                {
                    MenuItems.Add(item);
                }

                LoadCategories();
                FilterByCategory("Breakfast"); // Default to breakfast
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
            Categories.Add("Breakfast");
            Categories.Add("Snacks");
            Categories.Add("Lunch");
        }

        private int GetCategoryValue(string categoryName)
        {
            return categoryName switch
            {
                "Breakfast" => 1,
                "Snacks" => 2,
                "Lunch" => 3,
                _ => 1
            };
        }

        private bool MatchesCategory(MenuItem item, string categoryName)
        {
            if (categoryName == "All") return true;
            
            var categoryValue = GetCategoryValue(categoryName);
            return item.Category == categoryValue;
        }

        [RelayCommand]
        async Task FilterByCategoryAsync(string category)
        {
            if (IsBusy || CanteenId <= 0) return;

            try
            {
                IsBusy = true;
                SelectedCategory = category;
                
                var categoryValue = GetCategoryValue(category);
                var filteredItems = await _apiService.GetMenuByCanteenAndCategoryAsync(CanteenId, categoryValue);
                
                MenuItems.Clear();
                foreach (var item in filteredItems)
                {
                    MenuItems.Add(item);
                }
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
        void FilterByCategory(string category)
        {
            SelectedCategory = category;
            MenuItems.Clear();

            var filteredItems = _allMenuItems.Where(x => MatchesCategory(x, category));

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
