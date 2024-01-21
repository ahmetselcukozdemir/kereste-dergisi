using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kereste.BLL.DTO
{
	public class UserDTO
	{
        public int userID { get; set; }
        public string userName { get; set; }
		public string email { get; set; }
		public string password { get; set; }
		public bool isActive { get; set; }
        public bool isAdmin { get; set; }
    }
}
