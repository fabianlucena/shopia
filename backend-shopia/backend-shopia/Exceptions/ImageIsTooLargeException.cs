using RFHttpExceptions.Exceptions;

namespace backend_shopia.Exceptions
{
    public class ImageIsTooLargeException(long length, long max)
        : HttpException(400, "The image is too large ({0} >= {1})", lengthFormat(length), lengthFormat(max))
    {
        static string lengthFormat(long num)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = num;
            int order = 0;
            while (len >= 1000 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1000;
            }
            string result = String.Format("{0:0.##} {1}", len, sizes[order]);

            return result;
        }
    }
}
