using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketsWebApp.Models
{
    public class DropDownModel
    {
        public IEnumerable<EmployeeInputModel> Employees { get; set; }
        public TicketInputModel TicketData { get; set; }
    }
}
