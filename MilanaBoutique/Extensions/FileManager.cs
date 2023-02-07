using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace MilanaBoutique.Extensions
{
    public static class FileManager
    {
        public static bool IsImage(this IFormFile file)
        {
            return file.ContentType.Contains("image/");
        }

        public static bool IsSizeOkay(this IFormFile file, int mb)
        {

            return file.Length / 1024 / 1024 <= mb;

        }

        public static string SaveImg(this IFormFile file, string root, string folder)
        {
            string RootPath = Path.Combine(root, folder);
            string FileName = Guid.NewGuid().ToString() + file.FileName;
            string FullPath = Path.Combine(RootPath, FileName);

            using (FileStream fileStream = new FileStream(FullPath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return FileName;


        }
    }
}
