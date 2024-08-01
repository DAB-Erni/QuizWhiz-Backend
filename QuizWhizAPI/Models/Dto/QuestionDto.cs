namespace QuizWhizAPI.Models.Dto
{
    public class QuestionDto
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string QuestionAnswer { get; set; }
        public int CreatedQuizId { get; set; }

    }
}
