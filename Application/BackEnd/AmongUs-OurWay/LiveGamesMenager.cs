using System.Collections.Generic;

namespace AmongUs_OurWay
{
    public class LiveGamesMenager
    {
        public List<string> LiveGames { get; set; }
        
        public LiveGamesMenager()
        {
            LiveGames = new List<string>();
        }
    }
}