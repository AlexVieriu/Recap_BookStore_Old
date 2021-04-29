using System.IO;
using System.Threading.Tasks;

namespace BookStore_UI_WASM.Pages._Theory.ImageUpload
{
    public interface IFileUploadTest
    {
        void RemoveImg(string picName);
        Task UploadImg(Stream sFile, string picName);
    }
}
