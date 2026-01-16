using RFHttpExceptions.Exceptions;

namespace backend_shopia.Exceptions
{
    public class SomeStoreDoesNotExistException()
        : HttpException(400, "Some store does not exist")
    {
    }
}
