using backend_shopia.DTO;
using backend_shopia.Entities;
using backend_shopia.Exceptions;
using backend_shopia.IRepositories;
using backend_shopia.IServices;
using backend_shopia.QueryOptions;
using RFServices.Services;

namespace backend_shopia.Services;

public class CommerceFileService(
    ICommerceFileRepository commerceFileRepository,
    IServiceProvider serviceProvider
)
    : CreatableWithNameEntityService<CommerceFile>(commerceFileRepository, serviceProvider),
    ICommerceFileService
{
    public override async Task<CommerceFile> ValidateForCreateAsync(CommerceFile data)
    {
        data = await base.ValidateForCreateAsync(data);

        if (string.IsNullOrWhiteSpace(data.Name))
            throw new NoNameException();

        if (data.Commerce == null)
        {
            if (data.CommerceId <= 0)
                throw new NoCommerceException();

            var commerceService = ServiceProvider.GetRequiredService<ICommerceService>();
            data.Commerce = await commerceService.GetSingleOrDefaultByIdAsync(data.CommerceId)
                ?? throw new NoCommerceException();
        }

        var userPlanService = ServiceProvider.GetRequiredService<IUserPlanService>();
        var limits = await userPlanService.GetLimitsByCurrentUserAsync();

        if (data.Content.Length > limits[PlanLimitName.MaxCommerceImageSize])
            throw new ImageIsTooLargeException(data.Content.Length, limits[PlanLimitName.MaxCommerceImageSize]);

        var itemImagesCount = await GetCountAsync(new CommerceFileQueryOptions
        {
            IncludeInactive = true,
            CommerceId = data.CommerceId,
        });
        if (itemImagesCount >= limits[PlanLimitName.MaxTotalImagesPerSingleCommerce])
            throw new TotalImagesPerCommerceLimitReachedException();

        var totalCount = await GetCountByCurrentUserAsync(new CommerceFileQueryOptions { IncludeInactive = true });
        if (totalCount >= limits[PlanLimitName.MaxTotalCommercesImages])
            throw new TotalCommercesImagesLimitReachedException();

        var enabledCount = await GetCountByCurrentUserAsync();
        if (enabledCount > limits[PlanLimitName.MaxEnabledCommercesImages])
            throw new MaxEnabledCommercesImagesLimitReachedException();

        var aggregatedSize = await GetAggregatedSizeByCurrentUserAsync(new CommerceFileQueryOptions { IncludeInactive = true });
        aggregatedSize += data.Content.Length;
        if (aggregatedSize >= limits[PlanLimitName.MaxCommercesImagesAggregatedSize])
            throw new TotalCommercesImagesAggregatedSizeLimitReachedException();

        var enabledAggregatedSize = await GetAggregatedSizeByCurrentUserAsync();
        enabledAggregatedSize += data.Content.Length;
        if (enabledAggregatedSize >= limits[PlanLimitName.MaxEnabledCommercesImagesAggregatedSize])
            throw new MaxEnabledCommercesImagesAggregatedSizeLimitReachedException();

        return data;
    }

    public async Task<CommerceFileQueryOptions> GetFilterByOwnerIdAsync(long ownerId, CommerceFileQueryOptions? options = null)
    {
        var commerceService = ServiceProvider.GetRequiredService<ICommerceService>();
        var commercesId = await commerceService.GetListIdByOwnerIdAsync(ownerId, new CommerceQueryOptions
        {
            IncludeInactive = options?.IncludeInactive ?? false,
            Id = options?.CommerceId,
            Ids = options?.CommercesId,
        });

        options ??= new CommerceFileQueryOptions();
        options.CommercesId = commercesId;

        return options;
    }

    public async Task<int> GetCountByOwnerIdAsync(long ownerId, CommerceFileQueryOptions? options = null)
        => await GetCountAsync(await GetFilterByOwnerIdAsync(ownerId, options));

    public async Task<int> GetCountByCurrentUserAsync(CommerceFileQueryOptions? options = null)
        => await GetCountByOwnerIdAsync(await GetCurrentUserIdAsync(), options);

    public async Task<long> GetAggregatedSizeByOwnerIdAsync(long ownerId, CommerceFileQueryOptions? options = null)
    {
        options = await GetFilterByOwnerIdAsync(ownerId, options);
        return await commerceFileRepository.GetAggregatedSizeAsync(options);
    }

    public async Task<long> GetAggregatedSizeByCurrentUserAsync(CommerceFileQueryOptions? options = null)
        => await GetAggregatedSizeByOwnerIdAsync(await GetCurrentUserIdAsync(), options);

    public async Task<IEnumerable<CommerceFile>> AddByCommerceUuidAsync(Guid commerceUuid, FilesCollectionDTO files)
    {
        var commerceService = ServiceProvider.GetRequiredService<ICommerceService>();
        return await AddByCommerceIdAsync(await commerceService.GetSingleIdByUuidAsync(commerceUuid), files);
    }

    public async Task<IEnumerable<CommerceFile>> AddByCommerceIdAsync(long commerceId, FilesCollectionDTO files)
    {
        var result = new List<CommerceFile>();
        foreach (var file in files)
        {
            if (file.Content.Length == 0)
                continue;

            var commerceImage = new CommerceFile
            {
                CommerceId = commerceId,
                Name = file.Name,
                ContentType = file.ContentType,
                Content = file.Content,
            };

            result.Add(await CreateAsync(commerceImage));
        }

        return result;
    }

    public async Task<IEnumerable<CommerceFile>> GetListByCommerceIdAsync(long commerceId)
        => await GetListAsync(new CommerceFileQueryOptions
        {
            CommerceId = commerceId
        });
}