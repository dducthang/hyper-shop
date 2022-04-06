using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.Utility
{
    public class ImageWorking
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        public ImageWorking(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public string SaveImage(string folder, IFormFile file, string oldImageUrl = null)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;

            string fileName = Guid.NewGuid().ToString();
            string uploads = Path.Combine(wwwRootPath, folder);
            string extension = Path.GetExtension(file.FileName);

            if (oldImageUrl != null)
            {
                var oldImagePath = Path.Combine(wwwRootPath, oldImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
            {
                file.CopyTo(fileStreams);
            }

            return folder + fileName + extension;
        }

        public void Image_resize(string input_Image_Path, string output_Image_Path, int width = 473, int height = 592)
        {
            using (var image = Image.Load(_hostEnvironment.WebRootPath+input_Image_Path))
            {
                image.Mutate(x => x
                     .Resize(image.Width > width ? width : image.Width, image.Height > height ? height : image.Height));
                image.Save(_hostEnvironment.WebRootPath+output_Image_Path);
            }
        }
        
    }
}
