using RFBase.Exceptions;

namespace backend_shopia.Exceptions;

public class MaxActiveItemsLimitReachedException()
    : HttpException(400, "The maximum limit of active items has been reached.")
{
}
