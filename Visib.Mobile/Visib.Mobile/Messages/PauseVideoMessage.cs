using System;
using MvvmCross.Plugin.Messenger;

namespace Visib.Mobile.Messages
{
    public class PauseVideoMessage : MvxMessage
    {
        public PauseVideoMessage(object sender) : base(sender)
        {
        }

        public string Path { get; set; }
    }
}
