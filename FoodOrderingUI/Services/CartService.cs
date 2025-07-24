using FoodOrderingUI.Models;
using MenuItem = FoodOrderingUI.Models.MenuItem;

namespace FoodOrderingUI.Services
{
    public class CartService : ICartService
    {
        private readonly List<CartItem> _cartItems = new();
        private string _vendorId = string.Empty;

        public event EventHandler CartUpdated;

        public void AddItem(MenuItem menuItem, int quantity = 1, string? notes = null)
        {
            // Check if cart is empty or same vendor
            if (_cartItems.Count == 0)
            {
                _vendorId = menuItem.VendorId;
            }
            else if (_vendorId != menuItem.VendorId)
            {
                // Different vendor, clear cart
                _cartItems.Clear();
                _vendorId = menuItem.VendorId;
            }

            var existingItem = _cartItems.FirstOrDefault(x => x.MenuItem.Id == menuItem.Id);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                existingItem.Notes = notes;
            }
            else
            {
                _cartItems.Add(new CartItem
                {
                    MenuItem = menuItem,
                    Quantity = quantity,
                    Notes = notes
                });
            }

            CartUpdated?.Invoke(this, EventArgs.Empty);
        }

        public void RemoveItem(int menuItemId)
        {
            var item = _cartItems.FirstOrDefault(x => x.MenuItem.Id == menuItemId);
            if (item != null)
            {
                _cartItems.Remove(item);

                if (_cartItems.Count == 0)
                {
                    _vendorId = string.Empty;
                }

                CartUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        public void UpdateQuantity(int menuItemId, int quantity)
        {
            var item = _cartItems.FirstOrDefault(x => x.MenuItem.Id == menuItemId);
            if (item != null)
            {
                if (quantity <= 0)
                {
                    RemoveItem(menuItemId);
                }
                else
                {
                    item.Quantity = quantity;
                    CartUpdated?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public void ClearCart()
        {
            _cartItems.Clear();
            _vendorId = string.Empty;
            CartUpdated?.Invoke(this, EventArgs.Empty);
        }

        public List<CartItem> GetCartItems()
        {
            return _cartItems.ToList();
        }

        public decimal GetTotal()
        {
            return _cartItems.Sum(x => x.Total);
        }

        public int GetItemCount()
        {
            return _cartItems.Sum(x => x.Quantity);
        }

        public string GetVendorId()
        {
            return _vendorId;
        }
    }

}
