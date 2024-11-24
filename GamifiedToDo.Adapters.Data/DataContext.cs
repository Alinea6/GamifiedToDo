using GamifiedToDo.Adapters.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GamifiedToDo.Adapters.Data;

public class DataContext : DbContext
{
    public DbSet<Chore> Chores { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserLevel> UserLevels { get; set; }
    
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureTasks(modelBuilder);
        ConfigureUsers(modelBuilder);
        ConfigureUserLevels(modelBuilder);
        
        base.OnModelCreating(modelBuilder);
    }

    private void ConfigureTasks(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chore>()
            .HasIndex(c => c.Id)
            .IsUnique();

        modelBuilder.Entity<Chore>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId);
    }

    private void ConfigureUsers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Id)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Login)
            .IsUnique();
    }

    private void ConfigureUserLevels(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserLevel>()
            .HasIndex(u => u.Id)
            .IsUnique();

        modelBuilder.Entity<UserLevel>()
            .HasOne(u => u.User)
            .WithOne(ul => ul.UserLevel)
            .HasForeignKey<User>(x => x.Id);
    }
}