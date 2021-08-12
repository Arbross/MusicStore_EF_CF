using System.Data.Entity.ModelConfiguration;

namespace DAL
{
    public class MakerConfig : EntityTypeConfiguration<Maker>
    {
        public MakerConfig()
        {
            // Set primary key
            HasKey(x => x.Id);

            // Property
            Property(x => x.Name).IsRequired()
                                 .HasMaxLength(100);
        }
    }
}
