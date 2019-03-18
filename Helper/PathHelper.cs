using System.IO;

namespace Helper
{
    public static class PathHelper
    {
        public static void CleanDirectories(params string[] paths)
        {
            foreach (var path in paths)
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }

                Directory.CreateDirectory(path);
            }
        }
    }
}
