﻿using Kereste.BLL.DTO;
using Kereste.BLL.Services.Abstract;
using Kereste.DATA.Contexts;
using Kereste.DATA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Kereste.BLL.Services.Concrete
{
	public class UserService : IUserService
	{
		public KeresteDBContext _context;

        public UserService(KeresteDBContext context)
        {
			_context = context;
        }
        public bool AddUser(User model)
		{
			if (model != null)
			{
				User user = new User();
				user.Username = model.Username;
				user.Email = model.Email;
				user.Password = model.Password;
				user.isAdmin = model.isAdmin;
				user.IsActive = model.IsActive;
				_context.Users.Add(user);
				_context.SaveChanges();

				return true;
			}
			else
			{
				return false;
			}
		}

		public bool DeleteUser(int userId)
		{
			throw new NotImplementedException();
		}

		public List<UserDTO> GetAllUsers()
		{
			throw new NotImplementedException();
		}

        public List<UserDTO> GetAllUsers(int count)
        {
            List<UserDTO> getList = (from user in _context.Users.Where(t => t.IsActive == true)
                                     orderby user.ID descending
                                     select new UserDTO
                                     {
                                       userID = user.ID,
									   userName = user.Username,
									   email = user.Email,
									   nameSurname = user.NameSurname,
									   isActive = user.IsActive,
									   isAdmin = user.isAdmin,
                                       password = user.Password,
									   image = "https://cdn.kerestedergi.com/users" + user.Image
                                     }).Take(count).ToList();

			return getList;
        }

        public User GetUserById(int id)
        {
           return _context.Users.FirstOrDefault(x => x.ID == id);
        }

        public User GetUserByUser(string username, string password)
		{
			var user = _context.Users.FirstOrDefault(x => x.Username == username && x.Password == password);
			if (user != null)
			{
				return user;
			}
			else
			{
				return null;
			}
			
		}

        public int GetUserCount()
        {
			return _context.Users.Where(x => x.IsActive == true).Count();
        }

        public bool UpdateUser(UserDTO updatedUser)
		{
			var checkUser = _context.Users.FirstOrDefault(x => x.ID == updatedUser.userID);

			if (checkUser != null)
			{
				checkUser.NameSurname = updatedUser.nameSurname;
				checkUser.Username = updatedUser.userName;
				checkUser.Password = updatedUser.password;
				checkUser.Email = updatedUser.email;
				checkUser.Image = updatedUser.image != null ? updatedUser.image : checkUser.Image;
				_context.SaveChanges();

				return true;
			}

			else
			{
				return false;
			}
		}
	}
}
