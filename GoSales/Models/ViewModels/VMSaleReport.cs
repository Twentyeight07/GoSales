namespace GoSales.Models.ViewModels
{
    public class VMSaleReport
    {
        public string? RegistryDate { get; set; }
        public string? SaleNumber { get; set;}
        public string? DocType { get; set; }
        public string? ClientDocument { get; set; }
        public string? ClientName { get; set; }
        public string? SaleSubTotal { get; set; }
        public string? TotalSalesTax { get; set; }
        public string? TotalSale { get; set; }
        public string? Product { get; set; }
        public int Quantity { get; set; }
        public string? Price { get; set; }
        public string? Total { get; set; }
    }
}
