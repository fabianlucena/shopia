using RFBase.Exceptions;

namespace backend_shopia.Exceptions;

public class IncomptatibleCommerceUUIDdAndIDException()
    : HttpException(400, "Incompatible Commerce UUID and ID.")
{
}
