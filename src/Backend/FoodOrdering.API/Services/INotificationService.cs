namespace FoodOrdering.API.Services
{
    public interface INotificationService
    {
        Task SendNotificationAsync(string fcmToken, string title, string body, Dictionary<string, string>? data = null);
        Task SendOrderNotificationAsync(string userId, string title, string body, int orderId);
    }
}
