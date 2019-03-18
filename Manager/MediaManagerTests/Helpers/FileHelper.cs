using System.IO;
using System.Reflection;

namespace MediaManagerTests.Helpers
{
    public static class FileHelper
    {
        public static Stream GetInputFile(string filename)
        {
            Assembly thisAssembly = Assembly.GetExecutingAssembly();

            string path = "MediaManagerTests.InputFiles";

            return thisAssembly.GetManifestResourceStream(path + "." + filename);
        }

        public static void SaveFile(Stream file, string filePath)
        {
            using (var fileStream = File.Create(filePath))
            {
                file.Seek(0, SeekOrigin.Begin);
                file.CopyTo(fileStream);
            }
        }
    }
}
