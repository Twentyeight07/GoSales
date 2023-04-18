namespace GoSales.Models.ViewModels
{
    public class VMDashBoard
    {
        public int TotalSales { get; set; }
        public string? TotalIncome { get; set; }
        public int TotalProducts { get; set; }
        public int TotalCategories { get; set;}

        public List<VMWeekSales> LastWeekSales { get; set; }
        public List<VMWeekProducts> LastWeekTopProducts { get; set; }
    }
}
