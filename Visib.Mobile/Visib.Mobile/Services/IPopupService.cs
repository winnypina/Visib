using System;
using System.Threading.Tasks;

namespace Visib.Mobile.Services
{
    public interface IPopupService
    {

        Task ShowWithOptions(string message, Action<string> OkAction, string okText, string cancelText);
        Task Dismiss();
    }
}