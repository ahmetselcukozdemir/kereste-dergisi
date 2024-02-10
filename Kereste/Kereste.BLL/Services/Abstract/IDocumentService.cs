using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kereste.BLL.Services.Abstract
{
    public interface IDocumentService
    {
        public string CopyImage(IFormFile file, string path);
        public string CopyImageProfile(IFormFile file, string path);
    }
}
