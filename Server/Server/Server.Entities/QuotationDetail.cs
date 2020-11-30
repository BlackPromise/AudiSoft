
namespace Server.Entities
{
    public class QuotationDetail
    {
        public int QuotationDetailId{ get; set; }
        public int QuotationId{ get; set; }
        public int ProductId{ get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public int Quantity{ get; set; }
        public double Amount{ get; set; }
        public double Tax{ get; set; }
        public double Discount{ get; set; }
        public double Total{ get; set; }
    }
}
