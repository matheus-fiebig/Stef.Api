using API.STEF.Application.Orders.Models.Request.CreateOrder;
using API.STEF.Application.Orders.Models.Request.UpdateOrder;
using API.STEF.Application.Orders.Models.Response;
using API.STEF.Application.Shared.Models;

namespace API.STEF.Application.Orders.Services
{
    public interface IOrderService
    {
        Task<EitherOf<Error, bool>> DeleteAsync(int id);
        Task<EitherOf<Error, List<OrderResponse>>> GetAllAsync(int pageNumber, int pageSize);
        Task<EitherOf<Error, OrderResponse>> GetByIdAsync(int id);
        Task<EitherOf<Error, OrderResponse>> InsertAsync(CreateOrderRequest order);
        Task<EitherOf<Error, OrderResponse>> UpdateAsync(int id, UpdateOrderRequest order);
    }
}
