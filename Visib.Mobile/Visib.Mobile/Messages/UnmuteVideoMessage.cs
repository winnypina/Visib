using System;
using MvvmCross.Plugin.Messenger;

namespace Visib.Mobile.Messages
{
    public class UnmuteVideoMessage : MvxMessage
    {
        public UnmuteVideoMessage(object sender) : base(sender)
        {
        }

        public string Path { get; set; }
    }
}
