using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MilanaBoutique.Helpers
{
    public class Helper
    {
        public static void DeleteImg(string root, string folder, string imageName)
        {


            string fullPath = Path.Combine(root, folder, imageName);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}
