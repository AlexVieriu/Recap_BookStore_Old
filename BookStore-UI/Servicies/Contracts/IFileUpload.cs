using BlazorInputFile;
using System.Threading.Tasks;

namespace BookStore_UI.Servicies.Contracts
{
    public interface IFileUpload
    {
        public Task UploadFile(IFileListEntry file, string picName);
    }
}
