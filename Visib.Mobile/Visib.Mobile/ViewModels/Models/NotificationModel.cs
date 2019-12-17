using System;
using Visib.Mobile.Services;

namespace Visib.Mobile.ViewModels.Models
{
    public class NotificationModel
    {
        public string User1Id { get; set; }
        public string User2Id { get; set; }
        public string Text { get; set; }
        public DateTime LastChange { get; set; }

        public string UserPicture1 { get { return ApiService.MediaBaseAddress + $"{User1Id}.png"; } }
        public string UserPicture2 { get { return ApiService.MediaBaseAddress + $"{User2Id}.png"; } }
        public string Since
        {
            get
            {
                var span = DateTime.UtcNow - LastChange;

                var since = string.Empty;

                if (span.Days > 0)
                {
                    since += $"{span.Days}d";
                }
                else
                {
                    if (span.Hours > 0)
                    {
                        since += $"{span.Hours}h";
                    }
                    else
                    {
                        if (span.Minutes > 0)
                        {
                            since += $"{span.Minutes}m";
                        }
                        else
                        {
                            since += $"{span.Seconds}s";
                        }
                    }
                }
                return since;
            }
        }
    }
}
