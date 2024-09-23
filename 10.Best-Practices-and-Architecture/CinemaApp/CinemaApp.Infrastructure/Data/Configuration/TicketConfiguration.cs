using CinemaApp.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaApp.Infrastructure.Data.Configuration
{
    internal class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            List<Ticket> tickets = new List<Ticket>();
            int id = 0;
            Random rand = new Random();

            for (int i = 1; i < 8; i++)
            {
                int rows = rand.Next(10, 15);
                int seatCount = rand.Next(10, 20);

            }


            builder.HasData(tickets);
        }
    }
}
