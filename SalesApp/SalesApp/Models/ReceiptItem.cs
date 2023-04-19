
namespace SalesApp.Models
{
    public class ReceiptItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string TaxName { get; set; }
        public double Discount { get; set; }
        public string Quantity { get; set; }
        public string Unit { get; set; }
        public bool? IsManualValue { get; set; }
    }
}
