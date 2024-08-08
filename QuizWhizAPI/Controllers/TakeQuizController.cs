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
    public class TakeQuizController : ControllerBase
    {
        private readonly QuizDbContext _context;
        private readonly IMapper _mapper;

        public TakeQuizController(QuizDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TakeQuizDto>>> GetAllTakenQuizzes()
        {
            var takenQuizzes = await _context.TakeQuizzes
                .Include(tq => tq.CreatedQuiz)
                .ThenInclude(cq => cq.Questions)
                .ToListAsync();

            var takeQuizDtos = _mapper.Map<List<TakeQuizDto>>(takenQuizzes);
            return Ok(takeQuizDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TakeQuizDto>> GetTakenQuiz(int id)
        {
            var takenQuiz = await _context.TakeQuizzes
                .Include(tq => tq.CreatedQuiz)
                .ThenInclude(cq => cq.Questions)
                .FirstOrDefaultAsync(tq => tq.TakeQuizId == id);

            if (takenQuiz == null)
            {
                return NotFound(new { Message = "TakeQuiz not found" });
            }

            var takeQuizDto = _mapper.Map<TakeQuizDto>(takenQuiz);
            return Ok(takeQuizDto);
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitQuiz([FromBody] TakeQuizDto takeQuizDto)
        {
            try
            {
                // Retrieve the CreatedQuiz entity
                var createdQuiz = await _context.CreatedQuizzes
                    .Include(cq => cq.Questions)
                    .FirstOrDefaultAsync(cq => cq.CreatedQuizId == takeQuizDto.CreatedQuizId);

                if (createdQuiz == null)
                {
                    return NotFound(new { Message = "CreatedQuiz not found" });
                }

                // Create a new TakeQuiz entry
                var takeQuiz = new TakeQuiz
                {
                    CreatedQuizId = takeQuizDto.CreatedQuizId,
                    UserId = takeQuizDto.UserId,
                    Answer = takeQuizDto.Answer,
                    Score = 0,
                    CreatedQuiz = createdQuiz
                };

                // Compare the user's answers with the correct answers
                int score = 0;
                for (int i = 0; i < takeQuizDto.Answer.Count; i++)
                {
                    var question = createdQuiz.Questions.ElementAt(i);
                    var userAnswer = takeQuizDto.Answer[i];

                    // Check if the answer is correct
                    if (userAnswer.Equals(question.QuestionAnswer, StringComparison.OrdinalIgnoreCase))
                    {
                        score++;
                    }
                }

                // Update the TakeQuiz entity with the score
                takeQuiz.Score = score;

                // Add the new TakeQuiz entry to the database
                _context.TakeQuizzes.Add(takeQuiz);
                await _context.SaveChangesAsync();

                return Ok(new { Score = score });
            }
            catch (DbUpdateException dbEx)
            {
                // Log the detailed error
                var innerException = dbEx.InnerException?.Message ?? dbEx.Message;
                return BadRequest(new { Message = "An error occurred while saving the entity changes.", Details = innerException });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTakenQuiz(int id)
        {
            var takenQuiz = await _context.TakeQuizzes.FindAsync(id);
            if (takenQuiz == null)
            {
                return NotFound(new { Message = "TakeQuiz not found" });
            }

            _context.TakeQuizzes.Remove(takenQuiz);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
