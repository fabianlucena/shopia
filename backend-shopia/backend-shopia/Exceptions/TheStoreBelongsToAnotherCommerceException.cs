using RFHttpExceptions.Exceptions;

namespace backend_shopia.Exceptions
{
    public class TheStoreBelongsToAnotherCommerceException()
        : HttpException(400, "The store belongs to another commerce")
    {
    }
}
