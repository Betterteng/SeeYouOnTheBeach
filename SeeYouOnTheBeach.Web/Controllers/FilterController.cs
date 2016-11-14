using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using SeeYouOnTheBeach.Web.Repository;
using SeeYouOnTheBeach.Web.ViewModels;

namespace SeeYouOnTheBeach.Web.Controllers
{
    public class FilterController : Controller
    {
        private readonly DataRepository _dataRepository;

        public FilterController()
        {
            _dataRepository = new DataRepository();
        }

        // GET: Filter
        public ActionResult Index()
        {
            var beaches = _dataRepository.GetBeaches(true).ToList();
            var photos = _dataRepository.GetPhotos();
            var features = JsonConvert.SerializeObject(beaches.ToArray());
            var filters = _dataRepository.GetBeachFilters().ToList();

            var viewModel = new FilterViewModel()
            {
                Beaches = beaches,
                Photos = photos,
                BeachFilters = filters,
                BeachFeatures = features
                    .Replace("'", "\\'")
                    .Replace("\\n", string.Empty)
                    .Replace("\\\"", "\"")
            };
            return View(viewModel);
        }
    }
}