using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketsWebApp.Models;

namespace TicketsWebApp.Services
{
    public interface IEmployeeService
    {
        EmployeeInputModel GetEmployee(string id);
        IEnumerable<EmployeeInputModel> GetEmployees();
    }
}
