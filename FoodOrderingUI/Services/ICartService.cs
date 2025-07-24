using FoodOrderingUI.Models;
using MenuItem = FoodOrderingUI.Models.MenuItem;

namespace FoodOrderingUI.Services
{
    public interface ICartService
    {
        event EventHandler CartUpdated;
        void AddItem(MenuItem menuItem, int quantity = 1, string? notes = null);
        void RemoveItem(int menuItemId);
        void UpdateQuantity(int menuItemId, int quantity);
        void ClearCart();
        List<CartItem> GetCartItems();
        decimal GetTotal();
        int GetItemCount();
        string GetVendorId();
    }

}
