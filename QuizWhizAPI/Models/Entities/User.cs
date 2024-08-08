using System.ComponentModel.DataAnnotations;

namespace QuizWhizAPI.Models.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public  string UserName { get; set; } = string.Empty;

        [Required]
        public  string Password { get; set; } = string.Empty;

        [Required]
        public  string Role { get; set; } = string.Empty;

        // Navigation property for CreatedQuiz
        public ICollection<CreatedQuiz> CreatedQuizzes { get; set; } = new List<CreatedQuiz>();

        // Navigation property for TakeQuiz
        public ICollection<TakeQuiz> TakeQuizzes { get; set; } = new List<TakeQuiz>();

    }
}
