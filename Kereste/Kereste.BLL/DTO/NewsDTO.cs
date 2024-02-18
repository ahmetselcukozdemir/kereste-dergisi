using Kereste.DATA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kereste.BLL.DTO
{
    public class NewsDTO
    {
        public int NewsID { get; set; }
        public Category Category { get; set; }
        public User User { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Title { get; set; }
        public string AlternativeTitle { get; set; }
        public string Spot { get; set; }
        public string Body { get; set; }
        public string HeadImage { get; set; }
        public string DetailImage { get; set; }
        public string Tags { get; set; }
        public int Status { get; set; }
        public string External { get; set; }
        public string SelfLink { get; set; }
    }
}
