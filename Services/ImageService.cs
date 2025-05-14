using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace cloud_app_dev_exam_project.Services
{
    public static class ImageService
    {
        private static readonly string ImageFolderPath = Path.Combine(
            FileSystem.AppDataDirectory, "Images");

        static ImageService()
        {
            if (!Directory.Exists(ImageFolderPath))
                Directory.CreateDirectory(ImageFolderPath);
        }

        public static async Task<string?> PickAndSaveImageAsync()
        {
            try
            {
                var result = await FilePicker.Default.PickAsync(new PickOptions
                {
                    FileTypes = FilePickerFileType.Images,
                    PickerTitle = "Select an image"
                });

                if (result == null)
                    return null;

                string newFileName = $"{Guid.NewGuid()}{Path.GetExtension(result.FileName)}";
                string savedPath = Path.Combine(ImageFolderPath, newFileName);

                using var sourceStream = await result.OpenReadAsync();
                using var destStream = File.OpenWrite(savedPath);
                await sourceStream.CopyToAsync(destStream);

                return savedPath; 
            }
            catch (Exception ex)
            {
                
                return null;
            }
        }

        public static void DeleteImage(string imagePath)
        {
            if (File.Exists(imagePath))
                File.Delete(imagePath);
        }

        public static string GetFullPath(string fileName)
        {
            return Path.Combine(ImageFolderPath, fileName);
        }
    }
}
