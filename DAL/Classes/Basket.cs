using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    public class Basket
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountOfTrecks { get; set; }
        public int PublishYear { get; set; }
        public double LocalPrice { get; set; }
        public double Price { get; set; }
        public double? BeforeDiscountPrice { get; set; } = null;
        public int SoldCopies { get; set; }

        public int? GroupId { get; set; }
        public int? MakerId { get; set; }
        public int? UserId { get; set; }
        public int? GenreId { get; set; }
    }
}
