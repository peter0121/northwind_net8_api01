namespace northwind_net8_api01.Models
{
    public class PagiOrderListModel
    {
        public int pagenumber { get; set; }
        public int pagesize { get; set; }
        public int totalpages { get; set; }
        public int total { get; set; }
        public IList<OrderModel> orders { get; set; } = new List<OrderModel>();
    }
}
