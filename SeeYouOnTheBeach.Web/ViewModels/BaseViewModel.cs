using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SeeYouOnTheBeach.Web.Models;

namespace SeeYouOnTheBeach.Web.ViewModels
{
    public class BaseViewModel
    {
        public IEnumerable<Beach> Beaches { get; set; }
        public IEnumerable<Photo> Photos { get; set; }
    }
}