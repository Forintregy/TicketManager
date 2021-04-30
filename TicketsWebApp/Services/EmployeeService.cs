using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using TicketsWebApp.Models;
using Datalayer;

namespace TicketsWebApp.Services
{
    public class EmployeeService : IEmployeeService
    {
        private IdentityUsersContext _context;

        public EmployeeService(IdentityUsersContext context)
        {
            _context = context;
        }

        public EmployeeInputModel GetEmployee(string id)
        {
            var user = _context.Users.Where(u => string.Equals(u.Id, id)).FirstOrDefault();
            if (user is null) throw new Exception($"Пользователь с {id} не найден");
            return new EmployeeInputModel 
            { 
                Id = user.Id, 
                Name = user.UserName
            };
        }

        public IEnumerable<EmployeeInputModel> GetEmployees()
        {
            return _context.Users
                .Select(u=> new EmployeeInputModel 
                { 
                    Name = u.UserName, 
                    Id = u.Id 
                });
        }
    }

    
}
