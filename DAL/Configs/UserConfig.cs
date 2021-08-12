using System.Data.Entity.ModelConfiguration;

namespace DAL
{
    public class UserConfig : EntityTypeConfiguration<User>
    {
        public UserConfig()
        {
            // Set primary key
            HasKey(x => x.Id);

            // Property
            Property(x => x.Login).IsRequired()
                                  .HasMaxLength(100);

            Property(x => x.Password).IsRequired()
                                     .HasMaxLength(100);

            Property(x => x.IsAdmin).IsRequired();
        }
    }
}
