using MvvmCross.Plugin.Messenger;

namespace Visib.Mobile.Messages
{
    public class PlayVideoMessage : MvxMessage
    {
        public PlayVideoMessage(object sender) : base(sender)
        {
        }

        public string Path { get; set; }
    }
}
