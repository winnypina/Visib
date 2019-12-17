using System;
using System.Threading.Tasks;

namespace Visib.Mobile.Services
{
    public interface ICompressionService
    {
        Task Compress(string source, string destination, Action onCompleted);
        Task GenerateCovers(string source, Action onCompleted);
        Task AddMusic(string videoSource, string musicPath, Action onCompleted);
    }
}
