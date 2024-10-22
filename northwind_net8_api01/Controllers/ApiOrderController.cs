using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using northwind_net8_api01.DAL;
using northwind_net8_api01.Models;
using Polly;
using System.Diagnostics;

namespace northwind_net8_api01.Controllers
{
    [ApiController]
    [Route("v1/order")]
    public class ApiOrderController : ControllerBase
    {
        private readonly ILogger<ApiOrderController> _logger;
        private INorthwind _northwind;
        private Stopwatch _sw = new Stopwatch();
        private Dictionary<string, string> _result = new Dictionary<string, string>();

        public ApiOrderController(ILogger<ApiOrderController> logger, INorthwind northwind)
        {
            _logger = logger;
            _northwind = northwind;
            _sw.Start();
        }

        [HttpGet]
        public IActionResult GetOrdersDefault()
        {
            return GetOrders(1, 10);
        }

        [HttpGet("{pageNumber}/{pageSize}")]
        public IActionResult GetOrders(int pageNumber, int pageSize)
        {
            var SourceIP = HttpContext.Connection.RemoteIpAddress;

            _logger.LogTrace($"GetOrders,{SourceIP},{pageNumber},{pageSize}");

            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("{}");
            }

            try
            {
                PagiOrderListModel result = null;
                Policy
                .Handle<SqlException>(
                    ex => SqlServerTransientExceptionDetector.ShouldRetryOn(ex))
                .Or<TimeoutException>()
                .WaitAndRetry(2, retryAttempt =>
                {
                    // Adjust the retry interval as you see fit
                    Random jitterer = new Random();
                    return TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + TimeSpan.FromMilliseconds(jitterer.Next(0, 1000));
                }
                )
                .Execute(() =>
                {
                    result = _northwind.GetOrderList(pageNumber, pageSize);
                });

                if (result == null)
                {
                    return BadRequest("{}");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetOrders, Error,'{SourceIP}' ,{ex.ToString()}");

                return BadRequest("{}");
            }
        }

        // 獲取單一訂單
        [HttpGet("{id}")]
        public ActionResult<OrderModel> GetOrderById(int id)
        {
            var SourceIP = HttpContext.Connection.RemoteIpAddress;

            _logger.LogTrace($"GetOrderById,{SourceIP},{id}");

            try
            {
                OrderModel result = null;
                Policy
                .Handle<SqlException>(
                    ex => SqlServerTransientExceptionDetector.ShouldRetryOn(ex))
                .Or<TimeoutException>()
                .WaitAndRetry(2, retryAttempt =>
                {
                    // Adjust the retry interval as you see fit
                    Random jitterer = new Random();
                    return TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + TimeSpan.FromMilliseconds(jitterer.Next(0, 1000));
                }
                )
                .Execute(() =>
                {
                    result = _northwind.GetOrderById(id);

                });

                if (result == null)
                {
                    return NotFound("{}");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetOrderById, Error,'{SourceIP}' ,{ex.ToString()}");

                return BadRequest("{}");
            }
        }

    }
}
