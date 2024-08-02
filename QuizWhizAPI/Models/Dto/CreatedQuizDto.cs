using QuizWhizAPI.Models.Entities;

namespace QuizWhizAPI.Models.Dto
{
    public class CreatedQuizDto
    {
        public int CreatedQuizId { get; set; }
        public string Title { get; set; }
        public int UserId { get; set; }
        public string CreatedBy { get; set; }
        public ICollection<QuestionDto> Questions { get; set; }

    }
}
