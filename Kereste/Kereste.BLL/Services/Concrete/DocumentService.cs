using Kereste.BLL.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kereste.BLL.Services.Concrete
{
    public class DocumentService : IDocumentService
    {
        private readonly IConfiguration _config;
        public DocumentService(IConfiguration config)
        {
            _config = config;
        }
        private class CreateDirectory
        {
            public string returnUrl(string filePath, string year, string month, string day)
            {

                //@"C:/Foto/"
                if (!Directory.Exists(filePath + year))
                {
                    Directory.CreateDirectory(filePath + year);
                }
                if (!Directory.Exists(filePath + year + "/" + month))
                {
                    Directory.CreateDirectory(filePath + year + "/" + month);
                }
                if (!Directory.Exists(filePath + year + "/" + month + "/" + day))
                {
                    Directory.CreateDirectory(filePath + year + "/" + month + "/" + day);
                }

                return filePath + year + "/" + month + "/" + day;


            }
        }
        public string CopyImage(IFormFile file, string path)
        {
            string tempImagePath = "";
            string returnImagePath = "";

            string year = DateTime.Now.Year.ToString();
            string month = ("00" + DateTime.Now.Month.ToString()).Substring(("00" + DateTime.Now.Month.ToString()).Length - 2);
            string day = ("00" + DateTime.Now.Day.ToString()).Substring(("00" + DateTime.Now.Day.ToString()).Length - 2);

            if (file.ContentType.ToLower().Contains("jpg") || file.ContentType.ToLower().Contains("png") || file.ContentType.ToLower().Contains("jpeg"))
            {
                var qq = "." + file.FileName.Split('.').Last();
                tempImagePath = Guid.NewGuid().ToString().Substring(0, 8) + qq;

                CreateDirectory cd = new CreateDirectory();
                string _directory = cd.returnUrl(_config[path], year, month, day);



                var filePath = Path.Combine(_directory, tempImagePath);
                returnImagePath = year + "/" + month + "/" + day + "/" + tempImagePath;
                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }
            }
            return returnImagePath;
        }

        public string CopyImageProfile(IFormFile file, string path)
        {
            string tempImagePath = "";
            string returnImagePath = "";


            if (file.ContentType.ToLower().Contains("jpg") || file.ContentType.ToLower().Contains("png") || file.ContentType.ToLower().Contains("jpeg"))
            {
                var qq = "." + file.FileName.Split('.').Last();
                tempImagePath = Guid.NewGuid().ToString().Substring(0, 8) + qq;

                CreateDirectory cd = new CreateDirectory();
                string _directory = _config[path];

                var filePath = Path.Combine(_directory, tempImagePath);
                returnImagePath =  tempImagePath;
                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }
            }
            return returnImagePath;
        }
    }
}
