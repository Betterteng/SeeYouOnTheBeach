using SeeYouOnTheBeach.Web.ViewModels;

namespace SeeYouOnTheBeach.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Photo")]
    public partial class Photo : BaseViewModel
    {
        public int PhotoId { get; set; }

        public int BeachId { get; set; }

        public string Description { get; set; }

        public byte[] Content { get; set; }

        public virtual Beach Beach { get; set; }
    }
}
