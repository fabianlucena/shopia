using RFBase.Exceptions;

namespace backend_shopia.Exceptions;

public class MaxActiveStoresLimitReachedException()
    : HttpException(400, "The maximum limit of active stores has been reached.")
{
}
