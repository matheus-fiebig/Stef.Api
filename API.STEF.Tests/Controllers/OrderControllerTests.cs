using API.STEF.Application.Orders.Models.Request.CreateOrder;
using API.STEF.Application.Orders.Models.Request.UpdateOrder;
using API.STEF.Application.Orders.Models.Response;
using API.STEF.Application.Orders.Services;
using API.STEF.Application.Shared.Models;
using API.STEF.Presentation.Controllers;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace API.STEF.Tests.Controllers
{
    public class OrderControllerTests
    {
        private readonly Mock<IOrderService> _serviceMock;
        private readonly OrderController _controller;
        private readonly IFixture _fixture;

        public OrderControllerTests()
        {
            _fixture = new Fixture();
            _serviceMock = new Mock<IOrderService>();
            _controller = new OrderController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkWithOrders_WhenServiceReturnsOrders()
        {
            // Arrange
            var orders = _fixture.Build<OrderResponse>().CreateMany();
            _serviceMock
                .Setup(s => s.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new EitherOf<Error, List<OrderResponse>>() { Right = orders.ToList() });

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(orders, okResult.Value);
        }

        [Fact]
        public async Task GetAll_ShouldReturnBadRequestWithErrors_WhenServiceReturnsErrors()
        {
            // Arrange
            var error =  Error.Create("Invalid data");
            _serviceMock
               .Setup(s => s.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
               .ReturnsAsync(new EitherOf<Error, List<OrderResponse>>() { Left = error });

            // Act
            var result = await _controller.Get();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(error, badRequestResult.Value);
        }

        [Fact]
        public async Task GetById_ShouldReturnOkWithOrder_WhenServiceReturnsOrder()
        {
            // Arrange
            var order = _fixture.Build<OrderResponse>().Create();
            _serviceMock
                .Setup(s => s.GetByIdAsync(1))
                .ReturnsAsync(new EitherOf<Error, OrderResponse>() { Right = order });

            // Act
            var result = await _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(order, okResult.Value);
        }

        [Fact]
        public async Task Insert_ShouldReturnOk_WhenInsertSucceeds()
        {
            // Arrange
            var order = _fixture.Build<OrderResponse>().Create();
            var request = new CreateOrderRequest();
            _serviceMock
                .Setup(s => s.InsertAsync(request))
                .ReturnsAsync(new EitherOf<Error, OrderResponse>() { Right = order });

            // Act
            var result = await _controller.Insert(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(order, okResult.Value);
        }

        [Fact]
        public async Task Update_ShouldReturnBadRequest_WhenUpdateFailsWithErrors()
        {
            // Arrange
            var error = Error.Create("Cannot update paid order");
            var request = new UpdateOrderRequest();
            _serviceMock
                .Setup(s => s.UpdateAsync(1, request))
                .ReturnsAsync(new EitherOf<Error, OrderResponse>() { Left = error });

            // Act
            var result = await _controller.Update(1, request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(error, badRequestResult.Value);
        }

        [Fact]
        public async Task Delete_ShouldReturnOk_WhenDeleteSucceeds()
        {
            // Arrange
            _serviceMock
                .Setup(s => s.DeleteAsync(1))
                .ReturnsAsync(new EitherOf<Error, bool>() { Right = true });

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.True((bool)okResult.Value);
        }

        [Fact]
        public async Task Get_ShouldReturnInternalServerError_WhenServiceThrows()
        {
            // Arrange
            var error = Error.Create("Invalid data");
            _serviceMock
                .Setup(s => s.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("something bad happend"));

            // Act
            var result = await _controller.Get();

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(objectResult.StatusCode, 500);
        }
    }
}
