using GoSales.Models.ViewModels;
using Entity;
using System.Globalization;
using AutoMapper;

namespace GoSales.Utilities.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        // This AutoMapper profile is to map all the data we have in Entity Layer in our Classes to the ViewModels, it convert all the data we need to convert and pass all data we need
        public AutoMapperProfile()
        {
            #region Role
            CreateMap<Role, VMRole>().ReverseMap();
            #endregion

            #region User
            CreateMap<User, VMUser>()
                .ForMember(dest => 
                dest.IsActive, 
                opt => opt.MapFrom(origin => origin.IsActive == true ? 1 : 0)
                )
                .ForMember(dest => 
                dest.RoleName,
                opt => opt.MapFrom(origin => origin.RoleIdNavigation.Description)
                );

            CreateMap<VMUser, User>()
                .ForMember(dest =>
                dest.IsActive,
                opt => opt.MapFrom(origin => origin.IsActive == 1 ? true : false)
                )
                .ForMember(dest => 
                dest.RoleIdNavigation,
                opt => opt.Ignore()
                );

            #endregion

            #region Business
            CreateMap<Business, VMBusiness>()
                .ForMember(dest =>
                dest.TaxRate,
                opt => opt.MapFrom(origin => Convert.ToDecimal(origin.TaxRate, new CultureInfo("es-US")))
                );

            CreateMap<VMBusiness, Business>()
                .ForMember(dest =>
                dest.TaxRate,
                opt => opt.MapFrom(origin => Convert.ToString(origin.TaxRate, new CultureInfo("es-US")))
                );

            #endregion

            #region Category
            CreateMap<Category, VMCategory>()
                .ForMember(dest =>
                dest.IsActive,
                opt => opt.MapFrom(origin => origin.IsActive == true ? 1 : 0)
                );

            CreateMap<VMCategory, Category>()
                .ForMember(dest =>
                dest.IsActive,
                opt => opt.MapFrom(origin => origin.IsActive == 1 ? true : false)
                );

            #endregion

            #region Product
            CreateMap<Product, VMProduct>()
                .ForMember(dest =>
                dest.IsActive,
                opt => opt.MapFrom(origin => origin.IsActive == true ? 1 : 0)
                )
                .ForMember(dest =>
                 dest.CategoryName,
                opt => opt.MapFrom(origin => origin.Category.Description)
                )
                .ForMember(dest =>
                dest.Price,
                opt => opt.MapFrom(origin => Convert.ToString(origin.Price.Value, new CultureInfo("es-US")))
                );

            CreateMap<VMProduct, Product>()
                .ForMember(dest =>
                dest.IsActive,
                opt => opt.MapFrom(origin => origin.IsActive == 1 ? true : false)
                )
                .ForMember(dest =>
                 dest.Category,
                opt => opt.Ignore()
                )
                .ForMember(dest =>
                dest.Price,
                opt => opt.MapFrom(origin => Convert.ToDecimal(origin.Price, new CultureInfo("es-US")))
                );

            #endregion

            #region SaleDocType
            CreateMap<SaleDocType, VMSaleDocType>().ReverseMap();

            #endregion

            #region Sale
            CreateMap<Sale, VMSale>()
                .ForMember(dest =>
                dest.SaleDocType,
                opt => opt.MapFrom(origin => origin.SaleDocType.Description)
                )
                .ForMember(dest =>
                dest.User,
                opt => opt.MapFrom(origin => origin.User.Name)
                )
                .ForMember(dest =>
                dest.SubTotal,
                opt => opt.MapFrom(origin => Convert.ToString(origin.SubTotal.Value, new CultureInfo("es-US")))
                )
                .ForMember(dest =>
                dest.TotalTax,
                opt => opt.MapFrom(origin => Convert.ToString(origin.TotalTax.Value, new CultureInfo("es-US")))
                )
                .ForMember(dest =>
                dest.Total,
                opt => opt.MapFrom(origin => Convert.ToString(origin.Total.Value, new CultureInfo("es-US")))
                )
                .ForMember(dest =>
                dest.RegistryDate,
                opt => opt.MapFrom(origin => origin.RegistryDate.Value.ToString("dd/MM/yyyy"))
                );

            CreateMap<VMSale, Sale>()
                .ForMember(dest =>
                dest.SubTotal,
                opt => opt.MapFrom(origin => Convert.ToDecimal(origin.SubTotal, new CultureInfo("es-US")))
                )
                .ForMember(dest =>
                dest.TotalTax,
                opt => opt.MapFrom(origin => Convert.ToDecimal(origin.TotalTax, new CultureInfo("es-US")))
                )
                .ForMember(dest =>
                dest.Total,
                opt => opt.MapFrom(origin => Convert.ToDecimal(origin.Total, new CultureInfo("es-US")))
                );

            #endregion

            #region SaleDetail
            CreateMap<SaleDetail, VMSaleDetail>()
                .ForMember(dest =>
                dest.Price,
                opt => opt.MapFrom(origin => Convert.ToString(origin.Price.Value, new CultureInfo("es-US")))
                )
                .ForMember(dest =>
                dest.Total,
                opt => opt.MapFrom(origin => Convert.ToString(origin.Total.Value, new CultureInfo("es-US")))
                );

            CreateMap<VMSaleDetail, SaleDetail>()
                .ForMember(dest =>
                dest.Price,
                opt => opt.MapFrom(origin => Convert.ToDecimal(origin.Price, new CultureInfo("es-US")))
                )
                .ForMember(dest =>
                dest.Total,
                opt => opt.MapFrom(origin => Convert.ToDecimal(origin.Total, new CultureInfo("es-US")))
                );

            CreateMap<SaleDetail, VMSaleReport>()
                .ForMember(dest =>
                dest.RegistryDate,
                opt => opt.MapFrom(origin => origin.Sale.RegistryDate.Value.ToString("dd/MM/yyyy"))
                )
                .ForMember(dest =>
                dest.SaleNumber,
                opt => opt.MapFrom(origin => origin.Sale.SaleNumber)
                )
                .ForMember(dest =>
                dest.DocType,
                opt => opt.MapFrom(origin => origin.Sale.SaleDocType.Description)
                )
                .ForMember(dest =>
                dest.ClientDocument,
                opt => opt.MapFrom(origin => origin.Sale.ClientDoc)
                )
                .ForMember(dest =>
                dest.ClientName,
                opt => opt.MapFrom(origin => origin.Sale.ClientName)
                )
                .ForMember(dest =>
                dest.SaleSubTotal,
                opt => opt.MapFrom(origin => Convert.ToString(origin.Sale.SubTotal.Value, new CultureInfo("es-US")))
                )
                .ForMember(dest =>
                dest.TotalSalesTax,
                opt => opt.MapFrom(origin => Convert.ToString(origin.Sale.TotalTax.Value, new CultureInfo("es-US")))
                )
                .ForMember(dest =>
                dest.TotalSale,
                opt => opt.MapFrom(origin => Convert.ToString(origin.Sale.Total.Value, new CultureInfo("es-US")))
                )
                .ForMember(dest =>
                dest.Product,
                opt => opt.MapFrom(origin => origin.ProductDescription)
                )
                .ForMember(dest =>
                dest.Price,
                opt => opt.MapFrom(origin => Convert.ToString(origin.Price.Value, new CultureInfo("es-US")))
                )
                .ForMember(dest =>
                dest.Total,
                opt => opt.MapFrom(origin => Convert.ToString(origin.Total.Value, new CultureInfo("es-US")))
                );

            #endregion

            #region Menu
            CreateMap<Menu, VMMenu>()
                .ForMember(dest =>
                dest.SubMenus,
                opt => opt.MapFrom(origin => origin.InverseIdParentMenuNavigation)
                );

            #endregion

            #region Notifications
            CreateMap<Notification, VMNotification>()
                .ForMember(dest =>
                dest.CreatedAt,
                opt => opt.MapFrom(origin => origin.CreatedAt.Value.ToString("dd/MM/yyyy"))
                );

            #endregion

        }
    }
}
