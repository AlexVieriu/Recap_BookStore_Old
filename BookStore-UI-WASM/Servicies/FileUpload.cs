using BookStore_UI_WASM.Servicies.Contracts;
using System.IO;
using System.Threading.Tasks;

namespace BookStore_UI_WASM.Servicies
{
    //public class FileUpload : IFileUpload
    //{
    //    private readonly IWebHostEnvironment _webHost;

    //    public FileUpload(IWebHostEnvironment webHost)
    //    {
    //        _webHost = webHost;
    //    }

    //    public void RemoveFile(string picName)
    //    {
    //        var path = $"{_webHost.WebRootPath}\\images\\{picName}";
    //        if (File.Exists(path))           
    //            File.Delete(path);
    //    }

    //    public async Task UploadFile(Stream file, string picName)
    //    {
    //        var path = $"{_webHost.WebRootPath}\\images\\{picName}";
    //        // merge din 4,384 in 4,384 bites citirea
    //        var buffer = new byte[4 * 1096];
    //        int bytesRead;
    //        double totalRead = 0;
    //        using FileStream fs = new FileStream(path, FileMode.Create);

    //        while ((bytesRead = await file.ReadAsync(buffer)) != 0)
    //        {
    //            totalRead += bytesRead;
    //            await fs.WriteAsync(buffer);
    //        }
    //    }
    //}
}
