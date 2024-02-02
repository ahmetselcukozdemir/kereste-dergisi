using Kereste.BLL.DTO;
using Kereste.BLL.Services.Abstract;
using Kereste.DATA.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Kereste.BLL.Services.Concrete
{
    public class ClaimsService : IClaimsService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimsService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public void UpdateClaims(UserDTO user)
        {
            
        }
    }
}
