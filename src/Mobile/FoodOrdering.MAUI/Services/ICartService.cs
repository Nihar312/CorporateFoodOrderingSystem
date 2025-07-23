using FoodOrdering.MAUI.Models;

namespace FoodOrdering.MAUI.Services
{
    public interface ICartService
    {
        event EventHandler CartUpdated;
        void AddItem(MenuItom MenuItom, int quantity = 1, string? notes = null);
        void RemoveItem(int MenuItomId);
        void UpdateQuantity(int MenuItomId, int quantity);
        void ClearCart();
        List<CartItem> GetCartItems();
        decimal GetTotal();
        int GetItemCount();
        string GetVendorId();
    }
}