﻿using System;
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
		[StringLength(20)]
		public string? Password { get; set; }
		[StringLength(30)]
		public string? Email { get; set; }
        public string? Image { get; set; }
        public bool IsActive { get; set; }
        public bool isAdmin { get; set; }
    }
}