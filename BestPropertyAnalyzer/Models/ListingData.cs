using System;
using System.Xml.Serialization;

namespace BestPropertyAnalyzer.Models
{
    [XmlRoot("parcel_snapshot")]
    public class ListingData
    {
        public string id { get; set; }

        public string address { get; set; }

        public string apn { get; set; }

        public string county { get; set; }

        public float latitude { get; set; }

        public float longitude { get; set; }

        public string city { get; set; }

        public string owner { get; set; }

        public string Details { get; set; }


        //jsonData = "{\"address\":\"863 MASSACHUSETTS AVE APT 15, CAMBRIDGE, MA 02139\",\"apn\":\"CAMB M:00117 L: 0005700015\",\"city\":\"CAMBRIDGE\",\"county\":\"MIDDLESEX\",\"id\":\"65891272\",\"latitude\":42.367708,\"longitude\":-71.107172,\"matching_reason\":0}";

        public ListingData()
        {
        }
    }
}
