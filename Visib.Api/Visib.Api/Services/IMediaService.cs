using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Visib.Api.Services
{
    public interface IMediaService
    {
        Task UploadAsync(string fileName, MemoryStream file);
    }
}