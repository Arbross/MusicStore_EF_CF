using System.Data.Entity.ModelConfiguration;

namespace DAL
{
    public class PlateConfig : EntityTypeConfiguration<Plate>
    {
        public PlateConfig()
        {
            // Set primary key
            HasKey(x => x.Id);

            // Property
            Property(x => x.Name).IsRequired()
                                 .HasMaxLength(100);

            // One to Many
            HasRequired(x => x.Group).WithMany(x => x.Plates)
                                     .HasForeignKey(x => x.GroupId)
                                     .WillCascadeOnDelete(false);

            HasRequired(x => x.Maker).WithMany(x => x.Plates)
                                     .HasForeignKey(x => x.MakerId)
                                     .WillCascadeOnDelete(false);

            HasRequired(x => x.Genre).WithMany(x => x.Plates)
                                     .HasForeignKey(x => x.GenreId)
                                     .WillCascadeOnDelete(false);
        }
    }
}
