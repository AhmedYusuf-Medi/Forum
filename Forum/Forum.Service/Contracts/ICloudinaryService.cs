using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Forum.Service.Contracts
{
    public interface ICloudinaryService
    {
        Task<string[]> UploadPictureAsync(IFormFile pictureFile, string fileName, string username);
        Task DeleteImageAsync(string fileName);
    }
}