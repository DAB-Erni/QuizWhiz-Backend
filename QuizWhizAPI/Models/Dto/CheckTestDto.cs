namespace QuizWhizAPI.Models.Dto
{
    public class CheckTestDto
    {
        public int CheckTestId { get; set; }
        public int QuestionId { get; set; }
        public int TakeQuizId { get; set; }
        public bool IsCorrect { get; set; }
    }
}
