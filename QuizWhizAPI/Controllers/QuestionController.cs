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
        public async Task<ActionResult<QuestionDto>> GetQuestion(int id)
        {
            var question = await _context.Questions.FindAsync(id);

            if (question == null)
            {
                return NotFound();
            }

            return _mapper.Map<QuestionDto>(question);
        }

        [HttpPost]
        public async Task<ActionResult<QuestionDto>> CreateQuestion(QuestionCreateUpdateDto dto)
        {
            var createdQuiz = await _context.CreatedQuizzes.FindAsync(dto.CreatedQuizId);
            if (createdQuiz == null)
            {
                return BadRequest("Related quiz not found");
            }

            var question = new Question
            {
                QuestionText = dto.QuestionText,
                QuestionAnswer = dto.QuestionAnswer,
                CreatedQuizId = dto.CreatedQuizId,
                CreatedQuiz = createdQuiz
            };

            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            var questionDto = _mapper.Map<QuestionDto>(question);

            return CreatedAtAction(nameof(GetQuestion), new { id = question.QuestionId }, questionDto);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateQuestion(int id, QuestionCreateUpdateDto dto)
        {
            var existingQuestion = await _context.Questions
                .Include(cq => cq.CreatedQuiz)
                .FirstOrDefaultAsync(cq => cq.QuestionId == id);

            if (existingQuestion == null)
            {
                return NotFound();
            }

            var createdQuiz = await _context.CreatedQuizzes.FindAsync(dto.CreatedQuizId);
            if (createdQuiz == null) {
                return BadRequest("Related Quiz not found");
            }

            existingQuestion.QuestionText = dto.QuestionText;
            existingQuestion.QuestionAnswer = dto.QuestionAnswer;
            existingQuestion.CreatedQuizId = dto.CreatedQuizId;
            existingQuestion.CreatedQuiz = createdQuiz;

            try
            {
                await _context.SaveChangesAsync();
            }

            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(id))
                {
                    return NotFound();
                } else
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

        [HttpDelete("{id}")]
        public async Task<ActionResult<QuestionDto>> DeleteQuestion(int id)
        {
            var question = await _context.Questions.FindAsync(id);

            if (question == null)
            {
                return NotFound();
            }

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();

            return Ok(question);
        }

    }
}
