using System.Collections.Generic;

namespace SeeYouOnTheBeach.Web.OpenData.Barbecue
{
        public class BbqList
        {
            public string address { get; set; }
            public string name { get; set; }
            public string fbUid { get; set; }
            public double longitude { get; set; }
            public object bbqId { get; set; }
            public double latitude { get; set; }
            public string type { get; set; }
            public string distanceToBbq { get; set; }
            public string fbName { get; set; }
        }

        public class Barbecue
        {
            public int matchCount { get; set; }
            public List<BbqList> bbqList { get; set; }
        }
    
}