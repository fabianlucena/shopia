namespace backend_shopia.DTO;

public class UsedPlanDTO
{
    public int? TotalCommercesCount { get; set; }

    public int? EnabledCommercesCount { get; set; }

    public int? TotalStoresCount { get; set; }

    public int? EnabledStoresCount { get; set; }

    public int? TotalItemsCount { get; set; }

    public int? EnabledItemsCount { get; set; }

    public int? TotalItemsImagesCount { get; set; }

    public int? EnabledItemsImagesCount { get; set; }

    public long? ItemsImagesAggregatedSize { get; set; }

    public long? EnabledItemsImagesAggregatedSize { get; set; }

    public int? TotalCommercesImagesCount { get; set; }

    public int? EnabledCommercesImagesCount { get; set; }

    public long? CommercesImagesAggregatedSize { get; set; }

    public long? EnabledCommercesImagesAggregatedSize { get; set; }
}