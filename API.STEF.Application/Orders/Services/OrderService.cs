using API.STEF.Application.Orders.Models.Request.CreateOrder;
using API.STEF.Application.Orders.Models.Request.UpdateOrder;
using API.STEF.Application.Orders.Models.Response;
using API.STEF.Application.Orders.Specifications;
using API.STEF.Application.Shared.Models;
using API.STEF.Domain.Contracts.Repositories;
using API.STEF.Domain.Contracts.UnitOfWork;
using API.STEF.Domain.OrderAggregator;
using API.STEF.Domain.Shared.Models;

namespace API.STEF.Application.Orders.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IOrderRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<EitherOf<Error, List<OrderResponse>>> GetAllAsync(int pageNumber, int pageSize) 
        {
            var orders = await _repository.GetAsync(pageSize, pageNumber);
            return orders.Select(OrderResponse.MapFrom).ToList();
        }

        public async Task<EitherOf<Error, OrderResponse>> GetByIdAsync(int id)
        {
            var specification = new OrderByIdSpecification(id);
            var entity = await _repository.GetAsync(specification);
            if (entity == null)
            {
                return Error.Create("Pedido não encontrado");
            }

            return OrderResponse.MapFrom(entity);         
        }

        public async Task<EitherOf<Error, OrderResponse>> UpdateAsync(int id, UpdateOrderRequest request)
        {
            try
            {
                var order = await _repository.GetAsync(new OrderByIdSpecification(id));
                
                if (order == null)
                {
                    return Error.Create("Pedido não encontrado");
                }

                if (order.CanChangeOrder())
                { 
                    return Error.Create("Pedido ja foi pago");
                }

                await _unitOfWork.BeginTrasactionAsync();

                order.Update(
                    CustomerVO.CreateNew(request.CustomerName, request.CustomerEmail),
                    request.OrderItems.Select(x => new OrderItemDto(x.Quantity, x.ProductId)).ToList(),
                    request.Paid
                );

                await _repository.UpdateAsync(order);
                await _unitOfWork.CommitTransactionAsync();
                 
                return OrderResponse.MapFrom(order);
            }
            catch
            {
                await _unitOfWork.RollbackTrasactionAsync();
                throw;
            }
        }

        public async Task<EitherOf<Error, OrderResponse>> InsertAsync(CreateOrderRequest order)
        {
            try
            {
                if(order.OrderItems == null || !order.OrderItems.Any())
                {
                    return Error.Create("Nao e permitido a criacao de pedido sem itens");
                }

                await _unitOfWork.BeginTrasactionAsync();

                var newEntity = Order.CreateNew(
                    CustomerVO.CreateNew(order.CustomerName, order.CustomerEmail),
                    order.OrderItems.Select(x => new OrderItemDto(x.Quantity, x.ProductId)).ToList()
                );

                var addedEntity = await _repository.InsertAsync(newEntity);
                await _unitOfWork.CommitTransactionAsync();

                return OrderResponse.MapFrom(addedEntity);
            }
            catch
            {
                await _unitOfWork.RollbackTrasactionAsync();
                throw;
            }
        }
  
        public async Task<EitherOf<Error, bool>> DeleteAsync(int id)
        {
            try
            {
                var order = await _repository.GetAsync(new OrderByIdSpecification(id));

                if (order == null)
                {
                    return Error.Create("Pedido não encontrado");
                }

                if (order.CanChangeOrder())
                {
                    return Error.Create("Pedido ja foi pago");
                }

                await _unitOfWork.BeginTrasactionAsync();

                await _repository.DeleteAsync(new OrderByIdSpecification(id));

                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTrasactionAsync();
                throw;
            }
        }
    }
}
