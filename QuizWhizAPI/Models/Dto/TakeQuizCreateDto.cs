﻿namespace QuizWhizAPI.Models.Dto
{
    public class TakeQuizCreateDto
    {
        //public string Answer { get; set; } = string.Empty;
        public List<string> Answer { get; set; } = new List<string>();
        public int Score { get; set; }
        public int CreatedQuizId { get; set; }
        public int UserId { get; set; }
    }
}
