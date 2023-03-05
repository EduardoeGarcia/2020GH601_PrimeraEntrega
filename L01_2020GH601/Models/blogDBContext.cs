using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace L01_2020GH601.Models
{
    public class blogDBContext : DbContext
    {
        public blogDBContext(DbContextOptions<blogDBContext> options) : base(options)
        {

        }

        public DbSet<usuarios> usuarios { get; set; }
        public DbSet<publicaciones> publicaciones { get; set; }
        public DbSet<comentarios> comentarios { get; set; }
    }
}
