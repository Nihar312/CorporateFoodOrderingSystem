using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodOrdering.MAUI.Services;

namespace FoodOrdering.MAUI.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;

        [ObservableProperty]
        string email = string.Empty;

        [ObservableProperty]
        string password = string.Empty;

        [ObservableProperty]
        bool isLoginMode = true;

        [ObservableProperty]
        string firstName = string.Empty;

        [ObservableProperty]
        string lastName = string.Empty;

        [ObservableProperty]
        string companyName = string.Empty;

        public LoginViewModel(IApiService apiService)
        {
            _apiService = apiService;
            Title = "Login";
        }

        [RelayCommand]
        async Task LoginAsync()
        {
            if (IsBusy) return;

            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await Shell.Current.DisplayAlert("Error", "Please fill in all required fields.", "OK");
                return;
            }

            try
            {
                IsBusy = true;

                if (IsLoginMode)
                {
                    await _apiService.LoginAsync(Email, Password);
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
                    {
                        await Shell.Current.DisplayAlert("Error", "Please fill in all required fields.", "OK");
                        return;
                    }

                    await _apiService.RegisterAsync(Email, Password, FirstName, LastName, CompanyName);
                }

                await Shell.Current.GoToAsync("//main");
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
        void ToggleMode()
        {
            IsLoginMode = !IsLoginMode;
            Title = IsLoginMode ? "Login" : "Register";
            Email = string.Empty;
            Password = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            CompanyName = string.Empty;
        }
    }
}
