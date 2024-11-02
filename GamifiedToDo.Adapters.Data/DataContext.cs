using GamifiedToDo.Adapters.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GamifiedToDo.Adapters.Data;

public class DataContext : DbContext
{
    public DbSet<Chore> Chores { get; set; }
    
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureTasks(modelBuilder);
        
        base.OnModelCreating(modelBuilder);
    }

    private void ConfigureTasks(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chore>()
            .HasIndex(c => c.Id)
            .IsUnique();
    }
}