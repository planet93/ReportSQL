using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace BMI.Services.Util
{
    public class IO
    {
        public static string GetPath(string basePath, string fileName)
        {
            var fileExtension = GetExtensionFromString(fileName);
            var newFileName = RandomGenerator.RandomString();

            return $"{basePath}{newFileName}{fileExtension}";
        }

        public static string GetExtensionFromString(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return string.Empty;
            }
            var delimiter = fileName.LastIndexOf(".", StringComparison.Ordinal);
            return fileName.Substring(delimiter >= 0 ? delimiter : 0);
        }

        public static string FileNameWithoutExtension(string fileName)
        {
            var fileExtension = GetExtensionFromString(fileName);
            return fileName.Substring(0, fileName.Length - fileExtension.Length);
        }
        /// <summary>
        /// Наличие файла по расширению файла
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <param name="extensions"></param>
        /// <returns></returns>
        public static bool FileExistByExtensions(DirectoryInfo directoryInfo, params string[] extensions)
        {
            if (extensions == null)
            {
                return false;
            }
            var files = directoryInfo.EnumerateFiles();
            var resFiles = files.Where(f => extensions.Contains(f.Extension));

            return resFiles.Count() != 0;
        }
    }
}