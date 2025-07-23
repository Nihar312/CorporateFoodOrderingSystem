using Microsoft.EntityFrameworkCore;
using AutoMapper;
using FoodOrdering.API.Data;
using FoodOrdering.API.DTOs;
using FoodOrdering.API.Models;

namespace FoodOrdering.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly ILogger<OrderService> _logger;

        public OrderService(ApplicationDbContext context, IMapper mapper,
            INotificationService notificationService, ILogger<OrderService> logger)
        {
            _context = context;
            _mapper = mapper;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task<OrderResponseDto> CreateOrderAsync(string userId, CreateOrderDto orderDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var order = new Order
                {
                    UserId = userId,
                    VendorId = orderDto.VendorId,
                    SpecialInstructions = orderDto.SpecialInstructions,
                    Status = OrderStatus.Pending
                };

                decimal totalAmount = 0;
                var orderItems = new List<OrderItem>();

                foreach (var itemDto in orderDto.Items)
                {
                    var MenuItom = await _context.MenuItoms.FindAsync(itemDto.MenuItomId);
                    if (MenuItom == null || !MenuItom.IsAvailable)
                        throw new ArgumentException($"Menu item {itemDto.MenuItomId} not available");

                    var orderItem = new OrderItem
                    {
                        MenuItomId = itemDto.MenuItomId,
                        Quantity = itemDto.Quantity,
                        Price = MenuItom.Price,
                        Notes = itemDto.Notes
                    };

                    orderItems.Add(orderItem);
                    totalAmount += MenuItom.Price * itemDto.Quantity;
                }

                order.TotalAmount = totalAmount;
                order.OrderItems = orderItems;

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                // Send notification to vendor
                await _notificationService.SendOrderNotificationAsync(
                    orderDto.VendorId,
                    "New Order Received",
                    $"You have received a new order worth ₹{totalAmount}",
                    order.Id
                );

                return await GetOrderByIdAsync(order.Id);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<OrderResponseDto> GetOrderByIdAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Vendor)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItom)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                throw new ArgumentException("Order not found");

            return _mapper.Map<OrderResponseDto>(order);
        }

        public async Task<List<OrderResponseDto>> GetOrdersByUserIdAsync(string userId)
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Vendor)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItom)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return _mapper.Map<List<OrderResponseDto>>(orders);
        }

        public async Task<List<OrderResponseDto>> GetOrdersByVendorIdAsync(string vendorId)
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Vendor)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItom)
                .Where(o => o.VendorId == vendorId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return _mapper.Map<List<OrderResponseDto>>(orders);
        }

        public async Task<OrderResponseDto> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusDto statusDto)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Vendor)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                throw new ArgumentException("Order not found");

            order.Status = statusDto.Status;

            if (statusDto.EstimatedReadyTime.HasValue)
                order.EstimatedReadyTime = statusDto.EstimatedReadyTime;

            if (statusDto.Status == OrderStatus.Ready)
                order.ReadyTime = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Send notification to user
            string title = "";
            string body = "";

            switch (statusDto.Status)
            {
                case OrderStatus.Accepted:
                    title = "Order Accepted";
                    body = "Your order has been accepted by the vendor";
                    break;
                case OrderStatus.Preparing:
                    title = "Order Being Prepared";
                    body = $"Your order is being prepared. Ready in {statusDto.EstimatedReadyTime ?? 15} minutes";
                    break;
                case OrderStatus.Ready:
                    title = "Order Ready";
                    body = "Your order is ready for pickup from the canteen";
                    break;
                case OrderStatus.Completed:
                    title = "Order Completed";
                    body = "Thank you for your order!";
                    break;
                case OrderStatus.Cancelled:
                    title = "Order Cancelled";
                    body = "Your order has been cancelled";
                    break;
            }

            await _notificationService.SendOrderNotificationAsync(order.UserId, title, body, orderId);

            return await GetOrderByIdAsync(orderId);
        }

        public async Task<bool> CancelOrderAsync(int orderId, string userId)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            if (order == null || order.Status != OrderStatus.Pending)
                return false;

            order.Status = OrderStatus.Cancelled;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}