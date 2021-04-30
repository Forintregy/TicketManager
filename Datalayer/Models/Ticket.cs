using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Datalayer.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }
        public string AssignerId { get; set; }
        public string AssigneeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TicketState State { get; set; }
        public TicketPriority Priority { get; set; }        
    }

    public enum TicketState
    { 
        [Display(Name ="Выдан")]
        Issued,
        [Display(Name = "В работе")]
        InProcess,
        [Display(Name = "Завершён")]
        Completed
    }

    public enum TicketPriority
    {
        [Display(Name = "Нормальный", Description = "Нормальный")]
        Normal,
        [Display(Name = "Критический", Description = "Критический")]
        Critical
    }
}

