using RFHttpExceptions.Exceptions;

namespace backend_shopia.Exceptions
{
    public class NoPlanUuidException()
        : HttpException(400, "No plan UUID provided.")
    {
    }
}
