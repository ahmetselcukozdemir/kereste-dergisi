using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kereste.DATA.Entities
{
    public class Category
    {
        public int ID { get; set; }
        [StringLength(50)]
        public string? Name { get; set; }
        [StringLength(50)]
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? SelfLink { get; set; }
        public bool isActive { get; set; }

        public virtual ICollection<News> News { get; set; }
    }
}
