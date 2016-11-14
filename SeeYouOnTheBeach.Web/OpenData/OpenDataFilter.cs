using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Ajax.Utilities;
using SeeYouOnTheBeach.Web.Models;
using SeeYouOnTheBeach.Web.OpenData.Toilet;
using SeeYouOnTheBeach.Web.OpenData.BikeShare;
using SeeYouOnTheBeach.Web.Repository;
using SharpKml.Dom;
using SharpKml.Engine;

namespace SeeYouOnTheBeach.Web.OpenData
{
    public static class OpenDataFilter
    {
        //2km radius : +-0.018,+-0.0228
        //5km radius : +-0.045,+-0.057
        public static readonly double Lat = 0.045;
        public static readonly double Lng = 0.057;

        public static void ToiletFilter()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ToiletMapExport));
            ToiletMapExport file;
            using (XmlReader reader = XmlReader.Create(OpenDataPath.Toilet))
            {
                var repo = new PhotoDbContext();
                var beaches = repo.Beaches.ToList();
                repo.Dispose();
                file = (ToiletMapExport)serializer.Deserialize(reader);
                file.ToiletDetails = file.ToiletDetails.Where(t => ToiletProceedToiletDetail(ref t, beaches)).ToList();
            }
            using (XmlWriter writer = XmlWriter.Create(OpenDataPath.ToiletFiltered))
            {
                serializer.Serialize(writer, file);
                writer.Flush();
            }
        }

        public static async void BarbecueFilter()
        {
            var repo = new PhotoDbContext();
            var beaches = repo.Beaches.ToList();
            repo.Dispose();
            foreach (var beach in beaches)
            {
                using (var client = new HttpClient())
                {
                    var lat = beach.Latitude;
                    var lng = beach.Longitude;
                    var apiCallUrl =
                        $"http://www.meatinapark.com.au/multiplebbqfinder?long={lng}&lat={lat}&distance=2&userDetails=true&bbqCount=300";

                    client.BaseAddress = new Uri(apiCallUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.GetAsync("");
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStreamAsync();
                        using (var file = File.Create(OpenDataPath.BarbecueById(beach.BeachId)))
                        {
                            json.CopyTo(file);
                            file.Flush();
                        }
                    }
                }
            }            
        }

        public static void BikeShareFilter()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Response));
            Response file;
            using (XmlReader reader = XmlReader.Create(OpenDataPath.BikeShare))
            {
                var repo = new PhotoDbContext();
                var beaches = repo.Beaches.ToList();
                repo.Dispose();
                file = (Response)serializer.Deserialize(reader);
                file.Rows.Row = file.Rows.Row.Where(r => BikeShareProceedRow(ref r, beaches)).ToList();
            }
            using (XmlWriter writer = XmlWriter.Create(OpenDataPath.BikeShareFiltered))
            {
                serializer.Serialize(writer, file);
                writer.Flush();
            }
        }

        public static void KmlFilter(string originalPath, string emptyFilePath, string outputPath, bool multipleAssgnment = false)
        {
            var repo = new PhotoDbContext();
            var beaches = repo.Beaches.ToList();
            var stream = File.OpenRead(originalPath);
            var file = KmlFile.Load(stream);
            stream.Close();
            var stream2 = File.OpenRead(emptyFilePath);
            var file2 = KmlFile.Load(stream2);
            stream2.Close();
            var kml = file.Root as Kml;
            var kml2 = file2.Root as Kml;

            if (kml != null)
            {
                var list = kml.Feature.Flatten().OfType<Folder>().FirstOrDefault();
                Folder folder = new Folder();
                list.Flatten()?.OfType<Placemark>().ForEach(p =>
                {
                    var place = p.Clone();
                    ProceedPlacemark(ref place, beaches, multipleAssgnment);
                    if (place.TargetId != "-1")
                        folder.AddFeature(place);
                });

                if (kml2 != null)
                {
                    Document document = kml2.Feature.Flatten().OfType<Document>().FirstOrDefault();
                    var clone = document.Clone();
                    clone.AddFeature(folder);

                    var file3 = KmlFile.Create(clone, true);
                    var stream3 = File.OpenWrite(outputPath);
                    file3.Save(stream3);
                    stream3.Flush();
                    stream3.Close();
                }
            }
        }

        public static bool ToiletProceedToiletDetail(ref ToiletDetails details, List<Beach> beaches)
        {
            try
            {
                var lat = double.Parse(details.Latitude);
                var lng = double.Parse(details.Longitude);
                var beachid = AllocateBeach(lat, lng, beaches,false);
                if (beachid == "-1")
                {
                    return false;
                }
                details.State = beachid;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool BikeShareProceedRow(ref Row row, List<Beach> beaches)
        {
            try
            {
                var lat = double.Parse(row.Coordinates.Latitude);
                var lng = double.Parse(row.Coordinates.Longitude);
                var beachid = AllocateBeach(lat, lng, beaches, true);
                if (beachid == "-1")
                {
                    return false;
                }
                row.Id = beachid;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void ProceedPlacemark(ref Placemark placemark, List<Beach> beaches, bool multipleAssignment)
        {
            var point = placemark.Geometry as Point;
            if (point == null)
            {
                placemark.TargetId = "-1";
                return;
            }
            placemark.TargetId = AllocateBeach(point.Coordinate.Latitude, point.Coordinate.Longitude, beaches, multipleAssignment);
        }

        public static string AllocateBeach(double latitude, double longitude, List<Beach> beaches, bool multipleAssignment)
        {
            // 1:m assignment;
            string beachId = "-1";
            if (!multipleAssignment)
            {
                var maxlat = 1.0;
                var maxlng = 1.0;
                beaches.ForEach(b =>
                {
                    var abslat = Math.Abs((double)b.Latitude - latitude);
                    var abslng = Math.Abs((double)b.Longitude - longitude);
                    if (abslat <= Lat
                    && abslng <= Lng
                    && abslat <= maxlat
                    && abslng <= maxlng)
                    {
                        beachId = b.BeachId.ToString();
                        maxlat = abslat;
                        maxlng = abslng;
                    }
                });
                return beachId;
            }
            else // 1:m assignment;
            {
                var firstAssignment = true;
                beaches.ForEach(b =>
                {
                    var abslat = Math.Abs((double)b.Latitude - latitude);
                    var abslng = Math.Abs((double)b.Longitude - longitude);
                    if (abslat <= Lat && abslng <= Lng)
                    {
                        if (firstAssignment)
                        {
                            beachId = b.BeachId.ToString();
                            firstAssignment = false;
                        }
                        else
                        {
                            beachId += $",{b.BeachId.ToString()}";
                        }
                    }
                });
                return beachId;
            }
        }
    }
}