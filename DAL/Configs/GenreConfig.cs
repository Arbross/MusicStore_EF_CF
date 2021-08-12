using System.Data.Entity.ModelConfiguration;

namespace DAL
{
    public class GenreConfig : EntityTypeConfiguration<Genre>
    {
        public GenreConfig()
        {
            // Set primary key
            HasKey(x => x.Id);

            // Property
            Property(x => x.Name).IsRequired()
                                 .HasMaxLength(100);
        }
    }
}
