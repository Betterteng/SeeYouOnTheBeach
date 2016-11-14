using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeeYouOnTheBeach.Web.Models
{
    [Table("Feature")]
    public partial class Feature
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Feature()
        {
            IsChecked = false;
            //BeachFeatures = new HashSet<BeachFeatures>();
        }
        [Key]
        public int BeachFilterId { get; set; }

        [Required]
        [StringLength(255)]
        public string BeachFilterDescription { get; set; }

        [NotMapped]
        public bool IsChecked { get; set; }
        /*
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BeachFeatures> BeachFeatures { get; set; }
        */
    }
}
