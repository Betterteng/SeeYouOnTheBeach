using System.Linq;
using SeeYouOnTheBeach.Web.OpenData.Barbecue;
using SeeYouOnTheBeach.Web.OpenData.BikeShare;
using SeeYouOnTheBeach.Web.OpenData.Toilet;
using SharpKml.Dom;
using SharpKml.Engine;

namespace SeeYouOnTheBeach.Web.ViewModels
{
    public class MapLocation
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public string Description3 { get; set; }

        public static MapLocation MapBikeShareFromRow(Row row)
        {
            var location = new MapLocation()
            {
                Lat = double.Parse(row.Coordinates.Latitude),
                Lng = double.Parse(row.Coordinates.Longitude),
                Description1 = row.Featurename,
                Description2 = "Total number of bike spot: " + (int.Parse(row.Nbbikes) + int.Parse(row.Nbemptydoc)),
                Description3 = string.Empty
            };
            return location;
        }

        public static MapLocation MapBarbecueFromBbqList(BbqList item)
        {
            var location = new MapLocation()
            {
                Lat = item.latitude,
                Lng = item.longitude,
                Description1 = "Location: " + item.name + " " + item.address,
                Description2 = "Distance to Beach: " + item.distanceToBbq + "km",
                Description3 = string.Empty
            };
            return location;
        }

        public static MapLocation MapToiletFromToiletDetails(ToiletDetails place)
        {
            var location = new MapLocation
            {
                Lat = double.Parse(place.Latitude),
                Lng = double.Parse(place.Longitude),
                Description1 = place.Name,
                Description2 = "Features: " +
                               (place.Features.DrinkingWater ? "DrinkingWater; " : string.Empty) +
                               (place.Features.BabyChange ? "BabyChange; " : string.Empty) +
                               (place.Features.SanitaryDisposal ? "SanitaryDisposal; " : string.Empty) +
                               (place.Features.SharpsDisposal ? "SharpsDisposal; " : string.Empty) +
                               (place.Features.Showers ? "Showers; " : string.Empty),
                Description3 = "Accessibility: " +
                               (place.AccessibilityDetails.AccessibleFemale ? "Female; " : string.Empty) +
                               (place.AccessibilityDetails.AccessibleMale ? "Male; " : string.Empty) +
                               (place.AccessibilityDetails.AccessibleUnisex ? "Uni-Sex; " : string.Empty) +
                               (place.AccessibilityDetails.ParkingAccessible ? "ParkingAccessible; " : string.Empty)
            };

            return location;
        }

        public static MapLocation MapHpspitalFromPlacemark(Placemark place)
        {
            var location = new MapLocation();
            location.Description1 = place.Name ?? string.Empty;
            location.Description2 = string.Empty;
           //     .Replace("<![CDATA[",string.Empty)
           //     .Replace("]]>",string.Empty);
            location.Description3 = string.Empty;
            location.Lat = ((Point)(place.Geometry)).Coordinate.Latitude;
            location.Lng = ((Point)(place.Geometry)).Coordinate.Longitude;
            return location;
            /*
                <name>Calvary Health Care Bethlehem Ltd</name>
                <description><![CDATA[**HtmlCode**]]></description>
            */
        }

        public static MapLocation MapSportFromPlacemark(Placemark place)
        {
            var location = new MapLocation();
            var simpleDataList = place.ExtendedData.Flatten().OfType<SimpleData>().ToList();
            location.Description1 = simpleDataList.FirstOrDefault(s => s.Name == "FacilityName")?.Text;
            location.Description2 = 
                $"{simpleDataList.FirstOrDefault(s => s.Name == "StreetNo")?.Text ?? string.Empty} " +
                $"{simpleDataList.FirstOrDefault(s => s.Name == "StreetName")?.Text ?? string.Empty} " +
                $"{simpleDataList.FirstOrDefault(s => s.Name == "StreetType")?.Text ?? string.Empty} " +
                $"{simpleDataList.FirstOrDefault(s => s.Name == "SuburbTown")?.Text ?? string.Empty} " +
                $"{simpleDataList.FirstOrDefault(s => s.Name == "Postcode")?.Text ?? string.Empty}";
            location.Description3 = "Additional Info: " +
                $"{simpleDataList.FirstOrDefault(s => s.Name == "SportsPlayed")?.Text ?? string.Empty}";
            location.Lat = ((Point)(place.Geometry)).Coordinate.Latitude;
            location.Lng = ((Point)(place.Geometry)).Coordinate.Longitude;
            return location;
            /*
              <SimpleData name="OBJECTID">3495</SimpleData>
              <SimpleData name="Facility_ID">GLENEI11249</SimpleData>
              <SimpleData name="FacilityName">Sage Institute of Fitness</SimpleData>
              <SimpleData name="StreetNo">233</SimpleData>
              <SimpleData name="StreetName">Glenhuntley</SimpleData>
              <SimpleData name="StreetType">Road</SimpleData>
              <SimpleData name="SuburbTown">ELSTERNWICK</SimpleData>
              <SimpleData name="Postcode">3162</SimpleData>
              <SimpleData name="Latitude">-37.883879</SimpleData>
              <SimpleData name="Longitude">144.999084</SimpleData>
              <SimpleData name="FaciltySportPlayedID">17972</SimpleData>
              <SimpleData name="SportsPlayed">Fitness / Gymnasium Workouts</SimpleData>
              <SimpleData name="NumberFieldCourts">1</SimpleData>
              <SimpleData name="FieldSurfaceType" />
              <SimpleData name="FacilityAge" />
              <SimpleData name="FacilityCondition" />
              <SimpleData name="FacilityUpgradeAge" />
              <SimpleData name="Changerooms" />
              <SimpleData name="LGA">GLEN EIRA</SimpleData>
            */
        }
    }
}