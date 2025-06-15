using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions) { }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<TodoTask> TodoTasks { get; set; } = null!;
}