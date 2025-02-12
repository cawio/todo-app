using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Task = Infrastructure.Entities.Task;

namespace Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public int CurrentUserId { get; set; }
    public DbSet<TodoList> TodoLists => Set<TodoList>();
    public DbSet<Task> Tasks => Set<Task>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<TaskTags> TodoListTags => Set<TaskTags>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoList>().HasQueryFilter(tl => tl.UserId == CurrentUserId);
        modelBuilder.Entity<Task>().HasQueryFilter(t => t.TodoList.UserId == CurrentUserId);
        modelBuilder
            .Entity<TaskTags>()
            .HasQueryFilter(tt => tt.Task.TodoList.UserId == CurrentUserId);
        modelBuilder.Entity<User>().HasQueryFilter(u => u.Id == CurrentUserId);

        modelBuilder.Entity<TaskTags>().HasKey(tt => new { tt.TaskId, tt.TagId });

        modelBuilder
            .Entity<TaskTags>()
            .HasOne(tt => tt.Task)
            .WithMany(t => t.TaskTags)
            .HasForeignKey(tt => tt.TaskId);

        modelBuilder
            .Entity<TaskTags>()
            .HasOne(tt => tt.Tag)
            .WithMany(t => t.TaskTags)
            .HasForeignKey(tt => tt.TagId);
    }
}
