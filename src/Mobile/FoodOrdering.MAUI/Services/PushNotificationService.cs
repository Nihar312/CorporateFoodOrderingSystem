using Plugin.FirebasePushNotification;

namespace FoodOrdering.MAUI.Services
{
    public class PushNotificationService : IPushNotificationService
    {
        public event EventHandler<string> TokenRefreshed;
        public event EventHandler<Dictionary<string, object>> NotificationReceived;

        public void Initialize()
        {
            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                TokenRefreshed?.Invoke(this, p.Token);
            };

            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                NotificationReceived?.Invoke(this, (Dictionary<string, object>)p.Data);
            };

            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                // Handle notification tap
                if (p.Data.ContainsKey("orderId"))
                {
                    var orderId = p.Data["orderId"].ToString();
                    Shell.Current.GoToAsync($"//orders?orderId={orderId}");
                }
            };
        }

        public async Task<string> GetTokenAsync()
        {
            return CrossFirebasePushNotification.Current.Token ?? string.Empty;
        }
    }
}