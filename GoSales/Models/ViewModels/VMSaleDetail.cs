﻿using Entity;

namespace GoSales.Models.ViewModels
{
    public class VMSaleDetail
    {
        public int? ProductId { get; set; }

        public string? ProductBrand { get; set; }

        public string? ProductDescription { get; set; }

        public string? ProductCategory { get; set; }

        public int? Quantity { get; set; }

        public string? Price { get; set; }

        public string? Total { get; set; }

    }
}
