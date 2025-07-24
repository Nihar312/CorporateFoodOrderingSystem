using CommunityToolkit.Mvvm.ComponentModel;

namespace FoodOrderingUI.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        bool isBusy;

        [ObservableProperty]
        string title = string.Empty;
    }
}
