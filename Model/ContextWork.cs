using Microsoft.EntityFrameworkCore;

namespace Redis.Model
{
    public class ContextWork : DbContext
    {
        public ContextWork(DbContextOptions options) : base(options)
        {
        }

        DbSet<Worker> Workers { get; set; }


    }
}
