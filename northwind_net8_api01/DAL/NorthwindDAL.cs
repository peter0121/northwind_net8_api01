using Dapper;
using northwind_net8_api01.Models;
using northwind_net8_api01.MsSQL;

namespace northwind_net8_api01.DAL
{
    public class NorthwindDAL : INorthwind
    {
        private readonly ILogger<NorthwindDAL> _logger;

        private MSSQLConnectionFactory _MsSQLConnectionFactory;

        private readonly string _table_name_orders = "[Orders]";
        private readonly string _table_name_order_details = "[Order Details]";

        public NorthwindDAL(ILogger<NorthwindDAL> logger, MSSQLConnectionFactory connectionFactory)
        {
            _logger = logger;
            _MsSQLConnectionFactory = connectionFactory;
        }

        public PagiOrderListModel GetOrderList(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                //PageNumber and PageSize should be greater than 0.
                return null;
            }

            PagiOrderListModel paginationResult = null;

            using (var connection = _MsSQLConnectionFactory.CreateConn())
            {
                var query = @"SELECT [OrderID],[CustomerID],[EmployeeID],[OrderDate],[RequiredDate],[ShippedDate],[ShipVia],[Freight],
                            [ShipName],[ShipAddress],[ShipCity],[ShipRegion],[ShipPostalCode],[ShipCountry] FROM " + _table_name_orders + @"
                            ORDER BY OrderID OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
            
                            SELECT COUNT(*) FROM " + _table_name_orders + ";";

                var offset = (pageNumber - 1) * pageSize;

                using (var multi = connection.QueryMultiple(query, new { Offset = offset, PageSize = pageSize }))
                {
                    var orders = multi.Read<OrderModel>().ToList();
                    var totalRecords = multi.ReadSingle<int>();

                    var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                    paginationResult = new PagiOrderListModel()
                    {
                        pagenumber = pageNumber,
                        pagesize = pageSize,
                        total = totalRecords,
                        totalpages = totalPages,
                        orders = orders
                    };

                    //return Ok(paginationResult);
                }

            }

            if (paginationResult == null)
            {
                paginationResult = new PagiOrderListModel();
            }

            return paginationResult;
        }
    }
}