using northwind_net8_api01.Models;

namespace northwind_net8_api01.DAL
{
    public interface INorthwind
    {
        public PagiOrderListModel GetOrderList(int pageNumber = 1, int pageSize = 10);
    }
}
