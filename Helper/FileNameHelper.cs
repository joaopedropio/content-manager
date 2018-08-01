using System.Runtime.InteropServices;

namespace Helper
{
    public static class FileNameExtensions
    {
        public static string GetFileNameFromFilePath(string filePath)
        {
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            var fields = (isWindows) ? filePath.Split('\\') : filePath.Split('/');
            return fields[fields.Length - 1];
        }

        public static string GetFileExtention(string fileName)
        {
            var fields = fileName.Split('.');

            if (fields.Length < 2)
                return string.Empty;

            return fields[fields.Length - 1];
        }

        public static string GetNameFromFileName(string fileName)
        {
            var fields = fileName.Split('.');

            if (fields.Length < 2)
                return string.Empty;

            return fields[0];
        }

        public static string BuildFilePath(string path, string fileName)
        {
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            var separator = isWindows ? '\\' : '/';
            return path.EndsWith(separator) ? path + fileName : path + separator + fileName;
        }
    }
}
