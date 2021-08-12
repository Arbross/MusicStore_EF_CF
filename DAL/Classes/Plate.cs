using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    public class Plate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountOfTrecks { get; set; }
        public int PublishYear { get; set; }
        public double LocalPrice { get; set; }
        public double Price { get; set; }
        public double? BeforeDiscountPrice { get; set; }
        public int SoldCopies { get; set; }
        public bool IsAvailable { get; set; }

        // FOREIGN KEYS
        [ForeignKey(nameof(Group))]
        public int? GroupId { get; set; }
        [ForeignKey(nameof(Maker))]
        public int? MakerId { get; set; }
        [ForeignKey(nameof(Genre))]
        public int? GenreId { get; set; }

        // NAVIGATION PROPERTIES
        public virtual Group Group { get; set; }
        public virtual Maker Maker { get; set; }
        public virtual Genre Genre { get; set; }
    }
}
