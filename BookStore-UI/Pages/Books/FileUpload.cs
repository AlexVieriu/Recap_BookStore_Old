using BlazorInputFile;
using BookStore_UI.Servicies.Contracts;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BookStore_UI.Pages.Books
{
    public class FileUpload : IFileUpload
    {
        private readonly IWebHostEnvironment _web;

        public FileUpload(IWebHostEnvironment web)
        {
            _web = web;
        }

        public async Task UploadFile(IFileListEntry file, string picName)
        {
            try
            {
                var ms = new MemoryStream();
                await file.Data.CopyToAsync(ms);

                // build the Path
                var path = $"{_web.WebRootPath}\\images\\{picName}";

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
    }
}
