using System.ComponentModel.DataAnnotations;

namespace TicketsWebApp.Models
{
    public class EmployeeInputModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
