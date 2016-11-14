using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using SeeYouOnTheBeach.Web.Repository;
using SeeYouOnTheBeach.Web.ViewModels;
using SharpKml.Dom;
using Newtonsoft.Json;
using SeeYouOnTheBeach.Web.OpenData;
using SeeYouOnTheBeach.Web.Utilities;

namespace SeeYouOnTheBeach.Web.Controllers
{
    public class BeachDetailController : Controller
    {
        private readonly DataRepository _dataRepository;

        public BeachDetailController()
        {
            _dataRepository = new DataRepository();
        }

        public async Task<ActionResult> Index(int? beachid = 1)
        {
            if (beachid == null)
            {
                beachid = 1;
            }
            var barbecues = _dataRepository.GetBarbecueDataById(beachid.Value).Select(MapLocation.MapBarbecueFromBbqList).ToArray();
            var sport = _dataRepository.GetSportDataById(beachid.Value).Select(MapLocation.MapSportFromPlacemark).ToArray();
            var hospital = _dataRepository.GetHospitalDataById(beachid.Value).Select(MapLocation.MapHpspitalFromPlacemark).ToArray();
            var toilet = _dataRepository.GetToiletAndDrinkingData().Where(d => d.State == beachid.ToString()).Select(MapLocation.MapToiletFromToiletDetails);
            var bikeShare = _dataRepository.GetBikeShareDataById(beachid.Value).Select(MapLocation.MapBikeShareFromRow);
            var weather = await _dataRepository.GetWeatherDataByBeachId(beachid.Value);
                 
            var viewModel = new BeachDetailViewModel
            {
                Beaches = _dataRepository.GetBeaches(),
                Photos = _dataRepository.GetPhotosById(beachid.Value),              
                Sport = JsonConvert.SerializeObject(sport).ProceedQuotationAndBreaks(),
                Beach = _dataRepository.GetBeaches().FirstOrDefault(b => b.BeachId == beachid),
                Barbecue = JsonConvert.SerializeObject(barbecues).ProceedQuotationAndBreaks(),
                Hospital = JsonConvert.SerializeObject(hospital).ProceedQuotationAndBreaks(),
                BikeShare = JsonConvert.SerializeObject(bikeShare).ProceedQuotationAndBreaks(),
                Weather = weather.Replace("http","https").ProceedQuotationAndBreaks(),
                Toilet = JsonConvert.SerializeObject(toilet).ProceedQuotationAndBreaks()
            };
            
            return View(viewModel);
        }
    }
}