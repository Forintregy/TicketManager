using Datalayer.Models;
using Microsoft.EntityFrameworkCore;

namespace Datalayer
{
    public class TicketsDbContext : DbContext
    {
        public TicketsDbContext()
        {}
        public TicketsDbContext(DbContextOptions<TicketsDbContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
            //if (Tickets.FirstOrDefaultAsync().Result == null)
            //{
            //    Tickets.Add(new Ticket 
            //    { 
            //        Description = "Проверить работу приложения", 
            //        Name = "Важная задача", 
            //        State = TicketState.Issued, 
            //        Priority = TicketPriority.Critical, 
            //        AssigneeId = "1", 
            //        AssignerId = "2" 
            //    });
            //    this.SaveChanges();
            //}
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TicketsAppDb;Trusted_Connection=True;");
        }
        public DbSet<Ticket> Tickets { get; set; }
    }
}
