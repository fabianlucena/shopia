using AutoMapper;
using backend_shopia.DTO;
using backend_shopia.Entities;
using backend_shopia.Exceptions;
using backend_shopia.IServices;
using System.Globalization;

namespace backend_shopia
{
    public class StoreAddRequest_CommerceIdResolverAsync(ICommerceService commerceService)
        : IValueResolver<StoreAddRequest, Store, Int64>
    {
        public Int64 Resolve(
            StoreAddRequest source,
            Store destination,
            Int64 destMember,
            ResolutionContext context)
        {
            return commerceService.GetSingleOrDefaultIdForUuidAsync(source.CommerceUuid)?.Result
                ?? throw new CommerceDoesNotExistException();
        }
    }

    public class ItemAddRequest_CategoryIdResolverAsync(ICategoryService categoryService)
        : IValueResolver<ItemAddRequest, Item, Int64>
    {
        public Int64 Resolve(
            ItemAddRequest source,
            Item destination,
            Int64 destMember,
            ResolutionContext context)
        {
            return categoryService.GetSingleOrDefaultIdForUuidAsync(source.CategoryUuid)?.Result
                ?? throw new CategoryDoesNotExistException();
        }
    }

    public class ItemAddRequest_StoresResolverAsync(IStoreService storeService)
        : IValueResolver<ItemAddRequest, Item, IEnumerable<Store>?>
    {
        public IEnumerable<Store>? Resolve(
            ItemAddRequest source,
            Item destination,
            IEnumerable<Store>? destMember,
            ResolutionContext context)
        {
            if (source.StoresUuid == null
                || source.StoresUuid.Length <= 0
                || source.StoresUuid.Any(s => s == default)
            )
                return null;

            if (source.StoresUuid.Any(s => s == default))
                throw new NoStoreException();

            var stores = storeService.GetListForUuidsAsync(source.StoresUuid)?.Result
               ?? throw new StoreDoesNotExistException();

            return [..stores];
        }
    }

    public class MappingProfile
        : Profile
    {
        public MappingProfile()
        {
            CreateMap<CommerceAddRequest, Commerce>();
            CreateMap<Commerce, CommerceResponse>();

            CreateMap<StoreAddRequest, Store>()
                .ForMember(dest => dest.CommerceId, opt => opt.MapFrom<StoreAddRequest_CommerceIdResolverAsync>())
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location == null? null: src.Location.ToGeometryPoint()));
            CreateMap<Store, StoreResponse>();
            CreateMap<Store, StoreMinimalDTO>();

            CreateMap<Category, CategoryResponse>();
            CreateMap<Category, CategoryMinimalDTO>();

            CreateMap<ItemAddRequest, Item>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom<ItemAddRequest_CategoryIdResolverAsync>())
                .ForMember(dest => dest.Stores, opt => opt.MapFrom<ItemAddRequest_StoresResolverAsync>());
            CreateMap<Item, ItemResponse>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.ToString(CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.CategoryUuid, opt => opt.MapFrom(src => src.Category != null ? (Guid?)src.Category.Uuid : null))
                .ForMember(dest => dest.StoresUuid, opt => opt.MapFrom(src => src.Stores != null ? (Guid[]?)src.Stores.Select(s => s.Uuid) : null));

            CreateMap<Plan, PlanDTO>();
        }
    }
}
