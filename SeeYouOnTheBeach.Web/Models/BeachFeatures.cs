using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeeYouOnTheBeach.Web.Models
{
    [Table("BeachFeatures")]
    public partial class BeachFeatures
    {
        [Key]
        public int BeachFeatureId { get; set; }

        public int BeachId { get; set; }

        public int BeachFilterId { get; set; }

        //public virtual Beach Beach { get; set; }

        //public virtual Feature Feature { get; set; }
    }
}
