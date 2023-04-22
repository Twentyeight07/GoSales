
namespace GoSales.Models.ViewModels
{
    public class VMSale
    {
        public int SaleId { get; set; }

        public string? SaleNumber { get; set; }

        public int? SaleDocTypeId { get; set; }

        public string? SaleDocType { get; set; }

        public int? UserId { get; set; }

        public string? User { get; set; }

        public string? ClientDoc { get; set; }

        public string? ClientName { get; set; }

        public string? SubTotal { get; set; }

        public string? TotalTax { get; set; }

        public string? Total { get; set; }

        public string? RegistryDate { get; set; }

        public virtual ICollection<VMSaleDetail> SaleDetail { get; set; }
        
    }
}
