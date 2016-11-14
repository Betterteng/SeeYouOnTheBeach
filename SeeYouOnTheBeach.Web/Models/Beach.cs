namespace SeeYouOnTheBeach.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Beach")]
    public partial class Beach
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Beach()
        {
            //Photos = new HashSet<Photo>();
            BeachFeatures = new HashSet<BeachFeatures>();
        }

        public int BeachId { get; set; }

        public string BeachName { get; set; }

        public string Description { get; set; }
        

        [Column(TypeName = "numeric")]
        public decimal Latitude { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Longitude { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Photo> Photos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BeachFeatures> BeachFeatures { get; set; }
    }
}
