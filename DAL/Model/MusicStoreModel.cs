using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DAL
{
    public partial class MusicStoreModel : DbContext
    {
        public MusicStoreModel()
            : base("name=MSModel")
        {
            Database.SetInitializer(new MusicStoreDbInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Add Configuration Classes
            modelBuilder.Configurations.Add(new GroupConfig());
            modelBuilder.Configurations.Add(new MakerConfig());
            modelBuilder.Configurations.Add(new GenreConfig());
            modelBuilder.Configurations.Add(new PlateConfig());
            modelBuilder.Configurations.Add(new SoldPlateConfig());
            modelBuilder.Configurations.Add(new UserConfig());
        }

        public virtual DbSet<Plate> Plates { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Maker> Makers { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<SoldPlate> SoldPlates { get; set; }
        public virtual DbSet<Basket> Basket { get; set; }
        public virtual DbSet<SavePlate> SavePlates { get; set; }
    }
}
