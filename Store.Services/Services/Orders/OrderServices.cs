using Store.Core;
using Store.Core.Entities;
using Store.Core.Entities.Order;
using Store.Core.Services.Contract;
using Store.Core.Specifications.Orders;

namespace Store.Services.Orders
{
    public class OrderServices : IOrderServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketServices _basketServices;
        private readonly IPaymentServices _paymentServices;

        public OrderServices(IUnitOfWork unitOfWork,IBasketServices basketServices,IPaymentServices paymentServices)
        {
            _unitOfWork = unitOfWork;
            _basketServices = basketServices;
            _paymentServices = paymentServices;
        }
        #region CreateOrder
        public async Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            var basket = await _basketServices.GetBasketAsync(basketId);
            if (basket == null) return null;
            var orderItems = new List<OrderItem>();
            if (basket.Items.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var productItem = await _unitOfWork.Repository<Product, int>().GetAsync(item.Id);
                    var ProductItemOrder = new ProductItemOrder(productItem.Id, productItem.Name, productItem.PictureUrl);
                    var orderItem = new OrderItem(ProductItemOrder, productItem.Price, item.Quantity);
                    orderItems.Add(orderItem);
                }

            }
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetAsync(deliveryMethodId);
            var subtotal = orderItems.Sum(item => item.Price * item.Quantity);

            if (string.IsNullOrEmpty(basket.PaymentIntentId)) 
            {
                var spec = new OrderSpecificationsWithPaymentIntentId(basket.PaymentIntentId);
                var ExistedOrder=await _unitOfWork.Repository<Order, int>().GetWithSpecAsync(spec);
                _unitOfWork.Repository<Order, int>().Delete(ExistedOrder);
            }
            var basketDto= await _paymentServices.CreateOrUpdatePaymentIntentIdAsync(basketId);

            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems, subtotal, basketDto.PaymentIntentId);
            await _unitOfWork.Repository<Order, int>().AddAsync(order);
            var result = await _unitOfWork.SaveChanges();
            if (result <= 0) return null;
            return order;
        }
        #endregion
        #region GetOrdersForUser
        public async Task<IEnumerable<Order>>? GetOrdersForSpecificUserAsync(string buyerEmail)
        {
            var spec = new OrderSpecifications(buyerEmail);
            var orders = await _unitOfWork.Repository<Order, int>().GetAllWithSpecAsync(spec);
            if (orders == null) return null;
            return orders;
        }
        #endregion
        #region GetOrderById
        public async Task<Order>? GetOrderByIdForSpecificUserAsync(string buyerEmail, int orderId)
        {
            var spec = new OrderSpecifications(buyerEmail,orderId);
            var order= await _unitOfWork.Repository<Order, int>().GetWithSpecAsync(spec);
            if (order == null) return null;
            return order;
        }
        #endregion
    }
}
