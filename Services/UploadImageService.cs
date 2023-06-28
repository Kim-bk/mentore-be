using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Collections.Generic;

namespace API.Services
{
    public class UploadImageService
    {
        #region Image vs Video extensions
        private readonly List<string> ImageExtensions = new() { ".png", ".jpg", ".jpeg" };
        #endregion

        #region Cloudinary Informations

        private const string CLOUD_NAME = "dor7ghk95";
        private const string API_KEY = "588273259994552";
        private const string API_SECRET = "YImi-iuUxclgZJFC2-R0cN3tcEA";

        #endregion

        private Cloudinary _cloudinary;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public UploadImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnviroment = webHostEnvironment;
        }


        public string UploadFile(IFormFile file)
        {
            try
            {
                Account account = new(CLOUD_NAME, API_KEY, API_SECRET);
                _cloudinary = new Cloudinary(account);

                var fileUrl = "";
                if (!Directory.Exists(_webHostEnviroment.WebRootPath + "\\Images\\"))
                {
                    Directory.CreateDirectory(_webHostEnviroment.WebRootPath + "\\Images\\");
                }

                using (FileStream fileStream = System.IO.File.Create(_webHostEnviroment.WebRootPath + "\\Images\\" + file.FileName))
                {
                    file.CopyTo(fileStream);
                    fileStream.Flush();
                    fileUrl = _webHostEnviroment.WebRootPath + "\\Images\\" + file.FileName;
                }

                if (fileUrl != string.Empty)
                {
                    string extension = Path.GetExtension(file.FileName).ToLower();
                    if (ImageExtensions.Contains(extension))
                    {
                        var uploadParams = new ImageUploadParams
                        {
                            File = new FileDescription(fileUrl),
                        };

                        var uploadResult = _cloudinary.Upload(uploadParams);
                        return uploadResult.Url.ToString();
                    }
                }    
              
                return string.Empty;
            }
            catch
            {
                throw;
            }
        }
    }
}
