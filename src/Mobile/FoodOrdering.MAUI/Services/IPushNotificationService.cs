namespace FoodOrdering.MAUI.Services
{
    public interface IPushNotificationService
    {
        Task<string> GetTokenAsync();
        void Initialize();
        event EventHandler<string> TokenRefreshed;
        event EventHandler<Dictionary<string, object>> NotificationReceived;
    }
}