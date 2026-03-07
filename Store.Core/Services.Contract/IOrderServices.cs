using Store.Core.Entities.Order;

namespace Store.Core.Services.Contract
{
    public interface IOrderServices
    {
        Task<Order> CreateOrderAsync(string buyerEmail,string basketId,int deliveryMethodId,Address shippingAddress);
        Task<IEnumerable<Order>>? GetOrdersForSpecificUserAsync(string buyerEmail);
        Task<Order>? GetOrderByIdForSpecificUserAsync(string buyerEmail,int orderId);
    }
}
