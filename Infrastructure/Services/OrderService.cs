using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services
{
  public class OrderService : IOrderService
  {
    // private readonly IGenericRepository<Order> _orderRepo;
    // private readonly IGenericRepository<DeliveryMethod> _deliveryRepo;
    // private readonly IGenericRepository<Product> _productRepo;
    private readonly IBasketRepository _basketRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentService _paymentService;

    public OrderService(
    IBasketRepository basketRepository,
    IUnitOfWork unitOfWork, IPaymentService paymentService)
    {
      _paymentService = paymentService;
      _basketRepository = basketRepository;
      _unitOfWork = unitOfWork;

    }

    // public OrderService(IGenericRepository<Order> orderRepo,
    //     IGenericRepository<DeliveryMethod> deliveryRepo,
    //     IGenericRepository<Product> productRepo,
    //     IBasketRepository basketRepository)
    // {
    //   _productRepo = productRepo;
    //   _orderRepo = orderRepo;
    //   _deliveryRepo = deliveryRepo;
    //   _basketRepository = basketRepository;

    // }


    public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
    {
      //get basket from basket repo
      var basket = await _basketRepository.GetBasketAsync(basketId);
      //get items from product repo
      var items = new List<OrderItem>();
      foreach (var item in basket.Items)
      {
        //All data retrieved from the database except quantity
        // var productItem = await _productRepo.GetByIdAsync(item.Id);
        var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
        var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
        var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
        items.Add(orderItem);
      }
      //get delivery method from repo
      // var deliveryMethod = await _deliveryRepo.GetByIdAsync(deliveryMethodId);
      var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

      //calculate subtotal
      var subtotal = items.Sum(item => item.Price * item.Quantity);

      //Check if Order Exists
      var spec = new OrderByPaymentIntentIdSpecification(basket.PaymentIntentId);
      var existingOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

      if (existingOrder != null)
      {
        _unitOfWork.Repository<Order>().Delete(existingOrder);
        await _paymentService.CreateOrUpdatePaymentIntent(basket.PaymentIntentId);
      }

      //create the order
      var order = new Order(items, buyerEmail, shippingAddress, deliveryMethod, subtotal,
      basket.PaymentIntentId);
      _unitOfWork.Repository<Order>().Add(order);

      //TODO: save order to the database
      var result = await _unitOfWork.Complete();

      if (result <= 0) return null;

      //delete basket
      // await _basketRepository.DeleteBasketAsync(basketId);

      //return order 
      return order;
    }

    public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
    {
      return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
    }

    public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
    {
      var spec = new OrdersWithItemsAndOrderingSpecification(id, buyerEmail);

      return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
    }

    public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
    {
      var spec = new OrdersWithItemsAndOrderingSpecification(buyerEmail);

      return await _unitOfWork.Repository<Order>().ListAsync(spec);
    }
  }
}