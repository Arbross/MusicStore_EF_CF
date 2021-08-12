using System.Data.Entity.ModelConfiguration;

namespace DAL
{
    public class GroupConfig : EntityTypeConfiguration<Group>
    {
        public GroupConfig()
        {
            // Set primary key
            HasKey(x => x.Id);

            // Property
            Property(x => x.Name).IsRequired()
                                 .HasMaxLength(100);

            
        }
    }
}