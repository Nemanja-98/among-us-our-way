using System.Collections.Generic;

namespace AmongUs_OurWay
{
    public class LiveUsersMenager
    {
        public Dictionary<string, string> LiveUsers { get; set; }

        public Dictionary<string, string> LiveFriends { get; set; }

        public Dictionary<string, string> InGameUsers { get; set; }

        public LiveUsersMenager()
        {
            LiveUsers = new Dictionary<string, string>();

            LiveFriends = new Dictionary<string, string>();

            InGameUsers = new Dictionary<string, string>();
        }
    }
}