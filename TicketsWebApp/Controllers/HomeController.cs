using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using TicketsWebApp.Helpers;
using TicketsWebApp.Services;
using TicketsWebApp.Models;

namespace TicketsWebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ITicketService _ticketsService;
        private IEmployeeService _employeeService;
        private ILogger _logger;

        public HomeController(ITicketService ticketsService, 
            IEmployeeService employeeService, 
            ILogger<HomeController> logger)
        {
            _ticketsService = ticketsService;
            _employeeService = employeeService;
            _logger = logger;
        }

        [Route("[controller]/getticket/{ticketId}")]
        public IActionResult GetTicket(int ticketId)
        {
            if (ticketId<1)
                throw new Exception("Идентификатор тикета указан неверно!");
            try
            {
                var result = _ticketsService.GetTicket(ticketId);
                return PartialView(new TicketInputModel
                {
                    AssigneeId = _employeeService.GetEmployee(result.AssigneeId.ToString()).Name,
                    AssignerId = _employeeService.GetEmployee(result.AssigneeId.ToString()).Name,
                    Description = result.Description,
                    Id = result.Id,
                    Name = result.Name,
                    Priority = result.Priority,
                    State = result.State
                });
            }
            catch (Exception ex)
            {
                throw HelperMethods.LogAndThrow($"Ошибка при получении тикета с Id {ticketId}", ex, _logger);
            }
        }

        [Authorize]
        public IActionResult Index()
        {
            try
            {
                var result = _ticketsService.GetTickets().Select(t=>
                    new TicketInputModel 
                    {
                         AssigneeId= t.AssigneeId,
                         AssignerId = t.AssignerId,
                         Description = t.Description,
                         Id = t.Id,
                         Name = t.Name,
                         Priority = t.Priority,
                         State = t.State
                    })
                    .ToList();
                return View(result);
            }
            catch (Exception ex)
            {
                throw HelperMethods.LogAndThrow($"Ошибка при получении списка тикетов", ex, _logger);
            }
        }
        public IActionResult EditTicket(int id)
        {
            try 
            {
                ViewData["Users"] = new SelectList(_employeeService.GetEmployees(), "Id", "Name");
                var ticket = _ticketsService.GetTicket(id);
                return View(
                    new TicketInputModel 
                    {
                        AssigneeId = ticket.AssigneeId,
                        AssignerId = ticket.AssignerId,
                        Description = ticket.Description,
                        Id = ticket.Id,
                        Name = ticket.Name,
                        Priority = ticket.Priority,
                        State = ticket.State
                    });
            }
            catch (Exception ex)
            {
                throw HelperMethods.LogAndThrow($"При попытке получения тикета с Id {id} произошла ошибка:", ex, _logger);
            }
        }

        [HttpPost]
        public IActionResult EditTicket(TicketInputModel model)
        {
            model.AssignerId = User.FindFirst("sub").Value;
            if (!TryValidateModel(model))
                throw new Exception("Поля заполнены неверно!");
            try
            {
                _ticketsService.AddOrUpdateTicket(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw HelperMethods.LogAndThrow($"При попытке обновления тикета произошла ошибка:", ex, _logger);
            }
        }

        [Authorize(Roles = "manager")]
        public IActionResult CreateTicket()
        {
            ViewData["Users"] = new SelectList(_employeeService.GetEmployees(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "manager")]
        public IActionResult CreateTicket(TicketInputModel model)
        {
            model.AssignerId = User.FindFirst("sub").Value;
            if (!TryValidateModel(model))
                throw new Exception("Поля заполнены неверно!");
            if (_employeeService.GetEmployee(model.AssigneeId) == null)
                throw new Exception($"Пользователь с id {model.AssigneeId} не найден");
            if (_employeeService.GetEmployee(model.AssignerId) == null)
                throw new Exception($"Пользователь с id {model.AssignerId} не найден");
            try
            {
                _ticketsService.AddOrUpdateTicket(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw HelperMethods.LogAndThrow("При попытке добавления тикета произошла ошибка", ex, _logger);
            }
        }
    }
}
