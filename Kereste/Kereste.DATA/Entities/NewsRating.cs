using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kereste.DATA.Entities
{
    public class NewsRating
    {
        public int ID { get; set; }
        public int NewsID { get; set; }
        public int Hit { get; set; }

        public virtual News News { get; set; }
    }
}
