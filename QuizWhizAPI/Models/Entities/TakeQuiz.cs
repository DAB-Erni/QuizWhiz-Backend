using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizWhizAPI.Models.Entities
{
    public class TakeQuiz
    {
        [Key]
        public int TakeQuizId { get; set; }

        //public string Answer { get; set; } = string.Empty;
        public List<string> Answer { get; set; } = new List<string>();

        public int Score { get; set; }

        [ForeignKey("CreatedQuiz")]
        public int CreatedQuizId { get; set; }
        public CreatedQuiz CreatedQuiz { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User TakenBy { get; set; }

        // Navigation property for one-to-many relationship with CheckTest
        public ICollection<CheckTest> CheckTests { get; set; } = new List<CheckTest>();
    }
}
