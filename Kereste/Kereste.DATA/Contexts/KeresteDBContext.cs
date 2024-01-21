using Kereste.DATA.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kereste.DATA.Contexts
{
	public partial class KeresteDBContext : DbContext
	{
        public KeresteDBContext()
        {
                
        }

		public KeresteDBContext(DbContextOptions<KeresteDBContext> options)
	  : base(options)
		{
		}

		public virtual DbSet<User> Users { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
	   => optionsBuilder.UseSqlServer("Server=***********;Initial Catalog=***********;Persist Security Info=False;User ID=************;Password=***********;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");
	}
}
