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
                throw new ArgumentException("File is required.");
            }
            if (!IsValidFileType(file, fileType))
            {
                throw new ArgumentException($"Invalid file type. Expected {fileType} files.");
            }
            if (file.Length > maxSizeAllowed)
            {
                throw new ArgumentException($"File size exceeds the maximum allowed size of {maxSizeAllowed} bytes.");
            }
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(_baseDirectory, fileType, fileName);

            Directory.CreateDirectory(Path.Combine(_baseDirectory, fileType));

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            Console.WriteLine(Path.Combine(fileType, fileName));
            return Path.Combine(fileType, fileName);
        }

        private bool IsValidFileType(IFormFile file, string fileType)
        {
            var allowedExtensions = fileType switch
            {
                "image" => new[] { ".jpg", ".jpeg", ".png", ".gif" },
                "document" => new[] { ".pdf", ".doc", ".docx", ".txt" },
                _ => Array.Empty<string>()
            };

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return Array.Exists(allowedExtensions, ext => ext == extension);
        }

    }
}
