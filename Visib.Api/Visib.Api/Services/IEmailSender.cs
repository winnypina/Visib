using System.Threading.Tasks;

namespace Visib.Api.Services
{
    public interface IEmailSender
    {
        Task Send(string toAddress, string subject, string message);
    }
}