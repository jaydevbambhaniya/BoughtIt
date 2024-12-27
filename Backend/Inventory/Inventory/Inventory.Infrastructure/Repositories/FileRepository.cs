using Domain.Common;
using Domain.Common.Resources;
using Domain.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly string _baseDirectory;
        public FileRepository(string baseDirectory)
        {
            _baseDirectory = baseDirectory;
        }
        public Task<string> DeleteFileAsync(string FileName)
        {
            File.Delete(FileName);
            return Task.FromResult("Deleted");
        }

        public async Task<string> UploadFileAsync(IFormFile file, string fileType, int maxSizeAllowed)
        {
            if (file == null || file.Length == 0)
            {
                var error = ErrorCodes.GetError("FILE_NOT_PROVIDED");
                throw new CustomException(error.Code,error.Message);
            }
            if (!IsValidFileType(file, fileType))
            {
                var error = ErrorCodes.GetError("INVALID_FILE_TYPE");
                throw new CustomException(error.Code,error.Message);
            }
            if (file.Length > maxSizeAllowed)
            {
                var error = ErrorCodes.GetError("FILE_LIMIT");
                throw new CustomException(error.Code,error.Message);
            }
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(_baseDirectory, fileType, fileName);

            Directory.CreateDirectory(Path.Combine(_baseDirectory, fileType));

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return Path.Combine(fileType, fileName);
        }

        private bool IsValidFileType(IFormFile file, string fileType)
        {
            var allowedExtensions = fileType switch
            {
                "image" => [".jpg", ".jpeg", ".png", ".gif"],
                "document" => [".pdf", ".doc", ".docx", ".txt"],
                _ => Array.Empty<string>()
            };

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return Array.Exists(allowedExtensions, ext => ext == extension);
        }

    }
}
