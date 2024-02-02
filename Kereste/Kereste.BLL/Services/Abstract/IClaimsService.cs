using Kereste.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kereste.BLL.Services.Abstract
{
    public interface IClaimsService
    {
        public void UpdateClaims(UserDTO user);
    }
}
