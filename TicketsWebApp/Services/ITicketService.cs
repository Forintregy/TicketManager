using Datalayer.Models;
using System.Collections.Generic;
using TicketsWebApp.Models;

namespace TicketsWebApp.Services
{
    public interface ITicketService
    {
        Ticket GetTicket(int id);
        IEnumerable<Ticket> GetTickets();
        void ChangePriority(int id, TicketPriority priority);
        void ChangeState(int id, TicketState state);
        void AssignEmployee(int ticketId, string employeeId);
        void RemoveTicket(int id);
        Ticket AddOrUpdateTicket(TicketInputModel ticket);

    }
}
