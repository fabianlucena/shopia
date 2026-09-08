using RFIServices.QueryOptions;

namespace backend_shopia.QueryOptions;

public class CategoryQueryOptions : NominableEntityQueryOptions
{
    public CategoryQueryOptions() { }

    public CategoryQueryOptions(CategoryQueryOptions? options)
        : base(options)
    {
        if (options is null)
            return;
    }

    public override CategoryQueryOptions Clone()
        => new(this);
}
