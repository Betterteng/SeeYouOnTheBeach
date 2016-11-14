using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeeYouOnTheBeach.Web.OpenData
{
    public static class OpenDataPath
    {
        public static readonly string Sport = HttpContext.Current.Server.MapPath("~/OpenData/Sport/SportandRec.kml");
        public static readonly string SportEmpty = HttpContext.Current.Server.MapPath("~/OpenData/Sport/SportandRec_empty.kml");
        public static readonly string SportFiltered = HttpContext.Current.Server.MapPath("~/OpenData/Sport/SportandRec_filtered.kml");
        public static readonly string Toilet = HttpContext.Current.Server.MapPath("~/OpenData/Toilet/ToiletmapExport.xml");
        public static readonly string ToiletFiltered = HttpContext.Current.Server.MapPath("~/OpenData/Toilet/ToiletmapExport_filtered.xml");
        public static readonly string Hospital = HttpContext.Current.Server.MapPath("~/OpenData/Hospital/Hospital.kml");
        public static readonly string HospitalEmpty = HttpContext.Current.Server.MapPath("~/OpenData/Hospital/Hospital_empty.kml");
        public static readonly string HospitalEmptyFiltered = HttpContext.Current.Server.MapPath("~/OpenData/Hospital/Hospital_filtered.kml");
        public static readonly string BikeShare = HttpContext.Current.Server.MapPath("~/OpenData/BikeShare/BikeShare.xml");
        public static readonly string BikeShareFiltered = HttpContext.Current.Server.MapPath("~/OpenData/BikeShare/BikeShare_filtered.xml");
        public static readonly string BarbecueBase = HttpContext.Current.Server.MapPath("~/OpenData/Barbecue/Barbecue_json_");

        public static string BarbecueById(int id)
        {
            return BarbecueBase + id + ".json";
        }
    }
}