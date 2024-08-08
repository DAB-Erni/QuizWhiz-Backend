namespace QuizWhizAPI.Models.Dto
{
    public class TakeQuizDto
    {
        public int CreatedQuizId { get; set; }
        public int UserId { get; set; }
        public List<string> Answer { get; set; } = new List<string>();
    }
}
