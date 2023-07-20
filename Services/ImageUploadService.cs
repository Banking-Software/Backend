using System.Reflection;
using MicroFinance.Enums;

namespace MicroFinance.Services
{
    public class ImageUploadService
    {
        private IConfiguration _config;

        public ImageUploadService(IConfiguration config)
        {
            _config = config;
        }
        public Task<T> UploadImage<T>(T entity, IFormFile? image, List<string> listOfPropertyName) where T : class
        {
            if (image == null)
                return Task.FromResult(entity);

            string fileExtenstion = (Path.GetExtension(image.FileName)).Replace(".", "").ToUpper();
            try
            {
                float maxFileValue = float.Parse(_config["ApplicationSettings:ImageMaxSize"]);
                double maxFileSize = maxFileValue * 1024 * 1024; // 3MB 
                if (image.Length > maxFileSize)
                    throw new Exception($"File size exceeded the Limit. Upto {maxFileSize}MB is allowed while {image.Length}MB is received");

                var fileType = (FileType)Enum.Parse(typeof(FileType), fileExtenstion);
                using (var stream = new MemoryStream())
                {
                    image.CopyTo(stream);
                    SetPropertyByName(entity, listOfPropertyName[0], stream.ToArray());
                    SetPropertyByName(entity, listOfPropertyName[1], image.FileName);
                    SetPropertyByName(entity, listOfPropertyName[2], fileType);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Given Image Format is wrong. {ex.Message}");
            }
            return Task.FromResult(entity);
        }

        private static void SetPropertyByName(object obj, string propertyName, object newValue)
        {
            Type type = obj.GetType();
            PropertyInfo propertyInfo = type.GetProperty(propertyName);
            if (propertyInfo != null && propertyInfo.CanWrite)
                propertyInfo.SetValue(obj, newValue);
            else
                throw new Exception($"Property '{propertyName}' not found or not writable.");
        }

    }
}