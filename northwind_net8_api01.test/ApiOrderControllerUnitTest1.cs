using Moq;
using System.Net.Http;
using System.Web;
using Microsoft.AspNetCore.Http;
using northwind_net8_api01.Controllers;
using Microsoft.Extensions.Logging;
using northwind_net8_api01.DAL;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using northwind_net8_api01.Models;

namespace northwind_net8_api01.test
{
    public class Tests
    {
        private Mock<ILogger<ApiOrderController>> _mockLogger;
        private Mock<INorthwind> _mockNorthwind;
        private DefaultHttpContext _httpContext;
        private ApiOrderController _controller;


        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<ApiOrderController>>();
            _mockNorthwind = new Mock<INorthwind>();
            _controller = new ApiOrderController(_mockLogger.Object, _mockNorthwind.Object);

            _httpContext = new DefaultHttpContext();
            _httpContext.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.1");
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = _httpContext
            };
        }

        [Test]
        [TestCase(1, 10, 2)]
        [TestCase(2, 5, 1)]
        [TestCase(3, 20, 0)]
        [TestCase(1, 50, 50)]
        [TestCase(5, 10, 5)]
        public void GetOrders_ReturnsValidResult(int pageNumber, int pageSize, int expectedTotal)
        {
            // Arrange
            var orderList = new PagiOrderListModel
            {
                pagenumber = pageNumber,
                pagesize = pageSize,
                total = expectedTotal,
                totalpages = (expectedTotal > 0) ? 1 : 0,
                orders = new List<OrderModel>
            {
                new OrderModel { OrderID = 1, CustomerID = "C1" },
                new OrderModel { OrderID = 2, CustomerID = "C2" }
            }
            };

            _mockNorthwind.Setup(n => n.GetOrderList(pageNumber, pageSize)).Returns(orderList);

            // Act
            var result = _controller.GetOrders(pageNumber, pageSize) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result?.StatusCode, Is.EqualTo(200));
            var returnedOrders = result?.Value as PagiOrderListModel;
            Assert.That(returnedOrders, Is.Not.Null);
            Assert.That(returnedOrders?.total, Is.EqualTo(expectedTotal));
        }

        [Test]
        [TestCase(0, 10)]
        [TestCase(-1, 5)]
        [TestCase(1, -10)]
        [TestCase(0, 0)]
        [TestCase(-5, -5)]
        public void GetOrders_ReturnsBadRequest_WhenInvalidParameters(int pageNumber, int pageSize)
        {
            // Act
            var result = _controller.GetOrders(pageNumber, pageSize) as BadRequestObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result?.StatusCode, Is.EqualTo(400));
            Assert.That(result?.Value, Is.EqualTo("{}"));
        }
    }
}