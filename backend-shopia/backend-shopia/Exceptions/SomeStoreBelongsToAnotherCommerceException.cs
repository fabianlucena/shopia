using RFHttpExceptions.Exceptions;

namespace backend_shopia.Exceptions
{
    public class SomeStoreBelongsToAnotherCommerceException()
        : HttpException(400, "Some store belongs to another commerce")
    {
    }
}
