using System.Collections.Generic;

namespace AmongUs_OurWay.Models
{
    public class LiveUsersMenager
    {
        public Dictionary<string, string> LiveUsers { get; set; }

        public LiveUsersMenager()
        {
            LiveUsers = new Dictionary<string, string>();
        }
    }
}