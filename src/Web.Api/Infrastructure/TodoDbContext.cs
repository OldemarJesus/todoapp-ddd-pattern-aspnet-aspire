using Microsoft.EntityFrameworkCore;
using Web.Api.Domains;

namespace Web.Api.Infrastructure;

public class TodoDbContext : DbContext
{
    public DbSet<Member> Members { get; set; } = null!;
    public DbSet<TodoItem> TodoItems { get; set; } = null!;

    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TodoDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
