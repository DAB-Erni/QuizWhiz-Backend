using Microsoft.EntityFrameworkCore;
using QuizWhizAPI.Models.Entities;

namespace QuizWhizAPI.Data
{
    public class QuizDbContext : DbContext
    {
        public QuizDbContext(DbContextOptions<QuizDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<CreatedQuiz> CreatedQuizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<TakeQuiz> TakeQuizzes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User to CreatedQuiz: One-to-Many relationship
            modelBuilder.Entity<User>()
                .HasMany(u => u.CreatedQuizzes)
                .WithOne(cq => cq.CreatedBy)
                .HasForeignKey(cq => cq.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            // CreatedQuiz to Question: One-to-Many relationship
            modelBuilder.Entity<CreatedQuiz>()
                .HasMany(cq => cq.Questions)
                .WithOne(q => q.CreatedQuiz)
                .HasForeignKey(q => q.CreatedQuizId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete

            // CreatedQuiz to TakeQuiz: One-to-Many relationship
            modelBuilder.Entity<CreatedQuiz>()
                .HasMany(cq => cq.TakeQuizzes)
                .WithOne(tq => tq.CreatedQuiz)
                .HasForeignKey(tq => tq.CreatedQuizId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete

            // User to TakeQuiz: One-to-Many relationship
            modelBuilder.Entity<User>()
                .HasMany(u => u.TakeQuizzes)
                .WithOne(tq => tq.TakenBy)
                .HasForeignKey(tq => tq.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
        }
    }
}