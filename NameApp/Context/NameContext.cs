using Microsoft.EntityFrameworkCore;
using NameApp.Model;

namespace NameApp.Context
{
    public class NameContext : DbContext
    {
        public NameContext(DbContextOptions<NameContext> options)
            : base(options)
        { }
        public DbSet<Names> Names { get; set; }
    }
}
