using Entity;

namespace GoSales.Models.ViewModels
{
    public class VMUser
    {
        public int UserId { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public int? RoleId { get; set; }

        public string? RoleName { get; set; }

        public string? PicUrl { get; set; }

        public int? IsActive { get; set; }

        
    }
}
