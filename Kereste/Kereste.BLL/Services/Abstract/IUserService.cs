using Kereste.BLL.DTO;
using Kereste.DATA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kereste.BLL.Services.Abstract
{
	public interface IUserService
	{
		bool AddUser(User user);

		bool DeleteUser(int userId);

		UserDTO UpdateUser(int userId, UserDTO updatedUser);

		List<UserDTO> GetAllUsers();

		User GetUserByUser(string username,string password);
        User GetUserById(int id);
    }
}
