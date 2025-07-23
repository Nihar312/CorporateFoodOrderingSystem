using FoodOrdering.MAUI.Models;

namespace FoodOrdering.MAUI.Services
{
    public class CartService : ICartService
    {
        private readonly List<CartItem> _cartItems = new();
        private string _vendorId = string.Empty;

        public event EventHandler CartUpdated;

        public void AddItem(MenuItom MenuItom, int quantity = 1, string? notes = null)
        {
            // Check if cart is empty or same vendor
            if (_cartItems.Count == 0)
            {
                _vendorId = MenuItom.VendorId;
            }
            else if (_vendorId != MenuItom.VendorId)
            {
                // Different vendor, clear cart
                _cartItems.Clear();
                _vendorId = MenuItom.VendorId;
            }

            var existingItem = _cartItems.FirstOrDefault(x => x.MenuItom.Id == MenuItom.Id);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                existingItem.Notes = notes;
            }
            else
            {
                _cartItems.Add(new CartItem
                {
                    MenuItom = MenuItom,
                    Quantity = quantity,
                    Notes = notes
                });
            }

            CartUpdated?.Invoke(this, EventArgs.Empty);
        }

        public void RemoveItem(int MenuItomId)
        {
            var item = _cartItems.FirstOrDefault(x => x.MenuItom.Id == MenuItomId);
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

        public void UpdateQuantity(int MenuItomId, int quantity)
        {
            var item = _cartItems.FirstOrDefault(x => x.MenuItom.Id == MenuItomId);
            if (item != null)
            {
                if (quantity <= 0)
                {
                    RemoveItem(MenuItomId);
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