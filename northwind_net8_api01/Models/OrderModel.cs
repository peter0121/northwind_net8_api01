namespace northwind_net8_api01.Models
{
    public class OrderModel
    {
        public int OrderID { get; set; }
        public string CustomerID { get; set; } = "";
        public int EmployeeID { get; set; }
        public DateTime? OrderDate { get; set; }
        public string ShipCity { get; set; } = "";
    }
}
