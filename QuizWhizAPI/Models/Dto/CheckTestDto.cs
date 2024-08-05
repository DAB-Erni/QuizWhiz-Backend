using QuizWhizAPI.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuizWhizAPI.Models.Dto
{
    public class CheckTestDto
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
