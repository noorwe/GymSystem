using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GymSystemBLL.AttachmentService
{
    public class AttachmentService : IAttachmentService
    {
        private readonly string[] allowedExtensions = { ".png", ".jpg", ".jpeg" };
        private readonly long maxFileSize = 5 * 1024 * 1024; // 5MB


        public string? Upload(string folderName, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return null;

                if (file.Length > maxFileSize)
                    return null;

                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                    return null;


                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", folderName);

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var fileName = $"{Guid.NewGuid()}{extension}";

                var filePath = Path.Combine(folderPath, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                
                file.CopyTo(stream);
                

                return fileName;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Failed To Upload Photo: {ex}");
                return null;
            }
        }

        public bool Delete(string fileName, string folderName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderName))
                    return false;

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", folderName, fileName);

                if (!File.Exists(filePath))
                    return false;

                File.Delete(filePath);
                return true;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Failed To Delete Photo: {ex}");
                return false;
            }
        }
    }
}
