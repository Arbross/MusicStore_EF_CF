using System.Data.Entity.ModelConfiguration;

namespace DAL
{
    public class SoldPlateConfig : EntityTypeConfiguration<SoldPlate>
    {
        public SoldPlateConfig()
        {
            // Set primary key
            HasKey(x => x.Id);

            // Property
            Property(x => x.Name).IsRequired()
                                 .HasMaxLength(100);

            // One to Many
            //HasRequired(x => x.Genres).WithMany(x => x.SoldPlates)
            //                          .HasForeignKey(x => x.GroupId)
            //                          .WillCascadeOnDelete(false);

            //HasRequired(x => x.Maker).WithMany(x => x.SoldPlates)
            //                         .HasForeignKey(x => x.MakerId)
            //                         .WillCascadeOnDelete(false);

            //// Many to Many
            //HasMany(x => x.Genres).WithMany(x => x.SoldPlates);
        }
    }
}
