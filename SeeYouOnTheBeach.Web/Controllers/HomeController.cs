using SeeYouOnTheBeach.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SeeYouOnTheBeach.Web.OpenData;
using SeeYouOnTheBeach.Web.Repository;
using SeeYouOnTheBeach.Web.ViewModels;

namespace SeeYouOnTheBeach.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly DataRepository _dataRepository;

        public HomeController()
        {
            _dataRepository = new DataRepository();
        }

        public ActionResult Index(int beachid = 1)
        {
            ViewBag.beachid = beachid;
            var viewModel = new HomeViewModel()
            {
                Beaches = _dataRepository.GetBeaches(),
                Photos = _dataRepository.GetPhotos() // for test
            };
            return View(viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}