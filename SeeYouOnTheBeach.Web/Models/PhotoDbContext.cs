namespace SeeYouOnTheBeach.Web.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class PhotoDbContext : DbContext
    {
        public PhotoDbContext()
            : base("name=PhotoDbContext")
        {
        }

        public virtual DbSet<Beach> Beaches { get; set; }
        public virtual DbSet<Photo> Photos { get; set; }
        public virtual DbSet<BeachFeatures> BeachFeatures { get; set; }
        public virtual DbSet<Feature> BeachFilters { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Beach>()
                .Property(e => e.Latitude)
                .HasPrecision(18, 14);

            modelBuilder.Entity<Beach>()
                .Property(e => e.Longitude)
                .HasPrecision(18, 14);

            /*modelBuilder.Entity<Beach>()
                .HasMany(e => e.Photos)
                .WithRequired(e => e.Beach)
                .WillCascadeOnDelete(false);*/

            modelBuilder.Entity<Beach>()
                .HasMany(e => e.BeachFeatures);
            /*
            modelBuilder.Entity<Feature>()
                .HasMany(e => e.BeachFeatures)
                .WithRequired(e => e.Feature)
                .WillCascadeOnDelete(false);
                */
        }
    }
}
