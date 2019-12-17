using System.Threading.Tasks;

namespace Visib.Mobile.Services
{
    public interface IMediaService
    {
        Task Upload(byte[] data, string fileName);
    }
}