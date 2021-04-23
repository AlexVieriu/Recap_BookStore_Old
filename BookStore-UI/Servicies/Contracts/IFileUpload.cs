using BlazorInputFile;
using System.IO;
using System.Threading.Tasks;

namespace BookStore_UI.Servicies.Contracts
{
    public interface IFileUpload
    {
        public Task UploadFile(IFileListEntry file, string picName);
        public void UploadFile(IFileListEntry file, MemoryStream msfile, string picName);
        public void RemoveFile(string picName);
    }
}
