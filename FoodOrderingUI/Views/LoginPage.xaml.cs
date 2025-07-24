using FoodOrderingUI.ViewModels;

namespace FoodOrderingUI.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel loginViewModel)
	{
        BindingContext = loginViewModel;
		InitializeComponent();
	}

    private void EmailEntry_Completed(object sender, EventArgs e)
    {
        PasswordEntry.Focus();
    }
}