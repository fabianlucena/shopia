using RFBase.Exceptions;

namespace backend_shopia.Exceptions;

public class NoStoreProvidedException()
    : HttpException(400, "No store provided.")
{
}
