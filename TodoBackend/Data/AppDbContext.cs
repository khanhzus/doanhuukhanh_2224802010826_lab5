using Microsoft.EntityFrameworkCore;
using TodoBackend.Models;

namespace TodoBackend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();

        public DbSet<TodoItem> Todos => Set<TodoItem>();
    }
}