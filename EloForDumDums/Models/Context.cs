using Microsoft.EntityFrameworkCore;

namespace EloForDumDums.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions options) : base(options)
        {

        }

        protected Context()
        {
        }


        public DbSet<Player> Players { get; set; }
    }
}
