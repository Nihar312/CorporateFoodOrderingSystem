using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.EntityFrameworkCore;
using FoodOrdering.API.Data;

namespace FoodOrdering.API.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(ApplicationDbContext context, ILogger<NotificationService> logger)
        {
            _context = context;
            _logger = logger;

            // Initialize Firebase Admin SDK
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile("google-services.json")
                });
            }
        }

        public async Task SendNotificationAsync(string fcmToken, string title, string body, Dictionary<string, string>? data = null)
        {
            try
            {
                var message = new Message()
                {
                    Token = fcmToken,
                    Notification = new Notification()
                    {
                        Title = title,
                        Body = body
                    },
                    Data = data
                };

                var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                _logger.LogInformation($"Successfully sent message: {response}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending notification");
            }
        }

        public async Task SendOrderNotificationAsync(string userId, string title, string body, int orderId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null && !string.IsNullOrEmpty(user.FCMToken))
            {
                var data = new Dictionary<string, string>
                {
                    { "orderId", orderId.ToString() },
                    { "type", "order_update" }
                };

                await SendNotificationAsync(user.FCMToken, title, body, data);
            }
        }
    }
}