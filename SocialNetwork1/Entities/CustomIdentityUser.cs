using Microsoft.AspNetCore.Identity;

namespace SocialNetwork1.Entities
{
    public class CustomIdentityUser : IdentityUser
    {
        public string? Image { get; set; }
        public bool IsOnline { get; set; }
        public bool HasRequestPending { get; set; }
        public bool IsFriend { get; set; }
        public DateTime? DisconnectTime { get; set; }
        public string? ConnectTime { get; set; } = "";

        public virtual ICollection<Friend>? Friends { get; set; }

        public string GetLastSeen()
        {
            if (IsOnline)
                return "Online";

            var timeDifference = DateTime.Now - DisconnectTime.Value;

            if (timeDifference.TotalMinutes < 1)
                return "Just now";
            if (timeDifference.TotalMinutes < 60)
                return $"{(int)timeDifference.TotalMinutes} minutes ago";
            if (timeDifference.TotalHours < 24)
                return $"{(int)timeDifference.TotalHours} hours ago";
            if (timeDifference.TotalDays < 30)
                return $"{(int)timeDifference.TotalDays} days ago";

            return "Long time ago";
        }
    } 
}
