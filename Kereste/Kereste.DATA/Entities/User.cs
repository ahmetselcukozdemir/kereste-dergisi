using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kereste.DATA.Entities
{
	public class User
	{
        public int ID { get; set; }
		[StringLength(30)]
		public string? Username { get; set; }
		[StringLength(50)]
		public string? Password { get; set; }
		[StringLength(50)]
        public string? NameSurname { get; set; }
        [StringLength(50)]
        public DateTime? Birthday { get; set; }
        [StringLength(50)]
        public string? Email { get; set; }
        public string? Image { get; set; }
        public bool IsActive { get; set; }
        public bool isAdmin { get; set; }
    }
}
