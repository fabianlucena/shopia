using RFIServices.QueryOptions;

namespace backend_shopia.QueryOptions;

public class CommerceFileQueryOptions : ANominableEntityQueryOptions
{
    public long? CommerceId { get; set; }
    public IEnumerable<long>? CommercesId { get; set; }

    public CommerceFileQueryOptions() { }

    public CommerceFileQueryOptions(CommerceFileQueryOptions? options)
        : base(options)
    {
        if (options is null)
            return;

        CommerceId = options.CommerceId;
        CommercesId = options.CommercesId;
    }

    public override CommerceFileQueryOptions Clone()
        => new(this);

    public override CommerceFileQueryOptions UpdateFromRequest(HttpRequest request)
    {
        base.UpdateFromRequest(request);

        return this;
    }
}
