using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizWhizAPI.Data;
using QuizWhizAPI.Models.Dto;
using QuizWhizAPI.Models.Entities;

namespace QuizWhizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly QuizDbContext _context;
        private readonly IMapper _mapper;

        public QuestionController(QuizDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetAllQuestions()
        {
            var questions = await _context.Questions.ToListAsync();
            return _mapper.Map<List<QuestionDto>>(questions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionDto>> GetQuestionById(int id)
        {
            var question = await _context.Questions.FindAsync(id);

            if (question == null)
            {
                return NotFound();
            }

            return _mapper.Map<QuestionDto>(question);
        }

        [HttpPost("{quizId}/questions")]
        public async Task<IActionResult> AddQuestionToQuiz(int quizId, QuestionCreateUpdateDto dto)
        {
            var quiz = await _context.CreatedQuizzes
                .Include(cq => cq.Questions)
                .FirstOrDefaultAsync(cq => cq.CreatedQuizId == quizId);

            if (quiz == null)
            {
                return NotFound();
            }

            var question = new Question
            {
                QuestionText = dto.QuestionText,
                QuestionAnswer = dto.QuestionAnswer,
                CreatedQuizId = quizId
            };

            quiz.Questions.Add(question);
            await _context.SaveChangesAsync();

            var questionDto = _mapper.Map<QuestionDto>(question);
            return CreatedAtAction(nameof(GetQuestionById), new { id = question.QuestionId }, questionDto);
        }

        [HttpPut("{quizId}/questions/{questionId}")]
        public async Task<IActionResult> UpdateQuestion(int quizId, int questionId, QuestionCreateUpdateDto dto)
        {
            var existingQuestion = await _context.Questions
                .Include(cq => cq.CreatedQuiz)
                .FirstOrDefaultAsync(cq => cq.QuestionId == questionId);

            if (existingQuestion == null)
            {
                Console.WriteLine("Question not found"); // Debugging log
                return NotFound();
            }

            existingQuestion.QuestionText = dto.QuestionText;
            existingQuestion.QuestionAnswer = dto.QuestionAnswer;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(questionId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        private bool QuestionExists(int id)
        {
            return _context.Questions.Any(e => e.QuestionId == id);
        }

        [HttpDelete("{quizId}/questions/{questionId}")]
        public async Task<ActionResult<QuestionDto>> DeleteQuestion(int quizId, int questionId)
        {
            var question = await _context.Questions
                    .FirstOrDefaultAsync(q => q.QuestionId == questionId && q.CreatedQuizId == quizId);

            if (question == null)
            {
                return NotFound();
            }

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
