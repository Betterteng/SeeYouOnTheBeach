using System.Collections.Generic;
using SeeYouOnTheBeach.Web.Models;
using SharpKml.Dom;

namespace SeeYouOnTheBeach.Web.ViewModels
{
    public class BeachDetailViewModel : BaseViewModel
    {
        public string Sport { get; set; }
        public string Toilet { get; set; }
        public string Hospital { get; set; }
        public string Weather { get; set; }
        public string BikeShare { get; set; }
        public string Barbecue { get; set; }
        public Beach Beach { get; set; }
    //todo
    }
}