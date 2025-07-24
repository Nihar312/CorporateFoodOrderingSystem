namespace FoodOrderingUI.Views;

public partial class SplashPage : ContentPage
{
    public SplashPage()
    {
        InitializeComponent();
        NavigateToMainPage();
    }

    private async void NavigateToMainPage()
    {
        await Task.Delay(3000); // Show splash for 3 seconds
        await Shell.Current.GoToAsync("//login");
    }
}