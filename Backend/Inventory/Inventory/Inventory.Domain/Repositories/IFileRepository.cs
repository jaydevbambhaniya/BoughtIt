using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IFileRepository
    {
        public Task<string> UploadFileAsync(IFormFile File,string FileType,int MaxSizeAllowed);
        public Task<string> DeleteFileAsync(string FileName);
    }
}
