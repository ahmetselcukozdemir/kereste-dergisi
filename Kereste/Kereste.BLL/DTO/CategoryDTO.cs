using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kereste.BLL.DTO
{
    public class CategoryDTO
    {
        public int categoryID { get; set; }
        public string categoryName { get; set; }
        public string categoryDesc { get; set; }
        public string categoryImg { get; set; }
        public bool isActive { get; set; }
    }
}
