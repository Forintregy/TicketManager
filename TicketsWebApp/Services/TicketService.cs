using Datalayer;
using Datalayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TicketsWebApp.Models;

namespace TicketsWebApp.Services
{
    public class TicketService : ITicketService
    {
        private TicketsDbContext _context;

        public TicketService(TicketsDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Ticket> GetTickets()
        {
            return _context.Tickets;
        }

        public Ticket GetTicket(int id)
        {
            var result =  _context.Tickets.Where(t=>t.Id==id).FirstOrDefault();
            return result;
        }

        public void ChangeState(int id, TicketState state)
        {
            var ticket = GetTicket(id);
            if (ticket is null) 
                throw new InvalidOperationException($"Задача с идентификатором {id} не найдена!");

            ticket.State = state;
            _context.Tickets.Update(ticket);
            _context.SaveChanges();
        }

        public void ChangePriority(int id, TicketPriority priority)
        {
            var ticket = GetTicket(id);
            if (ticket is null)
                throw new InvalidOperationException($"Задача с идентификатором {id} не найдена!");

            ticket.Priority = priority;
            _context.Entry(ticket).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public  void RemoveTicket(int id)
        {
            var ticket = GetTicket(id);
            if (ticket is null)
                throw new InvalidOperationException($"Задача с идентификатором {id} не найдена!");

            _context.Entry(ticket).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        private Ticket UpdateTicket(TicketInputModel ticket)
        {
            var result = 
                _context.Tickets.Where(t => t.Id == ticket.Id).FirstOrDefaultAsync().Result;
            if (result is null)
                throw new InvalidOperationException($"Задача с идентификатором {ticket.Id} не найдена!");

            result.AssigneeId = ticket.AssigneeId;
            result.AssignerId = ticket.AssignerId;
            result.Description = ticket.Description;
            result.Name = ticket.Name;
            result.Priority = ticket.Priority;
            result.State = ticket.State;            
            _context.Entry(result).State = EntityState.Modified;
            _context.SaveChanges();
            return result;
        }

        private Ticket AddTicket(TicketInputModel ticket)
        {
            var result = new Ticket
            {
                AssigneeId = ticket.AssigneeId,
                AssignerId = ticket.AssignerId,
                Description = ticket.Description,
                Name = ticket.Name,
                Priority = ticket.Priority,
                State = ticket.State
            };
            _context.Add(result);
            _context.SaveChangesAsync().GetAwaiter();
            return result;
        }

        public Ticket AddOrUpdateTicket(TicketInputModel ticket)
        {
            if ((ticket.Id ?? 0) == 0)
                return AddTicket(ticket);
            return UpdateTicket(ticket);
        }

        public void AssignEmployee(int ticketId, string employeeId)
        {
            var ticket = GetTicket(ticketId);
            ticket.AssigneeId = employeeId;
            _context.Entry(ticket).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
