using System.Threading.Tasks;

namespace Visib.Api.Services
{
    public interface ISmsService
    {
        Task Send(string number, string message);
    }
}