using RFBase.Exceptions;

namespace backend_shopia.Exceptions;

public class ItemDoesNotExistException()
    : HttpException(400, "Item does not exist.")
{
}
