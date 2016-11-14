using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using SeeYouOnTheBeach.Web.Models;
using SeeYouOnTheBeach.Web.OpenData;
using SeeYouOnTheBeach.Web.OpenData.Barbecue;
using SeeYouOnTheBeach.Web.OpenData.BikeShare;
using SeeYouOnTheBeach.Web.OpenData.Toilet;
using SeeYouOnTheBeach.Web.Utilities;
using SharpKml.Dom;
using SharpKml.Engine;
using Feature = SeeYouOnTheBeach.Web.Models.Feature;

namespace SeeYouOnTheBeach.Web.Repository
{

    public class DataRepository
    {
        private readonly SimpleHttpCache _cache;
        private readonly PhotoDbContext _dbContext;
        private readonly string _weatherApi;

        public DataRepository()
        {
            _weatherApi = "225d7607c20f8d2b";
            _dbContext = new PhotoDbContext();
            _cache = new SimpleHttpCache();
        }


        public IEnumerable<Placemark> GetSportData()
        {
            const string cacheKey = "SportData";
            return _cache.Get(cacheKey, () =>
            {
                var stream = File.OpenRead(OpenDataPath.SportFiltered);
                var file = KmlFile.Load(stream);
                stream.Close();
                var document = file.Root as Document;
                var result = document.Flatten().OfType<Placemark>().ToList();
                _cache.Set(cacheKey, result);
                return result;
            });
        }

        public async Task<string> GetWeatherDataByBeachId(int id)
        {
            //for production
            // string cacheKey = $"WeatherData_{DateTime.Today}_Beach_{id}";
            //for test
            string cacheKey = $"WeatherData_{DateTime.Today}_Beach_{id}_Hour_{DateTime.Now.Hour}";
            return await _cache.Get(cacheKey, async () =>
            {
                using (var client = new HttpClient())
                {
                    var beach = GetBeaches().FirstOrDefault(b => b.BeachId == id);
                    if (beach == null)
                    {
                        return null;
                    }
                    var lat = beach.Latitude;
                    var lng = beach.Longitude;
                    var contiditon =
                        $"http://api.wunderground.com/api/{_weatherApi}/hourly/conditions/forecast/q/{lat},{lng}.json";

                    client.BaseAddress = new Uri(contiditon);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.GetAsync("");
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        _cache.Set(cacheKey, json);
                        return json;
                    }
                    return string.Empty;
                }
            });
        }

        public IEnumerable<Placemark> GetHospitalDataById(int id)
        {
            string cacheKey = $"HospitalData_{id}";
            return _cache.Get(cacheKey, () =>
            {
                var stream = File.OpenRead(OpenDataPath.HospitalEmptyFiltered);
                var file = KmlFile.Load(stream);
                stream.Close();
                var document = file.Root as Document;
                var result = document.Flatten().OfType<Placemark>()
                    .Where(p => p.TargetId.Split(',').Contains(id.ToString())).ToList();
                _cache.Set(cacheKey, result);
                return result;
            });
        }

        public IEnumerable<ToiletDetails> GetToiletAndDrinkingData()
        {

            string cacheKey = $"Toilet";
            return _cache.Get(cacheKey, () =>
            {
                using (XmlReader reader = XmlReader.Create(OpenDataPath.ToiletFiltered))
                {
                    var serializer = new XmlSerializer(typeof(ToiletMapExport));
                    var file = (ToiletMapExport)serializer.Deserialize(reader);
                    var result = file.ToiletDetails;
                    _cache.Set(cacheKey, result);
                    return result;
                }
            });
        }

        public IEnumerable<Row> GetBikeShareDataById(int id)
        {

            string cacheKey = $"BikeShare_{id}";
            return _cache.Get(cacheKey, () =>
            {
                using (XmlReader reader = XmlReader.Create(OpenDataPath.BikeShareFiltered))
                {
                    var serializer = new XmlSerializer(typeof(Response));
                    var file = (Response)serializer.Deserialize(reader);
                    var result = file.Rows.Row.Where(p => p.Id.Split(',').Contains(id.ToString())).ToList();
                    _cache.Set(cacheKey, result);
                    return result;
                }
            });
        }

        public IEnumerable<BbqList> GetBarbecueDataById(int id)
        {
            string cacheKey = $"Barbecue_{id}";
            return _cache.Get(cacheKey, () =>
            {
                using (var file = File.OpenRead(OpenDataPath.BarbecueById(id)))
                {
                    var serializer = new JsonSerializer();
                    var rootObject = serializer.Deserialize<Barbecue>(new JsonTextReader(new StreamReader(file)));
                    var result = rootObject.bbqList;
                    _cache.Set(cacheKey, result);
                    return result;
                }
            });
        }

        public IEnumerable<Feature> GetBeachFilters()
        {
            string cacheKey = $"BeachFilter";
            return _cache.Get(cacheKey, () =>
            {
                var result = _dbContext.BeachFilters.ToList();
                _cache.Set(cacheKey, result);
                    return result;
            });
        }

        public IEnumerable<BeachFeatures> GetBeachFeatures()
        {
            string cacheKey = $"BeachFeature";
            return _cache.Get(cacheKey, () =>
            {
                var result = _dbContext.BeachFeatures.ToList();
                _cache.Set(cacheKey, result);
                return result;
            });
        }

        public IEnumerable<Placemark> GetSportDataById(int id)
        {
            string cacheKey = $"SportData_{id}";
            return _cache.Get(cacheKey, () =>
            {

                var result = GetSportData()
                    .Where(p => p.TargetId == id.ToString())
                    .ToList();
                _cache.Set(cacheKey, result);
                return result;
            });
        }

        public IEnumerable<Photo> GetPhotosById(int id)
        {
            string cacheKey = $"Photos_{id}";
            return _cache.Get(cacheKey, () =>
            {
                var result = _dbContext.Photos.Where(p => p.BeachId == id).Include(p => p.Beach).ToList();
                _cache.Set(cacheKey, result);
                return result;
            });
        }

        public IEnumerable<Photo> GetPhotos()
        {
            string cacheKey = $"Photos";
            return _cache.Get(cacheKey, () =>
            {
                var result = _dbContext.Photos.Include(p => p.Beach).ToList();
                _cache.Set(cacheKey, result);
                return result;
            });
        }

        public IEnumerable<Beach> GetBeaches(bool includeFeatures = false)
        {
            string cacheKey = $"Beaches_{includeFeatures}";
            return _cache.Get(cacheKey, () =>
            {
                if (includeFeatures)
                {
                    var result = _dbContext.Beaches.Include(b => b.BeachFeatures);
                    _cache.Set(cacheKey, result);
                    return result;
                }
                else
                {
                    var result = _dbContext.Beaches;
                    _cache.Set(cacheKey, result);
                    return result;
                }
            });
        }
    }
}