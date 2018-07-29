using Microsoft.AspNetCore.Http;
using System.IO;

namespace ContentManager
{
    public static class FormFileExtensionMethods
    {
        public static void Save(this IFormFile file, string filePath)
        {
            if (file.Length > 0)
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
        }
    }
}
