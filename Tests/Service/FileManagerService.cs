using System;
using System.Drawing;
using System.IO;
using Omu.Drawing;
using Omu.ProDinner.Core.Service;

namespace Omu.ProDinner.Service
{
    public class FileManagerService : IFileManagerService
    {
        private const string MealsPath = "/meals/";
        private const string TempPath = "/temp/";

        public void DeleteImages(string root, string filename)
        {
            var c = root + MealsPath;
            File.Delete(c + filename);
            File.Delete(c + "s" + filename);
            File.Delete(c + "m" + filename);
        }

        public void MakeImages(string root, string filename, int x, int y, int w, int h)
        {
            using (var image = Image.FromFile(root + TempPath + filename))
            {
                var c = root + MealsPath;
                var img = Imager.Crop(image, new Rectangle(x, y, w, h));
                var resized = Imager.Resize(img, 200, 150, true);
                var small = Imager.Resize(img, 100, 75, true);
                var mini = Imager.Resize(img, 45, 34, true);
                Imager.SaveJpeg(c + filename, resized);
                Imager.SaveJpeg(c + "s" + filename, small);
                Imager.SaveJpeg(c + "m" + filename, mini);
            }
        }

        public string SaveTempJpeg(string root, Stream inputStream, out int w, out int h)
        {
            var g = Guid.NewGuid() + ".jpg";
            var filePath = root + TempPath + g;
            using (var image = Image.FromStream(inputStream))
            {
                var resized = Imager.Resize(image, 533, 400, true);
                Imager.SaveJpeg(filePath, resized);

                w = resized.Width;
                h = resized.Height;
                return g;
            }
        }
    }
}