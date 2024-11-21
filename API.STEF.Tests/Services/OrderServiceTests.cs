using API.STEF.Application.Orders.Models.Request.CreateOrder;
using API.STEF.Application.Orders.Models.Request.UpdateOrder;
using API.STEF.Application.Orders.Services;
using API.STEF.Domain.Contracts.Repositories;
using API.STEF.Domain.Contracts.UnitOfWork;
using API.STEF.Domain.OrderAggregator;
using API.STEF.Domain.ProductAggregator;
using API.STEF.Domain.Shared.Interfaces;
using API.STEF.Domain.Shared.Models;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Moq;

namespace API.STEF.Tests.Services
{

    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _repositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly OrderService _service;

        public OrderServiceTests()
        {
            _repositoryMock = new Mock<IOrderRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _service = new OrderService(_repositoryMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnOrders_WhenSuccess()
        {
            // Arrange
            var itens = new List<OrderItemDto>()
            {
                new OrderItemDto(1,1),
                new OrderItemDto(2,2)
            };

            var orders = new List<Order>()
            {
                Order.CreateNew(CustomerVO.CreateNew("jose", "abc@abc.com"), itens),
                Order.CreateNew(CustomerVO.CreateNew("jose2", "abc2@abc.com"), itens)
            };

            _repositoryMock
                .Setup(r => r.GetAsync(It.IsAny<int>(), It.IsAny<int>(), null))
                .ReturnsAsync(orders);

            // Act
            var result = await _service.GetAllAsync(1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Right);
            Assert.Null(result.Left);
            Assert.Equal(2, result.Right.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnOrder_WhenSuccess()
        {
            // Arrange
            var itens = new List<OrderItemDto>()
            {
                new OrderItemDto(2,1)
            };

            var order = Order.CreateNew(CustomerVO.CreateNew("jose", "abc@abc.com"), itens);
            order.OrderItems.First().Product = Product.CreateNew("produto", 10);

            _repositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ISpecification<Order>>()))
                .ReturnsAsync(order);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Right);
            Assert.Null(result.Left);
            Assert.Equal("jose", result.Right.CustomerName);
            Assert.Equal("abc@abc.com", result.Right.CustomerEmail);
            Assert.False(result.Right.Paid);
            Assert.Equal(20, result.Right.TotalValue);
            Assert.Single(result.Right.OrderItems);
            Assert.Equal(2,result.Right.OrderItems.First().Quantity);
            Assert.Equal(1,result.Right.OrderItems.First().ProductId);
            Assert.Equal("produto",result.Right.OrderItems.First().ProductName);
            Assert.Equal(10,result.Right.OrderItems.First().UnitPrice);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnError_WhenNotFound()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ISpecification<Order>>()))
                .ReturnsAsync((Order) null);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Right);
            Assert.NotNull(result.Left);
            Assert.Equal("Pedido não encontrado", result.Left.Description);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenSuccess()
        {
            // Arrange
            var itens = new List<OrderItemDto>()
            {
                new OrderItemDto(2,1)
            };

            var order = Order.CreateNew(CustomerVO.CreateNew("jose", "abc@abc.com"), itens);

            _repositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ISpecification<Order>>()))
                .ReturnsAsync(order);

            _repositoryMock
                .Setup(r => r.DeleteAsync(It.IsAny<ISpecification<Order>>()));

            // Act
            var result = await _service.DeleteAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Right);
            Assert.Null(result.Left);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnError_WhenNoOrderIsFound()
        {
            // Arrange

            // Act
            var result = await _service.DeleteAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Right);
            Assert.NotNull(result.Left);
            Assert.Equal("Pedido não encontrado", result.Left.Description);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnError_WhenOrderIsPaid()
        {
            // Arrange
            var itens = new List<OrderItemDto>()
            {
                new OrderItemDto(2,1)
            };

            var order = Order.CreateNew(CustomerVO.CreateNew("jose", "abc@abc.com"), itens);
            order.PayOrder();

            _repositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ISpecification<Order>>()))
                .ReturnsAsync(order);

            _repositoryMock
                .Setup(r => r.DeleteAsync(It.IsAny<ISpecification<Order>>()));

            // Act
            var result = await _service.DeleteAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Right);
            Assert.NotNull(result.Left);
            Assert.Equal("Pedido ja foi pago", result.Left.Description);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRollback_WhenException()
        {
            // Arrange
            _repositoryMock
               .Setup(r => r.GetAsync(It.IsAny<ISpecification<Order>>()))
               .ThrowsAsync(new Exception("ops"));


            // Act
            var result = await Assert.ThrowsAsync<Exception>(async () => await _service.DeleteAsync(1));

            // Assert
            _unitOfWorkMock.Verify(x => x.RollbackTrasactionAsync(), Times.Once);
        }

        [Fact]
        public async Task InsertAsync_ShouldReturnOrder_WhenSuccess()
        {
            // Arrange
            var request = new CreateOrderRequest()
            {
                CustomerName = "a",
                CustomerEmail = "b@b.com",
                OrderItems = new List<CreateOrderItemRequest>
                {
                    new()
                    {
                        Quantity = 1,
                        ProductId = 1,
                    }
                }
            };

            _repositoryMock
                .Setup(r => r.InsertAsync(It.IsAny<Order>()))
                .ReturnsAsync(Order.CreateNew(CustomerVO.CreateNew("a","b"), new List<OrderItemDto>()));

            // Act
            var result = await _service.InsertAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Right);
            Assert.Null(result.Left);
        }

        [Fact]
        public async Task InsertAsync_ShouldReturnError_WhenNoItensArePassed()
        {
            // Arrange
            var request = new CreateOrderRequest()
            {
                CustomerName = "a",
                CustomerEmail = "b@b.com",
                OrderItems = new List<CreateOrderItemRequest>()
            };

            // Act
            var result = await _service.InsertAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Right);
            Assert.NotNull(result.Left);
            Assert.Equal("Nao e permitido a criacao de pedido sem itens", result.Left.Description);
        }

        [Fact]
        public async Task InsertAsync_ShouldRollback_WhenException()
        {
            // Arrange
            var request = new CreateOrderRequest()
            {
                CustomerName = "a",
                CustomerEmail = "b@b.com",
                OrderItems = new List<CreateOrderItemRequest>()
                {
                    new()
                    {
                        Quantity = 1,
                        ProductId = 1,
                    }
                }
            };

            _repositoryMock
               .Setup(r => r.InsertAsync(It.IsAny<Order>()))
               .ThrowsAsync(new Exception("ops"));


            // Act
            var result = await Assert.ThrowsAsync<Exception>(async () =>  await _service.InsertAsync(request));

            // Assert
            _unitOfWorkMock.Verify(x => x.RollbackTrasactionAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldRollback_WhenException()
        {
            // Arrange
            var request = new UpdateOrderRequest()
            {
                CustomerName = "a",
                CustomerEmail = "b@b.com",
                OrderItems = new List<UpdateOrderItemRequest>()
                {
                    new()
                    {
                        Quantity = 1,
                        ProductId = 1,
                    }
                }
            };

            _repositoryMock
               .Setup(r => r.GetAsync(It.IsAny<ISpecification<Order>>()))
               .ThrowsAsync(new Exception("ops"));


            // Act
            var result = await Assert.ThrowsAsync<Exception>(async () => await _service.UpdateAsync(1, request));

            // Assert
            _unitOfWorkMock.Verify(x => x.RollbackTrasactionAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnError_WhenNoOrderFound()
        {
            // Arrange
            var request = new UpdateOrderRequest()
            {
                CustomerName = "a",
                CustomerEmail = "b@b.com",
                OrderItems = new List<UpdateOrderItemRequest>()
                {
                    new()
                    {
                        Quantity = 1,
                        ProductId = 1,
                    }
                }
            };

            _repositoryMock
               .Setup(r => r.GetAsync(It.IsAny<ISpecification<Order>>()))
               .ReturnsAsync((Order) null);

            // Act
            var result = await _service.UpdateAsync(1, request);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Right);
            Assert.NotNull(result.Left);
            Assert.Equal("Pedido não encontrado", result.Left.Description);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnError_WhenOrderAlreadyPaid()
        {
            // Arrange
            var request = new UpdateOrderRequest()
            {
                CustomerName = "a",
                CustomerEmail = "b@b.com",
                OrderItems = new List<UpdateOrderItemRequest>()
                {
                    new()
                    {
                        Quantity = 1,
                        ProductId = 1,
                    }
                }
            };

            var order = Order.CreateNew(CustomerVO.CreateNew("b", "c@c.com"), new List<OrderItemDto>());
            order.PayOrder();

            _repositoryMock
               .Setup(r => r.GetAsync(It.IsAny<ISpecification<Order>>()))
               .ReturnsAsync(order);

            // Act
            var result = await _service.UpdateAsync(1, request);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Right);
            Assert.NotNull(result.Left);
            Assert.Equal("Pedido ja foi pago", result.Left.Description);
        }


        [Fact]
        public async Task UpdateAsync_ShouldReturnOrder_WhenSuccess()
        {
            // Arrange
            var request = new UpdateOrderRequest()
            {
                CustomerName = "a",
                CustomerEmail = "b@b.com",
                OrderItems = new List<UpdateOrderItemRequest>()
                {
                    new()
                    {
                        Quantity = 1,
                        ProductId = 1,
                    }
                }
            };

            var order = Order.CreateNew(CustomerVO.CreateNew("b", "c@c.com"), new List<OrderItemDto>());

            _repositoryMock
               .Setup(r => r.GetAsync(It.IsAny<ISpecification<Order>>()))
               .ReturnsAsync(order);

            // Act
            var result = await _service.UpdateAsync(1, request);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Right);
            Assert.Null(result.Left);
        }
    }
}