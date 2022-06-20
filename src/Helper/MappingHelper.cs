using AutoMapper;
using MarketMedia.src.Entities;
using MarketMedia.src.Models;

namespace MarketMedia.src.Helper
{
    public class MappingHelper : Profile
    {
        public MappingHelper()
        {
            CreateMap<AddressInputDto, Address>();
            //CreateMap<AddressOutputDto, Address>();
            CreateMap<Address, AddressOutputDto>();
            //.ForMember(i => i.VillageName, j => j.MapFrom(m => m.Village.Name));


            CreateMap<BranchInputDto, Branch>();
            CreateMap<Branch, BranchOutputDto>();
            //    .ForMember(i => i.Village, j => j.MapFrom(m => m.Village.Name))
            //    .ForMember(i => i.Seller, j => j.MapFrom(m => m.Seller.Name));

            CreateMap<Category, Category>();
            CreateMap<CategoryInputDto, Category>();
            CreateMap<Category, CategoryOutputDto>();

            CreateMap<CellInputDto, Cell>();
            CreateMap<Cell, CellOutputDto>();
            //    .ForMember(i => i.Sector, j => j.MapFrom(m => m.Sector.Name));

            CreateMap<ContactInputDto, Contact>();
            CreateMap<Contact, ContactOutputDto>();

            CreateMap<CustomerInputDto, Customer>();
            CreateMap<Customer, CustomerOutputDto>();
            //    .ForMember(i => i.Address, j => j.MapFrom(m => m.Address.Details))
            //    .ForMember(i => i.Contact, j => j.MapFrom(m => m.Contact.Email+" "+m.Contact.phone));

            CreateMap<DistrictInputDto, District>();
            CreateMap<District, DistrictOutputDto>();
            //    .ForMember(i => i.Province, j => j.MapFrom(m => m.Province.Name));

            CreateMap<OrderInputDto, Order>();
            CreateMap<Order, OrderOutputDto>();
            //    .ForMember(i => i.Customer, j => j.MapFrom(m => m.Customer.Name))
            //    .ForMember(i => i.Product, j => j.MapFrom(m => m.Product.Name))
            //    .ForMember(i => i.Payment, j => j.MapFrom(m => m.Payment.Name));

            CreateMap<PaymentInputDto, Payment>();
            CreateMap<Payment, PaymentOutputDto>();

            CreateMap<ProductInputDto, Product>();
            CreateMap<Product, ProductOutputDto>();
            //    .ForMember(i => i.Category, j => j.MapFrom(m => m.Category.Name));
            ////.ForMember(n=>n.ProductCode, d=>d.MapFrom(p=>p.Id+"/"+p.Name+"/"+p.Category.Name));

            CreateMap<ProductSellerInputDto, ProductSeller>();
            CreateMap<ProductSeller, ProductSellerOutputDto>();
            //    .ForMember(i => i.Product, j => j.MapFrom(m => m.Product.Name))
            //    .ForMember(i => i.Seller, j => j.MapFrom(m => m.Seller.Name));

            CreateMap<ProvinceInputDto, Province>();
            CreateMap<Province, ProvinceOutputDto>();

            CreateMap<SectorInputDto, Sector>();
            CreateMap<Sector, SectorOutputDto>();
            //    .ForMember(i => i.District, j => j.MapFrom(m => m.District.Name));

            CreateMap<SellerInputDto, Seller>();
            CreateMap<Seller, SellerOutputDto>();

            CreateMap<VillageInputDto, Village>();
            CreateMap<Village, VillageOutputDto>();
            //    .ForMember(i => i.Cell, j => j.MapFrom(m => m.Cell.Name));

        }
    }
}
