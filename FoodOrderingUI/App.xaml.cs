namespace FoodOrderingUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = base.CreateWindow(activationState);
            
            // Set initial route to splash screen
            if (window?.Page is Shell shell)
            {
                shell.GoToAsync("//splash");
            }
            
            return window;
        }
    }
}
