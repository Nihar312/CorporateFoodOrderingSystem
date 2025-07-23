using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Plugin.FirebasePushNotification;

namespace FoodOrdering.MAUI.Platforms.Android;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Firebase Push Notification
        FirebasePushNotificationManager.ProcessIntent(this, Intent);
    }

    protected override void OnNewIntent(Intent intent)
    {
        base.OnNewIntent(intent);
        FirebasePushNotificationManager.ProcessIntent(this, intent);
    }
}