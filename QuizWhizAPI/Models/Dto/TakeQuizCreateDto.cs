using System.ComponentModel.DataAnnotations.Schema;

namespace QuizWhizAPI.Models.Dto
{
    public class TakeQuizCreateDto
    {
        //public string Answer { get; set; } = string.Empty;
        public int Score { get; set; }
        public int CreatedQuizId { get; set; }
        public int UserId { get; set; }
        public List<string> Answer { get; set; } = new List<string>();
        public List<CheckTestDto> CheckTests { get; set; }
    }
}
