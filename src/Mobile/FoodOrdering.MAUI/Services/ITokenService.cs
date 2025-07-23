namespace FoodOrdering.MAUI.Services
{
    public interface ITokenService
    {
        Task<string?> GetTokenAsync();
        Task SetTokenAsync(string token);
        Task ClearTokenAsync();
    }
}