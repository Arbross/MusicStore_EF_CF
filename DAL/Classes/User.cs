using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public int? PermanentDiscount { get; set; }

        // NAVIGATION PROPERTIES
        public virtual ICollection<SoldPlate> Sold { get; set; }
        public virtual ICollection<Basket> Basket { get; set; }
    }
}
