using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace SeeYouOnTheBeach.Web.OpenData.BikeShare
{
    [XmlRoot(ElementName = "coordinates")]
    public class Coordinates
    {
        [XmlAttribute(AttributeName = "human_address")]
        public string Human_address { get; set; }
        [XmlAttribute(AttributeName = "latitude")]
        public string Latitude { get; set; }
        [XmlAttribute(AttributeName = "longitude")]
        public string Longitude { get; set; }
        [XmlAttribute(AttributeName = "needs_recoding")]
        public string Needs_recoding { get; set; }
    }

    [XmlRoot(ElementName = "row")]
    public class Row
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "featurename")]
        public string Featurename { get; set; }
        [XmlElement(ElementName = "terminalname")]
        public string Terminalname { get; set; }
        [XmlElement(ElementName = "nbbikes")]
        public string Nbbikes { get; set; }
        [XmlElement(ElementName = "nbemptydoc")]
        public string Nbemptydoc { get; set; }
        [XmlElement(ElementName = "uploaddate")]
        public string Uploaddate { get; set; }
        [XmlElement(ElementName = "coordinates")]
        public Coordinates Coordinates { get; set; }
        [XmlAttribute(AttributeName = "_id")]
        public string _id { get; set; }
        [XmlAttribute(AttributeName = "_uuid")]
        public string _uuid { get; set; }
        [XmlAttribute(AttributeName = "_position")]
        public string _position { get; set; }
        [XmlAttribute(AttributeName = "_address")]
        public string _address { get; set; }
    }

    [XmlRoot(ElementName = "rows")]
    public class Rows
    {
        [XmlElement(ElementName = "row")]
        public List<Row> Row { get; set; }
    }

    [XmlRoot(ElementName = "response")]
    public class Response
    {
        [XmlElement(ElementName = "rows")]
        public Rows Rows { get; set; }
    }

}
