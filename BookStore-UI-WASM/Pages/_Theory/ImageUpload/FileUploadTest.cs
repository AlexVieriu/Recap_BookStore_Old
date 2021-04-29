using System.IO;
using System.Threading.Tasks;

namespace BookStore_UI_WASM.Pages._Theory.ImageUpload
{
    //public class FileUploadTest : IFileUploadTest
    //{
    //    private readonly IWebHostEnvironment _webHost;

    //    public FileUploadTest(IWebHostEnvironment webHost)
    //    {
    //        _webHost = webHost;
    //    }

    //    public void RemoveImg(string picName)
    //    {
    //        var path = $"{_webHost.WebRootPath}\\images\\{picName}";
    //        if (File.Exists(path))
    //            File.Delete(path);
    //    }

    //    public async Task UploadImg(Stream file, string picName)
    //    {
    //        var path = $"{_webHost.WebRootPath}\\images\\{picName}";
    //        var buffer = new byte[4 * 1096];
    //        int bytesRead;
    //        double totalRead = 0;

    //        using FileStream fs = new FileStream(path, FileMode.Create);

    //        while((bytesRead = await file.ReadAsync(buffer)) != 0)
    //        {
    //            totalRead += bytesRead;
    //            await fs.WriteAsync(buffer);
    //        }
    //    }
    //}
}
