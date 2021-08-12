using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    public class SoldPlate
    {
        public int Id { get; set; }
        [Required, MaxLength(30)]
        public string Name { get; set; }
        public int CountOfTrecks { get; set; }
        public int PublishYear { get; set; }
        public double LocalPrice { get; set; }
        public double Price { get; set; }
        public double? BeforeDiscountPrice { get; set; }
        public int SoldCopies { get; set; }
        public DateTime SoldTime { get; set; }
        public bool IsAvailable { get; set; }

        public int? UserId { get; set; }
        public int? GroupId { get; set; }
        public int? MakerId { get; set; }
        public int? GenreId { get; set; }
    }
}
