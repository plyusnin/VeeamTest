using System.IO;

namespace VeeamTest.Commons.FileManipulation
{
    internal static class StreamNameHelper
    {
        public static string GetStreamName(Stream ForStream)
        {
            switch (ForStream)
            {
                case FileStream fs:
                    return Path.GetFileName(fs.Name);

                case MemoryStream ms:
                    return $"{ms.Length} Bytes";

                default:
                    return "Stream";
            }
        }
    }
}