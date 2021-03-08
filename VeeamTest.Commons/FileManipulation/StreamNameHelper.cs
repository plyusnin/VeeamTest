using System.IO;

namespace VeeamTest.Commons.FileManipulation
{
    internal static class StreamNameHelper
    {
        /// <summary>Возвращает наиболее подходящее строковое представления для потока, указанного в <see cref="ForStream" /></summary>
        /// <param name="ForStream">Поток, название которого требуется определить</param>
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