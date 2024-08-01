namespace QuizWhizAPI.Models.Dto
{
    public class TakeQuizCreateDto
    {
        public string Answer { get; set; }
        public int Score { get; set; }
        public int CreatedQuizId { get; set; }
        public int UserId { get; set; }
    }
}
