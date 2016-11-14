using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeeYouOnTheBeach.Web.Models;

namespace SeeYouOnTheBeach.Web.ViewModels
{
    public class FilterViewModel : BaseViewModel
    {
        public List<Feature> BeachFilters { get; set; }
        public string BeachFeatures { get; set; }
    }
}