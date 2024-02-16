using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kereste.DATA.Entities
{
    public class News
    {
        public int ID { get; set; }
        [StringLength(200)]
        public string Title { get; set; }
        [StringLength(200)]
        public string? AlternativeTitle { get; set; }
        [StringLength(300)]
        public string Spot { get; set; }
        public string Body { get; set; }
        public string? HeadImage { get; set; }
        public string? VerticalImage { get; set; }
        public int Status { get; set; }
        [StringLength(100)]
        public string? Tags { get; set; }
        [StringLength(500)]
        public string SelfLink { get; set; }
        [StringLength(500)]
        public string? ExternalLink { get; set; }
        public DateTime InsertedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime PublishDate { get; set; }

        public virtual User User { get; set; }
        public virtual Category Category { get; set; }
    }
}
