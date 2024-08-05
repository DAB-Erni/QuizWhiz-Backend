using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizWhizAPI.Models.Entities
{
    public class CheckTest
    {
        [Key]
        public int CheckTestId { get; set; }

        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        public Question Question { get; set; }

        [ForeignKey("TakeQuiz")]
        public int TakeQuizId { get; set; }
        public TakeQuiz TakeQuiz { get; set; }

        [Required]
        public string Answer { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }

    }
}
