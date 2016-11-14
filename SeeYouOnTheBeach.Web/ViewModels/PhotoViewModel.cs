using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SeeYouOnTheBeach.Web.Models;

namespace SeeYouOnTheBeach.Web.ViewModels
{
    public class PhotoViewModel : BaseViewModel
    {
        public IEnumerable<Photo> Photo { get; set; }
    }
}