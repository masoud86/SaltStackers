using System.Collections.Generic;
using System.IO;

namespace SaltStackers.Common.Helper
{
    public static class FileHelper
    {
        public static List<string> GetFilesFromDirectory(string path)
        {
            var result = new List<string>();
            var filePaths = Directory.GetFiles(path);

            foreach (string filePath in filePaths)
            {
                result.Add(Path.GetFileName(filePath));
            }
            return result;
        }

        public static string GetExtension(this string path)
        {
            var ret = "";
            for (; ; )
            {
                var ext = Path.GetExtension(path);
                if (String.IsNullOrEmpty(ext))
                    break;
                path = path.Substring(0, path.Length - ext.Length);
                ret = ext + ret;
            }
            return ret;
        }
    }
}
