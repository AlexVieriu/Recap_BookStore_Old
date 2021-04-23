using BlazorInputFile;
using BookStore_UI.Servicies.Contracts;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BookStore_UI.Servicies
{
    public class FileUpload : IFileUpload
    {
        private readonly IWebHostEnvironment _webHost;

        public FileUpload(IWebHostEnvironment webHost)
        {
            _webHost = webHost;
        }

        public void RemoveFile(string picName)
        {
            var path = $"{_webHost}\\images\\{picName}";
            if (File.Exists(path))
                File.Delete(path);
        }

        public async Task UploadFile(IFileListEntry file, string picName)
        {
            try
            {
                var ms = new MemoryStream();
                await file.Data.CopyToAsync(ms);

                // build the Path
                var path = $"{_webHost.WebRootPath}\\images\\{picName}";

                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    ms.WriteTo(fs);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void UploadFile(IFileListEntry file, MemoryStream msfile, string picName)
        {
            try
            {
                var path = $"{_webHost.WebRootPath}\\images\\{picName}";
                using(FileStream fs = new FileStream(path, FileMode.Create))
                {
                    msfile.WriteTo(fs);
                }
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
