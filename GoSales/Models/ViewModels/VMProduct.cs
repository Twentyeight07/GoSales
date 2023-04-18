using Entity;

namespace GoSales.Models.ViewModels
{
    public class VMProduct
    {
        public int ProductId { get; set; }

        public string? BarCode { get; set; }

        public string? Brand { get; set; }

        public string? Description { get; set; }

        public int? CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public int? Stock { get; set; }

        public string? PicUrl { get; set; }

        public string? Price { get; set; }

        public int? IsActive { get; set; }

    }
}
