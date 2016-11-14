using System.Collections.Generic;
using System.Xml.Serialization;

namespace SeeYouOnTheBeach.Web.OpenData.Toilet
{
    [XmlRoot(ElementName = "GeneralDetails", Namespace = "http://toiletmap.gov.au/")]
    public class GeneralDetails
    {
        [XmlElement(ElementName = "Male", Namespace = "http://toiletmap.gov.au/")]
        public string Male { get; set; }
        [XmlElement(ElementName = "Female", Namespace = "http://toiletmap.gov.au/")]
        public string Female { get; set; }
        [XmlElement(ElementName = "FacilityType", Namespace = "http://toiletmap.gov.au/")]
        public string FacilityType { get; set; }
        [XmlElement(ElementName = "AccessLimited", Namespace = "http://toiletmap.gov.au/")]
        public string AccessLimited { get; set; }
        [XmlElement(ElementName = "PaymentRequired", Namespace = "http://toiletmap.gov.au/")]
        public string PaymentRequired { get; set; }
        [XmlElement(ElementName = "KeyRequired", Namespace = "http://toiletmap.gov.au/")]
        public string KeyRequired { get; set; }
        [XmlElement(ElementName = "Parking", Namespace = "http://toiletmap.gov.au/")]
        public string Parking { get; set; }
    }

    [XmlRoot(ElementName = "AccessibilityDetails", Namespace = "http://toiletmap.gov.au/")]
    public class AccessibilityDetails
    {
        [XmlElement(ElementName = "AccessibleMale", Namespace = "http://toiletmap.gov.au/")]
        public bool AccessibleMale { get; set; }
        [XmlElement(ElementName = "AccessibleFemale", Namespace = "http://toiletmap.gov.au/")]
        public bool AccessibleFemale { get; set; }
        [XmlElement(ElementName = "AccessibleUnisex", Namespace = "http://toiletmap.gov.au/")]
        public bool AccessibleUnisex { get; set; }
        [XmlElement(ElementName = "MLAK", Namespace = "http://toiletmap.gov.au/")]
        public bool MLAK { get; set; }
        [XmlElement(ElementName = "ParkingAccessible", Namespace = "http://toiletmap.gov.au/")]
        public bool ParkingAccessible { get; set; }
    }

    [XmlRoot(ElementName = "OpeningHours", Namespace = "http://toiletmap.gov.au/")]
    public class OpeningHours
    {
        [XmlElement(ElementName = "IsOpen", Namespace = "http://toiletmap.gov.au/")]
        public string IsOpen { get; set; }
    }

    [XmlRoot(ElementName = "Features", Namespace = "http://toiletmap.gov.au/")]
    public class Features
    {
        [XmlElement(ElementName = "BabyChange", Namespace = "http://toiletmap.gov.au/")]
        public bool BabyChange { get; set; }
        [XmlElement(ElementName = "Showers", Namespace = "http://toiletmap.gov.au/")]
        public bool Showers { get; set; }
        [XmlElement(ElementName = "DrinkingWater", Namespace = "http://toiletmap.gov.au/")]
        public bool DrinkingWater { get; set; }
        [XmlElement(ElementName = "SharpsDisposal", Namespace = "http://toiletmap.gov.au/")]
        public bool SharpsDisposal { get; set; }
        [XmlElement(ElementName = "SanitaryDisposal", Namespace = "http://toiletmap.gov.au/")]
        public bool SanitaryDisposal { get; set; }
    }

    [XmlRoot(ElementName = "Icon", Namespace = "http://toiletmap.gov.au/")]
    public class Icon
    {
        [XmlElement(ElementName = "IconURL", Namespace = "http://toiletmap.gov.au/")]
        public string IconURL { get; set; }
        [XmlAttribute(AttributeName = "IconAltText")]
        public string IconAltText { get; set; }
    }

    [XmlRoot(ElementName = "ToiletDetails", Namespace = "http://toiletmap.gov.au/")]
    public class ToiletDetails
    {
        [XmlElement(ElementName = "Name", Namespace = "http://toiletmap.gov.au/")]
        public string Name { get; set; }
        [XmlElement(ElementName = "Address1", Namespace = "http://toiletmap.gov.au/")]
        public string Address1 { get; set; }
        [XmlElement(ElementName = "Town", Namespace = "http://toiletmap.gov.au/")]
        public string Town { get; set; }
        [XmlElement(ElementName = "State", Namespace = "http://toiletmap.gov.au/")]
        public string State { get; set; }
        [XmlElement(ElementName = "Postcode", Namespace = "http://toiletmap.gov.au/")]
        public string Postcode { get; set; }
        [XmlElement(ElementName = "GeneralDetails", Namespace = "http://toiletmap.gov.au/")]
        public GeneralDetails GeneralDetails { get; set; }
        [XmlElement(ElementName = "AccessibilityDetails", Namespace = "http://toiletmap.gov.au/")]
        public AccessibilityDetails AccessibilityDetails { get; set; }
        [XmlElement(ElementName = "OpeningHours", Namespace = "http://toiletmap.gov.au/")]
        public OpeningHours OpeningHours { get; set; }
        [XmlElement(ElementName = "Features", Namespace = "http://toiletmap.gov.au/")]
        public Features Features { get; set; }
        [XmlElement(ElementName = "Icon", Namespace = "http://toiletmap.gov.au/")]
        public Icon Icon { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
        [XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string SchemaLocation { get; set; }
        [XmlAttribute(AttributeName = "Status")]
        public string Status { get; set; }
        [XmlAttribute(AttributeName = "Latitude")]
        public string Latitude { get; set; }
        [XmlAttribute(AttributeName = "Longitude")]
        public string Longitude { get; set; }
        [XmlAttribute(AttributeName = "ToiletURL")]
        public string ToiletURL { get; set; }
        [XmlAttribute(AttributeName = "LastUpdateDate")]
        public string LastUpdateDate { get; set; }
    }

    [XmlRoot(ElementName = "ToiletMapExport", Namespace = "http://toiletmap.gov.au/")]
    public class ToiletMapExport
    {
        [XmlElement(ElementName = "ToiletDetails", Namespace = "http://toiletmap.gov.au/")]
        public List<ToiletDetails> ToiletDetails { get; set; }


        [XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string SchemaLocation { get; set; }
    }

}