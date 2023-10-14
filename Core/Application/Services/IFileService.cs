using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IFileService
    {
        Task<List<(string fileName, string path)>> UploadAsync(string filePath, IFormFileCollection files);
        Task<bool> CopyFileAsync(string filePath,IFormFile file);
    }
}
