using Datalayer.Models;
using System.ComponentModel.DataAnnotations;

namespace TicketsWebApp.Models
{
    public class TicketInputModel
    {
        public int? Id { get; set; }
        [Display(Name = "Инициатор")]
        public string AssignerId { get; set; }
        [Display(Name = "Исполнитель")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Не может быть пустой!")]
        public string AssigneeId { get; set; }
        [Display(Name = "Название")]
        [MinLength(1)]
        [Required(AllowEmptyStrings =false, ErrorMessage = "Не может быть пустой!")]
        public string Name { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Display(Name = "Статус", Description = "Статус задачи")]
        public TicketState State { get; set; }
        [Display(Name = "Приоритет", Description = "Приоритет задачи")]
        public TicketPriority Priority { get; set; }
    }

}
